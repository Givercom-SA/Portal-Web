using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Security.Common;
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
using Web.Principal.Util;

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
        public async Task<IActionResult> ListarSolicitudes(string parkey)
        {
            ListarSolicitudesParameterVM model = new ListarSolicitudesParameterVM();
            ListarSolicitudesParameterVM viewModel = new ListarSolicitudesParameterVM();

        

          



            try
            {
                if (parkey != null)
                {
                    var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                    string[] parametros = dataDesencriptada.Split('|');

                    if (parametros.Count() > 1)
                    {
                        string FechaIngreso = parametros[0];
                        string CampoCodSolicitud = parametros[1];
                        string CampoRuc = parametros[2];
                        string CampoRazonSocial = parametros[3];
                        string NombreContacto = parametros[4];
                        string CodEstado = parametros[5];


                        // FechaIngreso=&CampoCodSolicitud=&CampoRuc=&CampoRazonSocial=&NombreContacto=&CodEstado=0
                        model.FechaIngreso = FechaIngreso == "" ? null : Convert.ToDateTime(FechaIngreso);
                        model.CampoCodSolicitud = CampoCodSolicitud;
                        model.CampoRuc = CampoRuc;
                        model.CampoRazonSocial = CampoRazonSocial;
                        model.NombreContacto = NombreContacto;
                        model.CodEstado = CodEstado;



                    }
                }


                viewModel =model;

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



        [HttpPost]
        public async Task<JsonResult> ListarEncriptar(ListarSolicitudesParameterVM model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                string url = $"{model.FechaIngreso}|{model.CampoCodSolicitud}|{model.CampoRuc}|{model.CampoRazonSocial}|{model.NombreContacto}|{model.CodEstado}";
                // FechaIngreso=&CampoCodSolicitud=&CampoRuc=&CampoRazonSocial=&NombreContacto=&CodEstado=0

                string urlEncriptado = this.GetUriHost() + Url.Action("ListarSolicitudes", "Solicitudes", new { area = "GestionarSolicitudes" }) + "?parkey=" + Encriptador.Instance.EncriptarTexto(url);

                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = urlEncriptado;



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BuscarEncriptar");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }
            return Json(ActionResponse);
        }



        [HttpGet]
        public async Task<IActionResult> VerSolicitud(string parkey)
        {
            AsignacionEvaluacionModel asignacionEvaluacionModel = new AsignacionEvaluacionModel();

            try
            {
                var nroSolicitud = Encriptador.Instance.DesencriptarTexto(parkey);

                asignacionEvaluacionModel.SolicitudVM = await _serviceSolicitud.obtenerSolicitudPorCodigo(nroSolicitud);
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
