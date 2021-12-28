using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Servicio.Embarque.Repositorio;
using Servicio.Embarque.ServiceConsumer;
using Servicio.Embarque.ServiceExterno;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.Message;
using ViewModel.Datos.UsuarioRegistro;

namespace Servicio.Embarque.Jobs
{
    public class BackgroundNotificacionArribo : IHostedService, IDisposable
    {
        private int number = 0;
        private Timer timer;

        private readonly INotificacionArriboRepository _repository;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioMessage _servicioMessage;
        private IConfiguration _configuration;

        public BackgroundNotificacionArribo(INotificacionArriboRepository repository,
            ServicioUsuario serviceUsuario,
            ServicioEmbarques serviceEmbarques,
            IConfiguration configuration,
            ServicioMessage servicioMessage)
        {
            _repository = repository;
            _serviceUsuario = serviceUsuario;
            _serviceEmbarques = serviceEmbarques;
            _configuration = configuration;
            _servicioMessage = servicioMessage;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(o =>
            {
                Interlocked.Increment(ref number);
                Console.WriteLine($"Printing from worker: {number}");
                try
                {
                    string pattern = "NA-{0}*";

                    var lista = _repository.ListaNotificacionesArriboPendientes().ListaNotificacionesPendientes;

                    if (lista != null)
                    {
                        lista = lista.Where(w => w.NOTARR_TIPO_DOCUMENTO == "NA")
                                        .ToList();



                        foreach (var item in lista)
                        {
                            var user = _serviceUsuario.ObtenerUsuarioPorId(item.NOTARR_IDUSUARIO_CREA);

                            if (user.Result != null)
                            {
                                string rutaNotificacionesPendiente = _configuration["NotificacionCarpeta:PendientesPath"];
                                string rutaNotificacionesProcesadas = _configuration["NotificacionCarpeta:ProcesadasPath"];


                                var archivo = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles(string.Format(pattern, item.NOTARR_KEYBLD)).FirstOrDefault();

                                if (archivo != null)
                                {

                                    int fileExtPos = archivo.Name.LastIndexOf(".");
                                    string fileName = archivo.Name;
                                    if (fileExtPos >= 0)
                                        fileName = fileName.Substring(0, fileExtPos);


                                    //Se envia la información al servicio que se ha actualizado y enviado
                                    var resultActualizacion = _serviceEmbarques.ActualizarDocumento(fileName, user.Result.Usuario.Correo);

                                    if (resultActualizacion.Result == 1)
                                    {


                                        string strCliente = "";
                                        

                                        if (user.Result.Usuario.EntidadTipoDocumento.Equals("RUC")) {
                                            strCliente = user.Result.Usuario.getNombre();
                                        }
                                        else {
                                            strCliente = user.Result.Usuario.EntidadRazonSocial;
                                        }
                                        EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                                        enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                                        enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyNotificacionArribo(strCliente, item.NOTARR_NUMERACION_EMBARQUE, item.GTEM_NOMBRES, Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupoUrl);
                                        enviarMessageCorreoParameterVM.RequestMessage.Correo = user.Result.Usuario.Correo;
                                        enviarMessageCorreoParameterVM.RequestMessage.Asunto = string.Format("Notificación de Arribo - Numeración de  Embarque: {0}", item.NOTARR_NUMERACION_EMBARQUE);
                                        enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                                        enviarMessageCorreoParameterVM.RequestMessage.Archivos[0]= string.Format("{0}/{1}", rutaNotificacionesPendiente, archivo.Name);
                                        var ressult =  _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
                                        //Se actualiza el registro indicando que ya se proceso
                                        _repository.ActualizarEstadoNotificacion(item.NOTARR_KEYBLD, item.NOTARR_IDUSUARIO_CREA, "NA");

                                        //Los movemos a la otra ruta
                                        File.Move(archivo.FullName, string.Format("{0}/{1}", rutaNotificacionesProcesadas, archivo.Name));
                                    }

                                }
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error from Memo worker: {ex.Message}");
                }

            },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
