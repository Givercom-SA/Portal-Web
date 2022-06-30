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

        
        private Timer timer;
        private int numberMemo = 0;
        private int numberFactura = 0;
        private int numberNotificacionArribo= 0;
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
              ILogger<ProcesoBusinessLogic> logger,
              ISolicitudFacturacionRepository repositoryFacturacion)
        {
            _repository = repository;
            _serviceUsuario = serviceUsuario;
            _serviceEmbarques = serviceEmbarques;
            _configuration = configuration;
            _servicioMessage = servicioMessage;
            _notificacionRepository = notificacionRepository;
            _repositoryFacturacion = repositoryFacturacion;
            _logger = logger;
        }
        public async Task MemoJobAsync()
        {
          
            Interlocked.Increment(ref numberMemo);
            _logger.LogInformation($"Iniciado Memo JOB: Iteracion: {numberMemo}");
            try
            {
                string pattern = "BM_*";
                string rutaNotificacionesPendiente = _configuration["MemosCarpeta:PendientesPath"];
                string rutaNotificacionesProcesadas = _configuration["MemosCarpeta:ProcesadasPath"];

                var archivos = new DirectoryInfo(rutaNotificacionesPendiente).GetFiles(pattern);
                string KeyBL = string.Empty;
                foreach (var archivo in archivos)
                {
                    _logger.LogInformation($"Iniciado Mover Archivo: Iteracion: {numberMemo}");

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

                    _logger.LogInformation($"Finalizado Mover Archivo JOB: Nombre: {archivo.Name}, Iteracion: {numberMemo}");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Memo Iteracion:{numberMemo}");

            }
            finally {
                _logger.LogInformation($"Finalizado Memo JOB: Iteracion: {numberMemo}");
            }
        }

        public async Task FacturacionJobAsync()
        {
            
            Interlocked.Increment(ref numberFactura);

            try
            {

                _logger.LogInformation($"Iniciado Facturacion JOB: Iteracion: {numberFactura}");

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
                    _logger.LogInformation($"Iniciado Facturacion Mover Archivo: Iteracion: {numberFactura}");

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
                        _logger.LogInformation($"Facturacion Se Mueve Archivo {strArchivoMover}: Iteracion: {numberFactura}");
                        listRutas.Add(new FileFacturacion() { Name = item.Name, FullName = item.FullName });
                    }
                    catch (Exception err)
                    {
                        _logger.LogError(err, $"Facturacion Error Mover Archivos: Iteracion: {numberFactura}");

                    }
                    finally
                    {
                        _logger.LogInformation($"Finalizado Facturacion Mover Archivo: Iteracion: {numberFactura}");
                    }
                }

                _logger.LogInformation($"Iniciado Facturacion Procesar Archivos Iteracion: {numberFactura}");
                foreach (var item in listRutas)
                {
                    try
                    {
                        _logger.LogInformation($"Iniciado Facturacion Procesar Archivos: Iteracion: {numberFactura}");

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
                                            _logger.LogInformation($"Facturacion Error respuesta de Servicio ActualizarBlTrabajado,  Regresar Archivos a la Ruta Inicial {rutaNotificacionesPendiente}/{item.Name}: Iteracion: {numberFactura}");
                                        }
                                    }
                                    else
                                    {
                                        regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                                        _logger.LogInformation($"Facturacion FLAG_COBROS_PENDIENTES<>0 y FLAG_ESTADO_FACTURACION_SOLICITUD<>1 Regresar Archivos a la Ruta Inicial {rutaNotificacionesPendiente}/{item.Name}: Iteracion: {numberFactura}");
                                    }
                                }
                                else
                                {
                                    regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                                    _logger.LogInformation($"Facturacion Estado de Solicitud de Facturacion no esta APROBADO, Regresar Archivos a la Ruta Inicial {rutaNotificacionesPendiente}/{item.Name}: Iteracion: {numberFactura}");
                                }
                            }
                            else
                            {
                                regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                                _logger.LogInformation($"Facturacion Erro en Obtener Solicitud de Facturacion, Regresar Archivos a la Ruta Inicial {rutaNotificacionesPendiente}/{item.Name}: Iteracion: {numberFactura}");
                            }
                        }
                        else
                        {
                            regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                            _logger.LogInformation($"Facturacion Error en Listar Solicitudes de Facturacion, Regresar Archivos a la Ruta Inicial {rutaNotificacionesPendiente}/{item.Name}: Iteracion: {numberFactura}");
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error Facturacion Proceso de Archivo, Iteracion: {numberFactura}, {ex.Message}");
                        regresarArchivo(string.Format("{0}/{1}", rutaNotificacionesProcesando, item.Name), string.Format("{0}/{1}", rutaNotificacionesPendiente, item.Name));
                        _logger.LogInformation($"Facturacion Regresar Archivos a la Ruta Inicial {rutaNotificacionesPendiente}/{item.Name}: Iteracion: {numberFactura}");
                    }
                    finally
                    {
                        _logger.LogInformation($"Finalizado Facturacion Procesar Archivo, Iteracion: {numberFactura}");
                    }
                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Facturacion JOB: Iteracion: {numberFactura},  {ex.Message}");

            }
            finally {
                _logger.LogInformation($"Finalizado Facturacion JOB: Iteracion: {numberFactura}");
            }



        }

        public async Task NotificacionArriboAsync() {

         

            Interlocked.Increment(ref numberNotificacionArribo);
            _logger.LogInformation($"Iniciado Notificacion Arribo : Iteracion: {numberNotificacionArribo}");

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
                        _logger.LogInformation($"Iniciado Notificacion Arribo Procesar archivo: Iteracion: {numberNotificacionArribo}");

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
                        _logger.LogInformation($"Finalizado Notificacion Arribo Procesar archivo: Iteracion: {numberNotificacionArribo}");
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Erro Notificacion Arribo : Iteracion: {numberNotificacionArribo}");
            }
            finally {
                _logger.LogInformation($"Finalizado Notificacion Arribo : Iteracion: {numberNotificacionArribo}");
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

