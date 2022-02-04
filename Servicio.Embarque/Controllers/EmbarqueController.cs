using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.AsignarAgente;
using Servicio.Embarque.Repositorio;
using AutoMapper;
using Servicio.Embarque.Models;
using TransMares.Core;
using System.Net;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using Servicio.Embarque.Models.SolicitudFacturacionTerceros;
using ViewModel.Datos.ListaNotificacionesArribo;
using ViewModel.Datos.Embarque.GestionarMemo;
using Servicio.Embarque.Models.GestionarMemo;
using Servicio.Embarque.Models.CobrosPagar;
using ViewModel.Datos.Embarque.CobrosPagar;
using ViewModel.Datos.ListaExpressRelease;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models.SolicitudFacturacion;
using ViewModel.Datos.Embarque.SolicitudFacturacion;
using static Utilitario.Constante.EmbarqueConstante;
using ViewModel.Datos.Message;
using Servicio.Embarque.ServiceConsumer;

namespace Servicio.Embarque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbarqueController : ControllerBase
    {
        private readonly IAsignarAgenteRepository _repository;
        private readonly INotificacionArriboRepository _repositoryNotificacion;
        private readonly IMemoRepository _repositoryMemo;
        private readonly ICobroPagarRepository _repositoryCobroPagar;
        private readonly ServiceExterno.ServicioEmbarques _servicioEmbarques;
        private readonly IMapper _mapper;
        private readonly ServicioMessage _servicioMessage;
        
        private IConfiguration _configuration;
        public EmbarqueController(IAsignarAgenteRepository repository,
            INotificacionArriboRepository repositoryNotificacion,
            IMemoRepository repositoryMemo,
            ICobroPagarRepository repositoryCobroPagar,
            IMapper mapper,
            IConfiguration configuration,
            ServicioMessage servicioMessage,
            ServiceExterno.ServicioEmbarques servicioEmbarques)
        {
            _repository = repository;
            _repositoryNotificacion = repositoryNotificacion;
            _repositoryMemo = repositoryMemo;
            _repositoryCobroPagar = repositoryCobroPagar;
            _mapper = mapper;
            _configuration = configuration;
            _servicioMessage = servicioMessage;
            _servicioEmbarques =servicioEmbarques;


        }

        [HttpGet]
        [Route("listar-agentes")]
        public ActionResult<ListarUsuarioEntidadResultVM> ObtenerListaUsuarioEntidad(int IdPerfil, int IdUsuarioExcluir)
        {
            var result = _repository.ObtenerUsuariosEntidad(IdPerfil, IdUsuarioExcluir);
            return _mapper.Map<ListarUsuarioEntidadResultVM>(result);
        }

        [HttpPost]
        [Route("listar-asignacion")]
        public ActionResult<ListarAsignarAgenteResultVM> ObtenerListarAsignacion(AsignarAgenteListarParameter parameter)
        {
            var result = _repository.ObtenerListaAsignacion(parameter);
            return _mapper.Map<ListarAsignarAgenteResultVM>(result);
        }

        [HttpPost]
        [Route("listar-asignados")]
        public ActionResult<ListarAsignarAgenteResultVM> ObtenerListarAsignados(AsignarAgenteListarParameter parameter)
        {
            var result = _repository.ObtenerListaAsignados(parameter);
            return _mapper.Map<ListarAsignarAgenteResultVM>(result);
        }

        [HttpPost]
        [Route("asignar-agente-crear")]
        public ActionResult<AsignarAgenteResultVM> AsignarAgenteCrear(AsignarAgenteCrearParameter parameter)
        {
            var result = _repository.AsignarAgenteCrear(parameter);
            if (result.IN_CODIGO_RESULTADO > 0) // Creado
            {
               Notificar(result.IN_CODIGO_RESULTADO, parameter.LogoEmpresa);
                result.IN_CODIGO_RESULTADO = 0;
            }
            return _mapper.Map<AsignarAgenteResultVM>(result);
        }

        [HttpPost]
        [Route("asignar-agente-cambiar-estado")]
        public ActionResult<AsignarAgenteResultVM> AsignarAgenteCambiarEstado(AsignarAgenteEstadoParameter parameter)
        {
            var result = _repository.AsignarAgenteCambiarEstado(parameter);
            if (result.IN_CODIGO_RESULTADO == 0)
            {
                Notificar(parameter.Id, parameter.LogoEmpresa);
            }
            return _mapper.Map<AsignarAgenteResultVM>(result);
        }

        [HttpPost]
        [Route("registrar-notificacion-arribo")]
        public ActionResult<string> RegistrarNotificacionArribo(RegistrarNotificacionArriboParameter parameter)
        {
            return _repositoryNotificacion.RegistrarNotificacionArribo(parameter);
        }

        [HttpGet]
        [Route("listar-notificacion-arribo-pendientes")]
        public ActionResult<ListarNotificacionesPendientesVW> ListaNotificacionesArriboPendientes(int IdPerfil, int IdUsuarioExcluir)
        {
            var result = _repositoryNotificacion.ListaNotificacionesArriboPendientes();
            return _mapper.Map<ListarNotificacionesPendientesVW>(result);
        }

        [HttpPost]
        [Route("registrar-facturacion-tercero")]
        public ActionResult<RegistrarFacturacionTerceroResultVM> RegistrarFacturacionTercero(RegistrarFacturacionTerceroParameterVM parameter)
        {
            var result = _repository.RegistrarFacturacionTercero(_mapper.Map<RegistrarFacturacionTerceroParameter>(parameter));

            return _mapper.Map<RegistrarFacturacionTerceroResultVM>(result);
        }

        [HttpPost]
        [Route("listar-facturacion-terceros")]
        public ActionResult<ListarFacturacionTerceroResultVM> ObtenerFacturacionTerceros(ListarFacturacionTerceroParameterVM parameter)
        {
            var result = _repository.ObtenerFacutracionTerceros(_mapper.Map<ListarFacturacionTerceroParameter>(parameter));
            return _mapper.Map<ListarFacturacionTerceroResultVM>(result);
        }

        [HttpGet]
        [Route("listar-facturacion-terceros-detalle")]
        public ActionResult<ListarFacturacionTerceroDetalleResultVM> ObtenerFacturacionTerceroDetalle(int IdFacturacionTercero)
        {
            var result = _repository.ObtenerFacutracionTerceroDetalle(IdFacturacionTercero);
            return _mapper.Map<ListarFacturacionTerceroDetalleResultVM>(result);
        }

        [HttpGet]
        [Route("listar-facturacion-terceros-detalle-keybl")]
        public ActionResult<ListarFacturacionTerceroDetallePorKeyblResultVM> ObtenerFacutracionTerceroDetallePorKeybl(string keybl)
        {
            var result = _repository.ObtenerFacutracionTerceroDetallePorKeybl(keybl);
            return _mapper.Map<ListarFacturacionTerceroDetallePorKeyblResultVM>(result);
        }


        [HttpGet]
        [Route("listar-cobros-pagar-get_padre-keybl")]
        public ActionResult<ListarCobrosPagarPadreBeyBlResultVM> ObtenerCobroPagarPadreKeyBl(string keybl)
        {
            var result = _repositoryCobroPagar.ObtenerEmbarquePadrePorKeyBl(keybl);
            return _mapper.Map<ListarCobrosPagarPadreBeyBlResultVM>(result);
        }


        [HttpPost]
        [Route("actualizar-facturacion-tercero")]
        public ActionResult<RegistrarFacturacionTerceroResultVM> ActualizarFacturacionTercero(RegistrarFacturacionTerceroParameterVM parameter)
        {
            var result = _repository.ActualizarFacturacionTercero(_mapper.Map<RegistrarFacturacionTerceroParameter>(parameter));
            if (result.IN_CODIGO_RESULTADO == 0)
            {
                switch (parameter.Estado)
                {
                    case "SA":
                        enviarCorreo(parameter.Correo,
                                $"Solicitud de Facturación a Tercero {result.CodigoFacturaTercero} Aprobado",
                                new FormatoCorreoBody().formatoBodySolicitud( result.RazonSocialNombres,
                                $" La solicitud <b>{result.CodigoFacturaTercero}</b> ha sido aprobada para el embarque <b>{parameter.EmbarqueNroBL}</b>.",parameter.LogoEmpresa));
                        break;
                    case "SR":
                        enviarCorreo(parameter.Correo,
                                $"Solicitud de Facturación a Tercero {result.CodigoFacturaTercero} Rechazado",
                                new FormatoCorreoBody().formatoBodySolicitud(result.RazonSocialNombres,
                                $" La solicitud {result.CodigoFacturaTercero} ha sido rechazado para el embarque {parameter.EmbarqueNroBL}.", parameter.LogoEmpresa));
                        break;
                }
            }
            return _mapper.Map<RegistrarFacturacionTerceroResultVM>(result);
        }

        [HttpPost]
        [Route("crear-notificacion-memo")]
        public ActionResult<NotificacionesMemoResultVM> CrearNotificacionMemo(NotificacionMemoParameterVM parameter)
        {
            var result = _repositoryMemo.CrearNotificacionMemo(_mapper.Map<NotificacionMemoParameter>(parameter));
            return _mapper.Map<NotificacionesMemoResultVM>(result);
        }

        [HttpPost]
        [Route("verificar-notificacion-memo")]
        public ActionResult<NotificacionesMemoResultVM> VerificarNotificacionMemo(NotificacionMemoParameterVM parameter)
        {
            var result = _repositoryMemo.VerificarNotificacionMemo(_mapper.Map<NotificacionMemoParameter>(parameter));
            return _mapper.Map<NotificacionesMemoResultVM>(result);
        }

        [HttpPost]
        [Route("crear-solicitud-memo")]
        public ActionResult<ProcesarSolicitudMemoResultVM> CrearSolicitudMemo([FromBody] SolicitudMemoParameterVM parameter)
        {
            var result = _repositoryMemo.CrearSolicitudMemo(_mapper.Map<SolicitudMemoParameter>(parameter));

            if (result.IN_CODIGO_RESULTADO == 0)
            {
             
            }
            return _mapper.Map<ProcesarSolicitudMemoResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-solicitudes-memo")]
        public ActionResult<ListarSolicitudesMemoResultVM> ObtenerSolicitudes(ListarSolicitudesMemoParameterVM parameter)
        {
            var result = _repositoryMemo.ObtenerSolicitudesMemo(_mapper.Map<ListarSolicitudesMemoParameter>(parameter));
            return _mapper.Map<ListarSolicitudesMemoResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-solicitud-memo")]
        public ActionResult<SolicitudMemoResultVM> ObtenerSolicitudMemo(string codSol)
        {
            var result = _repositoryMemo.ObtenerSolicitudMemoPorCodigo(codSol);

            var listaDoc = _repositoryMemo.ObtenerDocumentosSolicitudMemo(result.Codigo);
            var listaEve = _repositoryMemo.ObtenerEventosSolicitudMemo(result.Codigo);

            if (listaDoc.IN_CODIGO_RESULTADO == 0)
                result.ListaDocumentos = listaDoc.ListaDocumentos;

            if (listaEve.IN_CODIGO_RESULTADO == 0)
                result.ListaEventos = listaEve.ListaEventos;

            return _mapper.Map<SolicitudMemoResultVM>(result);
        }

        [HttpPost]
        [Route("actualizar-estado-documento-memo")]
        public ActionResult<SolicitudMemoDocumentoEstadoResultVM> ActualizarEstadoDocumentoMemo(SolicitudMemoDocumentoEstadoParameterVM parameter)
        {
            var result = _repositoryMemo.ActualizarSolicitudMemo(_mapper.Map<DocumentoEstadoMemoParameter>(parameter) );

            return _mapper.Map<SolicitudMemoDocumentoEstadoResultVM>(result);

           
        }

        [HttpPost]
        [Route("procesar-solicitud-memo")]
        public async Task<ActionResult<SolicitudMemoEstadoresultVM>> ProcesarSolicitudMemo(SolicitudMemoEstadoParameterVM parameter)
        {
            SolicitudMemoEstadoresultVM solicitudMemoEstadoresultVM = new SolicitudMemoEstadoresultVM();

            solicitudMemoEstadoresultVM.CodigoResultado = 50;
            solicitudMemoEstadoresultVM.MensajeResultado = "";

            try
            {
             var resultProcesaSolicitudMemo=   _repositoryMemo.ProcesarSolicitudMemo(parameter.CodigoSolicitud,parameter.IdUsuarioModifica,parameter.CodigoEstadoEvalua, parameter.CodigoMotivoRechazo);

                solicitudMemoEstadoresultVM.CodigoResultado = resultProcesaSolicitudMemo.IN_CODIGO_RESULTADO;
                solicitudMemoEstadoresultVM.MensajeResultado = resultProcesaSolicitudMemo.STR_MENSAJE_BD;

                var solicitud = _repositoryMemo.ObtenerSolicitudMemoPorCodigo(parameter.CodigoSolicitud);

                if (!solicitud.EstadoCodigo.Trim().Equals("SP"))
                {
                    if (solicitud.EstadoCodigo.Trim().Equals("SA"))
                    {
                        // ENVIAR CORREO APROBADO
                        enviarCorreo(solicitud.Correo,
                                "Transmares Group - Solicitud de Devolución de Cobro de Garantía Aprobada",
                                new FormatoCorreoBody().formatoBodySolicitudAprobada(solicitud.Codigo, parameter.ImagenEmpresaLogo));
                    }
                    else if (solicitud.EstadoCodigo.Trim().Equals("SR"))
                    {
                        //var listaDoc = _repositoryMemo.ObtenerDocumentosSolicitudMemo(solicitud.Codigo);
                    

                        // ENVIAR CORREO RECHAZO
                        enviarCorreo(solicitud.Correo,
                                "Transmares Group - Solicitud de Devolución de Cobro de Garantía Rechazada",
                                new FormatoCorreoBody().formatoBodySolicitudMemoRechazada(solicitud.Codigo, parameter.ImagenEmpresaLogo, solicitud.Motivorechazo));
                    }
                }

            }
            catch (Exception err) {

                solicitudMemoEstadoresultVM.CodigoResultado = -100;
                solicitudMemoEstadoresultVM.MensajeResultado = err.Message;


            }

            return solicitudMemoEstadoresultVM;
        }

        [HttpGet]
        [Route("asignar-agente-historial/{idAsginacionAgente}")]
        public ActionResult<AsignarAgenteHistorialResultVM> AsginarAgenteAduanaHistorial(int idAsginacionAgente)
        {
            var result = _repository.AsignarAgenteHistorial(idAsginacionAgente);
            return _mapper.Map<AsignarAgenteHistorialResultVM>(result);
            
        }

        [HttpGet]
        [Route("obtener-eventos-solicitud-memo")]
        public ActionResult<ListarEventosMemoResultVM> ObtenerEventosSolicitudMemo(string codSolicitud)
        {
            var result = _repositoryMemo.ObtenerEventosSolicitudMemo(codSolicitud);
            return _mapper.Map<ListarEventosMemoResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-cobros-pagar")]
        public ActionResult<ListarCobrosPagarResultVM> ObtenerCobrosPagar(string KeyBLD, string BL, string BLNieto, string ConceptoCodigo)
        {
            var result = _repositoryCobroPagar.ObtenerCobrosPagar(KeyBLD, BL, BLNieto, ConceptoCodigo);
            return _mapper.Map<ListarCobrosPagarResultVM>(result);
        }


        [HttpPost]
        [Route("verificar-asignacion-agente-aduanas")]
        public ActionResult<VerificarAsignacionAgenteAduanasResultVM> VerificarAsignacionAgenteAduanas(VerificarAsignacionAgenteAduanasParameterVM parameter)
        {
            var result = _repository.VerificarAsignarAgenteAduanas(_mapper.Map<VerificarAsignacionAgenteAduanasParameter>(parameter));
            return _mapper.Map<VerificarAsignacionAgenteAduanasResultVM>(result);
        }


  

        [HttpPost]
        [Route("crear-cobros-pagar")]
        public ActionResult<CobrosPagarResult> CrearCobrosPagar(CobrosPagarParameterVM parameter)
        {
            var result = _repositoryCobroPagar.CrearCobrosPagar(_mapper.Map<CobrosPagarParameter>(parameter));
            return _mapper.Map<CobrosPagarResult>(result);
        }

        [HttpPost]
        [Route("actualizar-cobros-pagar")]
        public ActionResult<CobrosPagarResult> ActualizarCobrosPagar(CobrosPagarParameterVM parameter)
        {
            var result = _repositoryCobroPagar.ActualizarCobrosPagar(_mapper.Map<CobrosPagarParameter>(parameter));
            return _mapper.Map<CobrosPagarResult>(result);
        }


        [HttpGet]
        [Route("listar-express-release-aceptadas")]
        public ActionResult<ListaExpressReleaseAceptadasVW> ListaExpressReleaseAceptadas()
        {
            var result = _repositoryNotificacion.ListaExpressReleaseAceptadas();
            return _mapper.Map<ListaExpressReleaseAceptadasVW>(result);
        }

        [HttpPost]
        [Route("registrar-express-release-aceptadas")]
        public ActionResult<string> RegistrarExpressReleaseAceptadas(ExpressReleaseParameterVM parameter)
        {
            var result = _repositoryNotificacion.RegistrarExpressReleaseAceptadas(parameter.KeyBl, parameter.NroBl, parameter.Idusuario);
            return result;
        }

        [HttpPost]
        [Route("enviar-correo-memo")]
        public async Task<ActionResult<string>> EnviarCorreoMemo(MemoCorreoParameterVM parameter)
        {


            int CorreoEnviado = 0;
            try
            {
                
                string rutaNotificacionesProcesadas = _configuration["MemosCarpeta:ProcesadasPath"];

                var archivo = new System.IO.DirectoryInfo(rutaNotificacionesProcesadas).GetFiles(parameter.NombreArchivo).FirstOrDefault();

                if (archivo != null)
                {
                    string soloNombreSinExtension = "";
                    string[] listNombreArchivo = parameter.NombreArchivo.Split(".");

                    if (listNombreArchivo.Length > 0)
                    {
                         soloNombreSinExtension = listNombreArchivo[0];
                    }

                    var resultMemoEnviado =  await _servicioEmbarques.ActualizatMemoEnviadoEmbarque(parameter.KeyBLD, soloNombreSinExtension);

                    if (resultMemoEnviado == 1)
                    {
                        EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                        enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                        enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyNotificarMemo($"Se adjunta documento por el Memo del BL {parameter.NroBL}.", parameter.LogoEmpresa);
                        enviarMessageCorreoParameterVM.RequestMessage.Correo = parameter.Correo;
                        enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"Transmares Group – Documento por el Memo";
                        enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                        enviarMessageCorreoParameterVM.RequestMessage.Archivos[0] = archivo.FullName;
                        await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
                        CorreoEnviado = 1;
                    }
                }
            }
            catch (Exception)
            {
                CorreoEnviado = -1;
            }

            return CorreoEnviado.ToString();
        }

        [HttpPost]
        [Route("solicitud-facturacion-provision-fact-tercero")]
        public ActionResult<ListarProvisionFacturacionTerceroResultVM> ObtenerProvicionFacturacionTercero(ListarProvisionFacturacionTerceroParameterVM parameter)
        {
            var result = _repositoryCobroPagar.ObtenerProvisionEmbarqueFacturacionTercerto(_mapper.Map<ListarProvisionFacturacionTerceroParameter>(parameter));
            return _mapper.Map<ListarProvisionFacturacionTerceroResultVM>(result);
            
        }

       
        public async Task<EnviarMessageCorreoResultVM> Notificar(int Id,string logoEmpresa)
        {
            EnviarMessageCorreoResultVM resultenvioMensaje = new EnviarMessageCorreoResultVM();
            var result = _repository.AsignarAgenteDetalle(Id);
            string asunto = string.Empty;
            string correoPara = string.Empty;
            string contenido = string.Empty;
            string nombreRazonSocialCliente = string.Empty;

            if (result != null)
            {
                switch (Convert.ToInt32(result.Estado))
                {
                    

                    case EmbarqueEstadoAgenteAduanas.PENDIENTE:
                        nombreRazonSocialCliente = result.NombreUsuarioAsigna;

                        asunto = string.Format("Número de Embarque {0} - Asignación de Agente de Aduanas ", result.NumeroEmbarque);
                        contenido = string.Format("Estimado {0}, <br/><br/>El consignatorio {1} le asigno como agente de aduanas el embarque {2}. Favor de evaluar dicha solicitud por medio de nuestro portal web.", result.NombreUsuarioAsignado, result.NombreUsuarioAsigna, result.NumeroEmbarque);
                        correoPara = result.CorreoUsuarioAsignado;
                        
                        break;

                    case EmbarqueEstadoAgenteAduanas.APROBADO:
                        nombreRazonSocialCliente = result.NombreUsuarioAsigna;
                        asunto = string.Format("Número de Embarque {0} - Asignación de Agente de Aduanas Aprobado", result.NumeroEmbarque);
                        contenido = string.Format("Estimado {0}, <br/><br/>El agente de aduanas {1} aprobó la asignación del número de embarque {2}", result.NombreUsuarioAsigna, result.NombreUsuarioAsignado, result.NumeroEmbarque);
                        correoPara = result.CorreoUsuarioAsigna;
                        
                        break;

                    case EmbarqueEstadoAgenteAduanas.RECHAZADO:
                        nombreRazonSocialCliente = result.NombreUsuarioAsigna;
                        asunto = string.Format("Número de Embarque {0} - Asignación de Agente de Aduanas Rechazado", result.NumeroEmbarque);
                        contenido = string.Format("Estimado {0}, <br/><br/>El agente de aduanas {1} anuló la asignación de agente de aduanas para el embarque {2}. <br/><br/> Motivo: {3}", result.NombreUsuarioAsigna, result.NombreUsuarioAsignado, result.NumeroEmbarque, result.Observacion);
                        correoPara = result.CorreoUsuarioAsigna;
                        
                        break;

                    case EmbarqueEstadoAgenteAduanas.ANULADO:
                        nombreRazonSocialCliente = result.NombreUsuarioAsignado;
                        asunto = string.Format("Número de Embarque {0} - Asignación de Agente de Aduanas Anulado", result.NumeroEmbarque);
                        contenido = string.Format("Estimado {0}, <br/><br/> El consignatario {1} anulo la asignación del número de embarque {2}. <br/><br/> Motivo: {3}", result.NombreUsuarioAsignado, result.NombreUsuarioAsigna,result.NumeroEmbarque, result.Observacion);
                        correoPara = result.CorreoUsuarioAsignado;
                        
                        break;
                }

                asunto = string.Format("Transmares Group - {0}", asunto);

                 enviarCorreo(correoPara, asunto, new FormatoCorreoBody().formatoBodyNotificarEmbarque(nombreRazonSocialCliente, contenido, logoEmpresa));

            }

            return resultenvioMensaje;
        }

        private  async void enviarCorreo(string _correo, string _asunto, string _contenido)
        {
           
         
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
            await  _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);

        }
    }
}
