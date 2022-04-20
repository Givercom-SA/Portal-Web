using BusinessLogic.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Servicio.Embarque.Models.GestionarMemo;
using Servicio.Embarque.Models.SolicitudFacturacion;
using Servicio.Embarque.Repositorio;
using Servicio.Embarque.ServiceConsumer;
using Servicio.Embarque.ServiceExterno;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.Message;

namespace Servicio.Embarque.BusinessLogic
{
    public class ProcesoBusinessLogic : IBusinessLogic
    {

        private int number = 0;
        private Timer timer;

        private readonly IMemoRepository _repository;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioEmbarques _serviceEmbarques;
        private IConfiguration _configuration;
        private readonly ISolicitudFacturacionRepository _repositoryFacturacion;
        private readonly ServicioMessage _servicioMessage;
        private readonly ILogger<ProcesoBusinessLogic> _logger;
        private readonly INotificacionArriboRepository _notificacionRepository;
        

        public ProcesoBusinessLogic(IMemoRepository repository,
            ServicioUsuario serviceUsuario,
            ServicioEmbarques serviceEmbarques,
            IConfiguration configuration,
             ServicioMessage servicioMessage,
             INotificacionArriboRepository notificacionRepository,
              ILogger<ProcesoBusinessLogic> logger)
        {
            _repository = repository;
            _serviceUsuario = serviceUsuario;
            _serviceEmbarques = serviceEmbarques;
            _configuration = configuration;
            _servicioMessage = servicioMessage;
            _notificacionRepository = notificacionRepository;
            _logger = logger;
        }
        public async Task MemoJobAsync()
        {
            try
            {
                string pattern = "BM_*";
                string rutaNotificacionesPendiente = _configuration["MemosCarpeta:PendientesPath"];
                string rutaNotificacionesProcesadas = _configuration["MemosCarpeta:ProcesadasPath"];

                var archivos = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles(pattern);
                string KeyBL = string.Empty;
                foreach (var archivo in archivos)
                {
                    string KeyBLFull = Path.GetFileName(archivo.Name).ToUpper().Replace("BM_", "");
                    string[] arrayKeyBl = KeyBLFull.Split('_');
                    KeyBL = arrayKeyBl[0];
                    var parameter = new NotificacionMemoParameter
                    {
                        KeyBLD = KeyBL,
                        IdUsuario = -1,
                        NombreArchivo = KeyBLFull
                    };
                    var result = _repository.CrearNotificacionMemo(parameter);
                    archivo.MoveTo(string.Format("{0}/{1}", rutaNotificacionesProcesadas, archivo.Name));
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MemoJobAsync");
               
            }
        }

        public async Task FacturacionJobAsync()
        {
            Interlocked.Increment(ref number);

            try
            {

                Console.WriteLine($"Facturacion job: {number}");
                string rutaNotificacionesPendiente = _configuration["TrabajadoCarpeta:PendientePath"];
                string rutaNotificacionesProcesadas = _configuration["TrabajadoCarpeta:ProcesadasPath"];
                string rutaNotificacionesProcesando = _configuration["TrabajadoCarpeta:ProcesandoPath"];
                string rutaNotificacionesBackup = _configuration["TrabajadoCarpeta:BackupPath"];

                List<FileFacturacion> listRutas = new List<FileFacturacion>();
                var archivosActuales = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles();
                string strArchivoMover = "";
                string strArchivoBackupMover = "";

                foreach (var item in archivosActuales)
                {
                    try
                    {
                        strArchivoMover = string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name);
                        strArchivoBackupMover = string.Format("{0}/{1}", rutaNotificacionesBackup, item.Name);

                        if (File.Exists(strArchivoMover))
                        {

                            File.Move(strArchivoMover, strArchivoBackupMover);
                            File.Delete(strArchivoMover);
                        }

                        File.Move(item.FullName, strArchivoMover);

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

                        if (resultFacturacion?.SolicitudFacturaciones.Count() > 0)
                        {
                            var resultEmbarqueGtrm = _serviceEmbarques.ObtenerEstadoSolicitudFacturacion(resultFacturacion.SolicitudFacturaciones.ElementAt(0).IdSolicitudTaf);

                            if (resultFacturacion?.IN_CODIGO_RESULTADO == 0)
                            {

                                if (resultFacturacion.SolicitudFacturaciones.Count() == 1 &&
                                resultFacturacion.SolicitudFacturaciones.ElementAt(0).Estado == Utilitario.Constante.EmbarqueConstante.EstadoGeneral.APROBADO.ToString())
                                {
                                    if (resultEmbarqueGtrm?.Result?.FLAG_ESTADO_FACTURACION_SOLICITUD == "1" &&
                                    resultEmbarqueGtrm?.Result?.FLAG_COBROS_PENDIENTES == "0")
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
                                                 resultFacturacion.SolicitudFacturaciones.ElementAt(0).CodigoSolicitud,
                                                resultFacturacion.SolicitudFacturaciones.ElementAt(0).NroBl,
                                               _configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupoUrl.ToString()]);

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
                                    else
                                    {
                                        regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                                    }
                                }
                                else
                                {
                                    regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                                }
                            }
                            else
                            {
                                regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                            }
                        }
                        else
                        {
                            regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error facturacion job: {ex.Message}");
                        regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                    }
                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "FacturacionJobAsync");

            }
        }

        public async Task NotificacionArriboAsync() {
            Interlocked.Increment(ref number);
            Console.WriteLine($"Printing from worker: {number}");
            try
            {
                string pattern = "NA-{0}*";

                var lista = _notificacionRepository.ListaNotificacionesArriboPendientes().ListaNotificacionesPendientes;

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


                                    if (user.Result.Usuario.EntidadTipoDocumento.Equals("RUC"))
                                    {
                                        strCliente = user.Result.Usuario.getNombre();
                                    }
                                    else
                                    {
                                        strCliente = user.Result.Usuario.EntidadRazonSocial;
                                    }

                                    //Los movemos a la otra ruta
                                    File.Move(archivo.FullName, string.Format("{0}/{1}", rutaNotificacionesProcesadas, archivo.Name));

                                    EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                                    enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                                    enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyNotificacionArribo(strCliente, item.NOTARR_NUMERACION_EMBARQUE, item.GTEM_NOMBRES, Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupoUrl);
                                    enviarMessageCorreoParameterVM.RequestMessage.Correo = user.Result.Usuario.Correo;
                                    enviarMessageCorreoParameterVM.RequestMessage.Asunto = string.Format("Notificación de Arribo - Numeración de  Embarque: {0}", item.NOTARR_NUMERACION_EMBARQUE);
                                    enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                                    enviarMessageCorreoParameterVM.RequestMessage.Archivos[0] = string.Format("{0}/{1}", rutaNotificacionesProcesadas, archivo.Name);
                                    var ressult = _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
                                    //Se actualiza el registro indicando que ya se proceso
                                    _notificacionRepository.ActualizarEstadoNotificacion(item.NOTARR_KEYBLD, item.NOTARR_IDUSUARIO_CREA, "NA");


                                }

                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NotificacionArriboAsync");
            }
        }
        private void regresarArchivo(string origen, string destino)
        {

            File.Move(origen, destino);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public class FileFacturacion
        {

            public string FullName { get; set; }
            public string Name { get; set; }

        }
    }
}

