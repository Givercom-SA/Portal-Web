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
using ViewModel.Datos.UsuarioRegistro;
using Servicio.Embarque.Models.GestionarMemo;
using ViewModel.Datos.Message;
using Servicio.Embarque.Models.SolicitudFacturacion;

namespace Servicio.Embarque.Jobs
{
    public class BackgroundFacturar : IHostedService, IDisposable
    {
        private int number = 0;
        private Timer timer;

        private readonly IMemoRepository _repository;
        private readonly ISolicitudFacturacionRepository _repositoryFacturacion;
        private readonly ServicioUsuario _serviceUsuario;
        
        private readonly ServicioEmbarques _serviceEmbarques;
        private IConfiguration _configuration;
        
       
        private readonly ServicioMessage _servicioMessage;


        public BackgroundFacturar(IMemoRepository repository,
            ServicioUsuario serviceUsuario,
            ServicioEmbarques serviceEmbarques,
            IConfiguration configuration,
            ServicioMessage servicioMessage,
            ISolicitudFacturacionRepository repositoryFacturacion)
        {
            _repository = repository;
            _serviceUsuario = serviceUsuario;
            _serviceEmbarques = serviceEmbarques;
            _configuration = configuration;
            _servicioMessage = servicioMessage;
            _repositoryFacturacion = repositoryFacturacion;
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
                Console.WriteLine($"Facturacion job: {number}");
                string rutaNotificacionesPendiente = _configuration["TrabajadoCarpeta:PendientePath"];
                string rutaNotificacionesProcesadas = _configuration["TrabajadoCarpeta:ProcesadasPath"];
                string rutaNotificacionesProcesando = _configuration["TrabajadoCarpeta:ProcesandoPath"];

                List<FileFacturacion> listRutas = new List<FileFacturacion>();
                var archivosActuales = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles();

                foreach (var item in archivosActuales)
                {
                    try
                    {
                        File.Move(item.FullName, string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name));
                        listRutas.Add(new FileFacturacion() { Name = item.Name, FullName = item.FullName });
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine($"Error from Memo worker: {err.Message}");
                    }
                    finally
                    {

                    }
                }
                // fin pasar a carpeta de procesado

                foreach (var item in listRutas)
                {
                    try
                    {
                        int fileExtPos = item.Name.LastIndexOf(".");
                        string fileName = item.Name;
                        if (fileExtPos >= 0)
                            fileName = fileName.Substring(0, fileExtPos);

                        string KeyBLFull = Path.GetFileName(item.Name).ToUpper().Replace("BT_", "");
                        string[] arrayKeyBl = KeyBLFull.Split('_');
                        string KeyBL = arrayKeyBl[0];

                        var resultFacturacion = _repositoryFacturacion.ListarSolicitudFacturacionPorKeyBl(KeyBL);
                        if (resultFacturacion?.IN_CODIGO_RESULTADO == 0)
                        {
                            if (resultFacturacion.SolicitudFacturaciones.Count() == 1)
                            {
                                var resultActualizarBltrabajo = _serviceEmbarques.ActualizarBlTrabajado(KeyBL, fileName);
                                if (resultActualizarBltrabajo.Result.ToString() == "1")
                                {
                                    //Registrar Notificacion de Facturacion
                                    NotificacionFacturacionParameter parameterNotificacionFactu = new NotificacionFacturacionParameter();
                                    parameterNotificacionFactu.IdUsuarioRegistra = -1;
                                    parameterNotificacionFactu.KEYBLD = KeyBL;
                                    parameterNotificacionFactu.Estado = "E";
                                    var resultNotificacion = _repositoryFacturacion.NotificarFacturacion(parameterNotificacionFactu);


                                    EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                                    enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                                    enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyNotificacionFacturacion(
                                         resultFacturacion.SolicitudFacturaciones.ElementAt(0).SolicitanteEmpresaPersona,
                                        resultFacturacion.SolicitudFacturaciones.ElementAt(0).NroBl,
                                        Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupoUrl);

                                    enviarMessageCorreoParameterVM.RequestMessage.Correo = resultFacturacion.SolicitudFacturaciones.ElementAt(0).SolicitanteCorreo;
                                    enviarMessageCorreoParameterVM.RequestMessage.Asunto = "Notificación de Facturación";
                                    enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                                    enviarMessageCorreoParameterVM.RequestMessage.Archivos[0] = string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name);

                                    var ressult = _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);


                                }
                                else
                                {

                                    regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                                }
                            }
                        }

                        //Se envia la información al servicio que se ha actualizado y enviado
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error facturacion job: {ex.Message}");
                        regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                    }
                }
            },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }


        private void regresarArchivo(string origen, string destino) {

            File.Move(origen, destino);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        public class FileFacturacion { 
        
        public string FullName { get; set; }
            public string Name { get; set; }

        }

    }
}

