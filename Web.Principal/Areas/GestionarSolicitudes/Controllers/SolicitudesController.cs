using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Solicitud;
using ViewModel.Datos.SolictudAcceso;
using Web.Principal.Areas.GestionarSolicitudes.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Areas.GestionarSolicitudes.Controllers
{
    [Area("GestionarSolicitudes")]
    public class SolicitudesController : BaseController
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioSolicitud _serviceSolicitud;
        private readonly IConfiguration _configuration;
        private static ILogger _logger = ApplicationLogging.CreateLogger("SolicitudesController");
        private readonly IMapper _mapper;

        public SolicitudesController(
            ServicioMaestro serviceMaestro,
            ServicioSolicitud serviceSolicitud,
            IMapper mapper, IConfiguration configuration)
        {
            _serviceMaestro = serviceMaestro;
            _serviceSolicitud = serviceSolicitud;
            _mapper = mapper;
            _configuration = configuration;
         


        }

        [HttpGet]
        public async Task<IActionResult> ListarSolicitudes(ListarSolicitudesParameterVM model)
        {
            var viewModel = (model == null) ? new ListarSolicitudesParameterVM() : model;
            try
            {
                var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(34);
                ViewBag.ListarEstado = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

                var listaSolicitud = await _serviceSolicitud.ObtenerSolicitudes(model);

                if (listaSolicitud.CodigoResultado == 0)
                    viewModel.listaResultado = listaSolicitud.ListaSolicitudes;
            }
            catch (Exception err) {
                _logger.LogError(err, "ListarSolicitudes");
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> VerSolicitud(string nroSolicitud)
        {
            
            AsignacionEvaluacionModel asignacionEvaluacionModel = new AsignacionEvaluacionModel();
            try { 
            asignacionEvaluacionModel.SolicitudVM  = await _serviceSolicitud.obtenerSolicitudPorCodigo(nroSolicitud);
            asignacionEvaluacionModel.CodigoSolicitud = nroSolicitud;


                /*// Obtenermos los motivos de rechazo
                var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(28);
                ViewBag.ListarMotivosRechazos = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
                ViewBag.listaDocumentoPermitidos = usuario.DocumentosRevisar;*/
            }
            catch (Exception err)
            {
                _logger.LogError(err, "VerSolicitud");
            }

            return View(asignacionEvaluacionModel);
        }

        public async Task<JsonResult> ActualizarDocumentoPorSolicitud(
            string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo)
        {

            var mensajeResult = "";
            try { 
                mensajeResult=await _serviceSolicitud.ActualizarEstadoDocumento(codSolicitud, codDocumento, CodEstado, CodEstadoRechazo, usuario.idUsuario);

            }
            catch (Exception err)
            {
                _logger.LogError(err, "ActualizarDocumentoPorSolicitud");
            }
            return Json(mensajeResult);
        }

        public async Task<JsonResult> ProcesarSolicitud(string codSolicitud)
        {
            var mensajeResult = "";


            try {
                mensajeResult = await _serviceSolicitud.ProcesarSolicitud(codSolicitud);
            }
            catch (Exception err)
            {
                _logger.LogError(err, "ProcesarSolicitud");
            }
            return Json(mensajeResult);
        }

        public async Task<JsonResult> AprobarSolicitud(string codSolicitud)
        {

            ActionResponse ActionResponse = new ActionResponse();
            try { 

            SolicitudAccesoAprobarParameterVM solicitudAccesoAprobarParameterVM = new SolicitudAccesoAprobarParameterVM();

            solicitudAccesoAprobarParameterVM.CodigoSolicitud = codSolicitud;
            solicitudAccesoAprobarParameterVM.EstadoSolicitud =Utilitario.Constante.EmbarqueConstante.EstadoGeneral.APROBADO;
            solicitudAccesoAprobarParameterVM.IdUsuarioEvalua = usuario.idUsuario;
            solicitudAccesoAprobarParameterVM.ImagenGrupTransmares = $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}" ;
            solicitudAccesoAprobarParameterVM.UrlTransmares = $"{this.GetUriHost()}";
            var mensajeResult = await _serviceSolicitud.AprobarSolicitud(solicitudAccesoAprobarParameterVM);

                if (mensajeResult.CodigoResultado >= 0)
                {
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = mensajeResult.MensajeResultado;
                }
                else if (mensajeResult.CodigoResultado < 0) {
                    _logger.LogError(mensajeResult.MensajeResultado, "Erro en API _serviceSolicitud.AprobarSolicitud");
                    ActionResponse.Codigo = -300;
                    ActionResponse.Mensaje = "Ocurrio un error inesperado, por favor intentar nuevamente.";
                }
                else
                {
                    ActionResponse.Codigo = -300;
                    ActionResponse.Mensaje = "Ocurrio un error inesperado, por favor intentar nuevamente.";
                }
            }
            catch (Exception err)
            {
                _logger.LogError(err, "AprobarSolicitud");
            }
            return Json(ActionResponse);
        }

        public async Task<JsonResult> RechazarSolicitud(AsignacionEvaluacionModel model)
        {
            ActionResponse actionResponse = new ActionResponse();
            actionResponse.ListActionListResponse = new List<ActionErrorResponse>();


            try { 

            if (ModelState.IsValid)
            {


                SolicitudAccesoAprobarParameterVM solicitudAccesoAprobarParameterVM = new SolicitudAccesoAprobarParameterVM();

                solicitudAccesoAprobarParameterVM.CodigoSolicitud = model.CodigoSolicitud;
                solicitudAccesoAprobarParameterVM.EstadoSolicitud = Utilitario.Constante.EmbarqueConstante.EstadoGeneral.RECHAZADO;
                solicitudAccesoAprobarParameterVM.IdUsuarioEvalua = usuario.idUsuario;
                solicitudAccesoAprobarParameterVM.MotivoRechazo = model.MotivoRechazo;

                var mensajeResult = await _serviceSolicitud.RechazarSolicitud(solicitudAccesoAprobarParameterVM);

                if (mensajeResult.CodigoResultado >= 0)
                {
                    actionResponse.Codigo = 0;
                    actionResponse.Mensaje = mensajeResult.MensajeResultado;
                }
                else
                {
                    actionResponse.Codigo = -300;
                    actionResponse.Mensaje = "Ocurrio un error inesperado, por favor intentar nuevamente.";
                }
            }
            else
            {

                actionResponse.Codigo = 100;

                var erroneousFields = ModelState.Where(ms => ms.Value.Errors.Any())
                                   .Select(x => new { x.Key, x.Value.Errors });

                foreach (var erroneousField in erroneousFields)
                {
                    var fieldKey = erroneousField.Key;
                    var fieldErrors = string.Join(" | ", erroneousField.Errors.Select(e => e.ErrorMessage));

                    actionResponse.ListActionListResponse.Add(new ActionErrorResponse()
                    {
                        Mensaje = fieldErrors,
                        NombreCampo = fieldKey
                    }); ;
                }


                actionResponse.Mensaje = "Por favor ingresar los campos requeridos.";

            }
            }
            catch (Exception err)
            {
                _logger.LogError(err, "RechazarSolicitud");
            }
            return Json(actionResponse);

        }
    }
}
