using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servicio.Embarque.Repositorio;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using AutoMapper;
using System.Net;
using TransMares.Core;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using Servicio.Embarque.ServiceExterno;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using Utilitario.Constante;
using ViewModel.Datos.Message;
using Servicio.Embarque.ServiceConsumer;

namespace Servicio.Embarque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionamientoController : ControllerBase
    {
        private readonly IDireccionamientoRepository _repository;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioMessage _servicioMessage;
        
        private readonly IMapper _mapper;

        private static ILogger _logger = ApplicationLogging.CreateLogger("DireccionamientoController");


        public DireccionamientoController(IDireccionamientoRepository repository,
                                          ServicioEmbarques serviceEmbarques,
                                          IMapper mapper,
                                          ServicioMessage servicioMessage)
        {
            _repository = repository;
            _serviceEmbarques = serviceEmbarques;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
        }
        [HttpPost]
        [Route("solicitud-direccionamiento-crear")]
        public async Task<ActionResult<SolicitudDireccionamientoResultVM>> SolicitudDireccionamientoCrear([FromBody] SolicitudDireccionamientoParameter parameter)
        {
            var result = _repository.RegistrarSolicitudDireccionamiento(parameter);

            var resultEmbarque = await _serviceEmbarques.ObtenerEmbarque(parameter.KeyBL);

            if (result.IN_CODIGO_RESULTADO == 0)
            {
                // Llamar Servicio Externo
                var external_result = await _serviceEmbarques.RegistrarSolicitudDireccionamiento(result.IN_IDSOLICITUD.ToString(),
                                                                           result.VH_CODSOLICITUD,
                                                                           parameter.IdEntidadSeleccionado,
                                                                           parameter.KeyBL,
                                                                           parameter.CodModalidad,
                                                                           parameter.CodigoTaf,
                                                                           "SP",
                                                                           parameter.Correo);

       
                string mensaje =string.Format("Considerar que se ha registrado la solicitud de direccionamiento Nro. {0} para que pueda proceder a evaluarla.", result.VH_CODSOLICITUD);
                string mensajeCliente = string.Format("Confirmamos la recepción de su solicitud de direccionamiento Nro. {0}, considerar que tiene hasta el {1} para poder culminar con el proceso.", result.VH_CODSOLICITUD,parameter.VencimientoPlazo);

        
                string strAlmacenReceptorCarga = "";

                if (parameter.CodModalidad == EmbarqueConstante.DireccionamientoModalidad.SADA_DESCARGA_DIRECTA.ToString())
                {
                    strAlmacenReceptorCarga = "<strong>Receptor de Carga: </strong>" + parameter.RazonSocial;
                }
                else {
                    strAlmacenReceptorCarga = "<strong>Almacén Direccionado: </strong>" + parameter.RazonSocial;
                }

                //Enviar Correo
              await  enviarCorreo(parameter.Correo,
                            "Transmares Group - Solicitud de Direccionamiento",
                            new FormatoCorreoBody().formatoBodySolicitudDireccionamientoCreada(mensajeCliente,
                                                                                               parameter.NroBL,
                                                                                               parameter.NaveViaje,
                                                                                               parameter.Consignatario,
                                                                                               parameter.CantidadCtn,
                                                                                               strAlmacenReceptorCarga ,
                                                                                                parameter.ImagenEmpresaLogo));
              await  enviarCorreo(resultEmbarque.OPERADOR_MAIL,
                             "Transmares Group - Solicitud de Direccionamiento",
                             new FormatoCorreoBody().formatoBodySolicitudDirrecionamientoCreadaOperador(result.VH_CODSOLICITUD, mensaje, parameter.NroBL,
                                                                                               parameter.NaveViaje,
                                                                                               parameter.Consignatario,
                                                                                               parameter.CantidadCtn,
                                                                                               strAlmacenReceptorCarga, parameter.ImagenEmpresaLogo));

            }
            return _mapper.Map<SolicitudDireccionamientoResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-solicitudes/{nroSolicitud}/{RucDni}/{codEstado}")]
        public ActionResult<ListarSolicitudesResultVM> ObtenerSolicitudes(string nroSolicitud, string RucDni, string codEstado)
        {
            var result = _repository.ObtenerSolicitudes(nroSolicitud, RucDni, codEstado);
            return _mapper.Map<ListarSolicitudesResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-solicitud-porcodigo")]
        public ActionResult<SolicitudResultVM> ObtenerSolicitudPorCodigo(string codSol)
        {
            var result = _repository.ObtenerSolicitudPorCodigo(codSol);

            var listaDoc = _repository.ObtenerDocumentosPorSolicitud(result.Codigo);
            var listaEve = _repository.ObtenerEventosPorSolicitud(result.Codigo);

            if (listaDoc.IN_CODIGO_RESULTADO == 0)
                result.ListaDocumentos = listaDoc.ListaDocumentos;

            if (listaEve.IN_CODIGO_RESULTADO == 0)
                result.ListaEventos = listaEve.ListaEventos;

            return _mapper.Map<SolicitudResultVM>(result);
        }

        [HttpPut]
        [Route("actualizar-estado-documento/{codSolicitud}/{codDocumento}/{CodEstado}/{CodEstadoRechazo}/{userId}")]
        public ActionResult<SolicitudDireccionamientoResultVM> ActualizarEstadoDocumento(string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo, int userId)
        {
            var result = _repository.ActualizarSolicitudPorCodigo(codSolicitud, codDocumento, CodEstado, CodEstadoRechazo, userId);

            return _mapper.Map<SolicitudDireccionamientoResultVM>(result); ;
        }

        [HttpPost]
        [Route("procesar-solicitud")]
        public async Task<ActionResult<EvaluarDireccionamientoVM>> ProcesarSolicitud(ProcesarDireccionamientoParameterVM parameter)
        {

            EvaluarDireccionamientoVM evaluarDireccionamientoVM = new EvaluarDireccionamientoVM();
            evaluarDireccionamientoVM.CodigoResultado = 0;
            evaluarDireccionamientoVM.MensajeResultado = "Registrado con éxito";

            _repository.ProcesarSolicitud(parameter.codSolicitud, parameter.CodigoEstado, parameter.CodigoMotivoRechazo, parameter.idUsuario);

            var solicitud = _repository.ObtenerSolicitudPorCodigo(parameter.codSolicitud);
            
            if (!solicitud.EstadoCodigo.Trim().Equals("SP"))
            {
                try
                {

                    var external_result = await _serviceEmbarques.ActualizarEstadoSolicitudDireccionamiento(solicitud.Id.ToString(), parameter.CorreoUsuarioOperador, solicitud.EstadoCodigo.Trim(), parameter.CodigoMotivoRechazo);


                    if (external_result==1)
                    {



                        if (solicitud.EstadoCodigo.Trim().Equals("SA"))
                        {

                            // ENVIAR CORREO APROBADO
                          await  enviarCorreo(solicitud.Correo,
                                    "Transmares Group - Solicitud de Direccionamiento Aprobada",
                                    new FormatoCorreoBody().formatoBodySolicitudDireccionamientoAprobada(solicitud.Codigo, parameter.nombreImagenEmpresa));

                        }
                        else if (solicitud.EstadoCodigo.Trim().Equals("SR"))
                        {


                            // ENVIAR CORREO RECHAZO
                            await enviarCorreo(solicitud.Correo,
                                    "Transmares Group - Solicitud de Direccionamiento Rechazada",
                                    new FormatoCorreoBody().formatoBodySolicitudDireccionamientoRechazada(solicitud.Codigo, solicitud.MotivoRechazo, parameter.nombreImagenEmpresa));
                        }
                        // Llamar Servicio Externo

                    }
                    else {

                        evaluarDireccionamientoVM.CodigoResultado = -1;
                        evaluarDireccionamientoVM.MensajeResultado = "Ocurrio un error inesperado .";
                    }



                }
                catch (Exception err)
                {

                   // _logger.LogError(err, "DireccionamientoApi-ProcesarSolicitud");
                    evaluarDireccionamientoVM.CodigoResultado = -1;
                    evaluarDireccionamientoVM.MensajeResultado = "Ocurrio un error inesperado, por favor volver a intentar.";
                }



            }
            return evaluarDireccionamientoVM;


        }

        [HttpGet]
        [Route("obtener-eventos-solicitud")]
        public ActionResult<ListarEventosResultVM> ObtenerEventosPorSolicitud(string codSolicitud)
        {
            var result = _repository.ObtenerEventosPorSolicitud(codSolicitud);
            return _mapper.Map<ListarEventosResultVM>(result);
        }

        [HttpGet]
        [Route("validar-solicitud-direccionamiento/{KeyBL}")]
        public ActionResult<SolicitudDireccionamientoResultVM> ValidarSolicitudDireccionamiento(string KeyBL)
        {
            var result = _repository.ValidarSolicitudDireccionamiento(KeyBL);
            return _mapper.Map<SolicitudDireccionamientoResultVM>(result);
        }

        private async  Task<string> enviarCorreo(string _correo, string _asunto, string _contenido)
        {
     

            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
           await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
            return "";
        }
    }
}
