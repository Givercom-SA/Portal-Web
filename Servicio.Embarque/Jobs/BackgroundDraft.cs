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

namespace Servicio.Embarque.Jobs
{
    public class BackgroundDraft : IHostedService, IDisposable
    {
        private int number = 0;
        private Timer timer;

        private readonly INotificacionArriboRepository _repository;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioMessage _servicioMessage;
        
        private IConfiguration _configuration;

        public BackgroundDraft(INotificacionArriboRepository repository,
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

                var lista = _repository.ListaNotificacionesArriboPendientes()
                                .ListaNotificacionesPendientes
                                .Where(w => w.NOTARR_TIPO_DOCUMENTO == "BD")
                                .ToList();

               // string pattern = "BL-{0}*";

                foreach (var item in lista)
                {
                    var user = _serviceUsuario.ObtenerUsuarioPorId(item.NOTARR_IDUSUARIO_CREA);

                    if (user.Result != null)
                    {
                        string rutaNotificacionesPendiente = _configuration["DraftCarpeta:PendientesPath"];
                        string rutaNotificacionesProcesadas = _configuration["DraftCarpeta:ProcesadasPath"];
                      
                        
                        var listaArchivos = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles();

                        foreach (var archivo in listaArchivos)
                        {
                            var keyBldArchivo = archivo.Name.Split('-')[1];

                            if (keyBldArchivo.Equals(item.NOTARR_KEYBLD))
                            {
                                int fileExtPos = archivo.Name.LastIndexOf(".");
                                string fileName = archivo.Name;
                                if (fileExtPos >= 0)
                                    fileName = fileName.Substring(0, fileExtPos);

                                //Se envia la información al servicio que se ha actualizado y enviado
                                var resultActualizacion = _serviceEmbarques.ActualizarDocumento(fileName, user.Result.Usuario.Correo);

                                if (resultActualizacion.Result == 1)
                                {
                                    EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                                    enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                                    enviarMessageCorreoParameterVM.RequestMessage.Contenido = item.NOTARR_KEYBLD;
                                    enviarMessageCorreoParameterVM.RequestMessage.Correo = user.Result.Usuario.Correo;
                                    enviarMessageCorreoParameterVM.RequestMessage.Asunto = string.Format("Contra Originales - Numeración de Embarque: ", item.NOTARR_KEYBLD);
                                    enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                                    enviarMessageCorreoParameterVM.RequestMessage.Archivos[0]= string.Format("{0}/{1}", rutaNotificacionesPendiente, archivo.Name);

                                    var ressult =  _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);

                                    //Se actualiza el registro indicando que ya se proceso
                                    _repository.ActualizarEstadoNotificacion(item.NOTARR_KEYBLD, item.NOTARR_IDUSUARIO_CREA, "BD");

                                    //Los movemos a la otra ruta
                                    File.Move(archivo.FullName, string.Format("{0}/{1}", rutaNotificacionesProcesadas, archivo.Name));
                                }
                            }
                        }
                    }
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
