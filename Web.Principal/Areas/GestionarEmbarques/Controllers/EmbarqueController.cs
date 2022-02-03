using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceExterno;
using Web.Principal.Utils;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.Model;
using Web.Principal.ServiceConsumer;
using ViewModel.Datos.Embarque.AsignarAgente;
using ViewModel.Datos.RegistrarNotificacionArribo;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.CobrosPagar;
using static Utilitario.Constante.EmbarqueConstante;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using ViewModel.Datos.ListaExpressRelease;
using Utilitario.Constante;
using TransMares.Core;
using ViewModel.Datos.Message;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class EmbarqueController : BaseController
    {
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _servicMaestro;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioMessage _servicioMessage;
        
        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("EmbarqueController");


        public EmbarqueController(ServicioEmbarques serviceEmbarques,
                                    ServicioMaestro servicMaestro,
                                    ServicioEmbarque serviceEmbarque,
                                    ServicioUsuario serviceUsuario,
                                    ServicioMessage servicioMessage,
        IMapper mapper)
        {
            _serviceEmbarques = serviceEmbarques;
            _serviceEmbarque = serviceEmbarque;
            _servicMaestro = servicMaestro;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(EmbarqueBuscarModel model)
        {
            var viewModel = (model == null) ? new EmbarqueBuscarModel() : model;

            try
            {
                if (model.Anio == null)
                    model.Anio = "";

                if (model.TipoFiltro == null)
                    model.TipoFiltro = "";


                var listServiceAnios = await _servicMaestro.ObtenerParametroPorIdPadre(53);
                model.ListAnios = new SelectList(listServiceAnios.ListaParametros, "ValorCodigo", "NombreDescripcion");

                var listServiceTipoFiltro = await _servicMaestro.ObtenerParametroPorIdPadre(49);
                model.TipoFiltros = new SelectList(listServiceTipoFiltro.ListaParametros, "ValorCodigo", "NombreDescripcion");

                var listServicios = await _servicMaestro.ObtenerParametroPorIdPadre(12);
                model.ListaServicios = new SelectList(listServicios.ListaParametros, "ValorCodigo", "NombreDescripcion");

                var listOrigen = await _servicMaestro.ObtenerParametroPorIdPadre(79);
                model.ListaOrigen = new SelectList(listOrigen.ListaParametros, "ValorCodigo", "NombreDescripcion");


                var resultSesion = HttpContext.Session.GetUserContent();

                if (model.Anio != "" && model.TipoFiltro != "")
                {
                    model.listEmbarques = await _serviceEmbarques.ListarEmbarques(resultSesion.Sesion.CodigoTransGroupEmpresaSeleccionado,
                                                                            short.Parse(model.Anio), 
                                                                            model.Servicio,
                                                                            model.Origen,
                                                                            short.Parse(model.TipoFiltro), model.Filtro == null ? "" : model.Filtro,
                                                                            resultSesion.obtenerTipoEntidadTransmares(), resultSesion.Sesion.RucIngresadoUsuario);
                }

            }
            catch (Exception err)
            {
                _logger.LogError(err, "Buscar Embarque");
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(string codigo, string anio, string tipofiltro, string filtro, string servicio, string origen)
        {
            var resultSesion = HttpContext.Session.GetUserContent();

            EmbarqueDetalleModel model = new EmbarqueDetalleModel();
            model.EmbarqueDetalle = new Model.EmbarqueModel();
            model.Servicio = servicio;
            model.Origen = origen;

            try
            {

                var tieneDesglose = _serviceEmbarques.ObtenerDesglose(codigo, 0, 0);

                model.TieneDesgloses = tieneDesglose.Result.ToString().Trim();


                model.TipoEntidad = usuario.TipoEntidad;

                if (TempData["MensajeDesglose"] != null)
                    ViewBag.MensajeDesglose = TempData["MensajeDesglose"];

                var embarque = await _serviceEmbarques.ObtenerEmbarque(codigo);
                model.EmbarqueDetalle = embarque;


                var listAgentes = await _serviceEmbarque.ListarAgentes(6, usuario.idUsuario);
                ViewBag.Agentes = listAgentes.Usuarios;

                VerificarAsignacionAgenteAduanasParameterVM parameter = new VerificarAsignacionAgenteAduanasParameterVM();
                parameter.KEYBLD = codigo;

                var resultVerificarAsignacionAgenteAduanas = await _serviceEmbarque.VerificarAsignacionAgenteAduanas(parameter);
                model.EstaAsginadoAgenteAduanas = resultVerificarAsignacionAgenteAduanas.CodigoResultado.ToString();

                if (usuario.TipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS.ToString()))
                {
                    model.SoyAgenteAduanas = true;
                }
                else
                {
                    model.SoyAgenteAduanas = false;
                }


                var resultCobroPagar = await VerificarEmbarqueCobroPagarRegistrado(codigo);
                model.listaCobrosPagar = resultCobroPagar.ListaCobrosPagar;
                model.ExisteCobrosPagarRegistrado = resultCobroPagar.ExisteCobrosPagarRegistrado;

                var listaExpressRelease = await _serviceEmbarque.ListarExpressReleaseAceptadas();

                if (listaExpressRelease.listaExpressRelease.Where(w => w.KeyBl.Equals(embarque.KEYBLD) && w.NroBl.Equals(embarque.NROBL)).Count() == 1)
                    ViewBag.MostrarExpressRelease = false;
                else
                    ViewBag.MostrarExpressRelease = true;

                model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.NO;

                if (model.EmbarqueDetalle.FLAG_COBROS_FACTURADOS.Trim().Equals("0"))
                {
                    if (model.TipoEntidad.Trim().Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.CLIENTE_FINAL.ToString()))
                    {
                        if (model.EmbarqueDetalle.TIPO_PADRE.ToUpper().Equals("MASTER"))
                        {
                            if (model.ExisteCobrosPagarRegistrado == false)
                            {
                                model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.MENSAJE;
                                model.MensajeOpcionRegistroFacturacionTercero = "No hay cobros asignados por pagar, por favor coordinar con el forwarder.";
                            }
                            else
                            {
                                model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.SI;
                            }
                        }
                        else
                        {


                            var resultCobroPagarPadereKeyBl = await _serviceEmbarque.ObtenerCobroPagarPadreKeyBL(codigo);

                            if (resultCobroPagarPadereKeyBl.EmbarquePadreKeyBl != null)
                            {

                                var resultCobroPagarPadre = await VerificarEmbarqueCobroPagarRegistrado(resultCobroPagarPadereKeyBl.EmbarquePadreKeyBl.EmbarqueKeyBL);

                                if (resultCobroPagarPadre.ExisteCobrosPagarRegistrado == false)
                                {
                                    model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.MENSAJE;
                                    model.MensajeOpcionRegistroFacturacionTercero = "No hay cobros asignados por pagar, por favor coordinar con el forwarder.";
                                }
                                else
                                {
                                    model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.SI;
                                }


                            }

                        }

                    }
                    else
                    {
                        if (model.ExisteCobrosPagarRegistrado == false)
                        {
                            model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.MENSAJE;
                            model.MensajeOpcionRegistroFacturacionTercero = "No hay cobros asignados por pagar.";
                        }
                        else
                        {
                            model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueOpcionRegFacturacionTercero.SI;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno Embarque");
            }

            ViewBag.Codigo = codigo;
            ViewBag.Anio = anio;
            ViewBag.Filtro = filtro;

            return View(model);
        }



        private async Task<CobroPagarRegistradoResponse> VerificarEmbarqueCobroPagarRegistrado(string keybl)
        {

            var listCobrosPagar = await _serviceEmbarque.ObtenerCobrosPagar(keybl, string.Empty, string.Empty, string.Empty);

            listCobrosPagar.ListaCobrosPagar.OrderBy(x => x.Id);

            CobroPagarRegistradoResponse cobroPagarRegistradoResponse = new CobroPagarRegistradoResponse();

            cobroPagarRegistradoResponse.ListaCobrosPagar = listCobrosPagar;

            
            if (cobroPagarRegistradoResponse.ListaCobrosPagar != null)
            {

                if (cobroPagarRegistradoResponse.ListaCobrosPagar.ListaCobrosPagar.Count() > 0)
                {
                    cobroPagarRegistradoResponse.ExisteCobrosPagarRegistrado = true;
                }
                else
                {
                    cobroPagarRegistradoResponse.ExisteCobrosPagarRegistrado = false;
                }

            }
            else
            {
                cobroPagarRegistradoResponse.ExisteCobrosPagarRegistrado = false;
            }

            return cobroPagarRegistradoResponse;
        }


        class CobroPagarRegistradoResponse {

            
            public ListarCobrosPagarResultVM ListaCobrosPagar { get; set; }
            public bool ExisteCobrosPagarRegistrado { get; set; }

        }




        [HttpGet]
        public async Task<IActionResult> ObtenerCobros(string id)
        {
            ListaCobrosModel model = new ListaCobrosModel();

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(id);

                model = await _serviceEmbarques.ObtenerCobros(id);

                model.NroEmbarque = embarque.NROBL;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Obtener cobros");

                model.listaCobros = new List<EmbarqueCobrosModel>();
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTracking(string id)
        {
            ListaTrackingModel model = new ListaTrackingModel();

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(id);

                model = await _serviceEmbarques.ObtenerTracking(id);
                model.NroEmbarque = embarque.NROBL;
                ViewBag.IdPerfil = usuario.IdPerfil;
                ViewBag.Condicion = embarque.DES_CONDICION.Trim().ToUpper();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Obtener Tracking");

                model.Tracking = new EmbarqueTrackingModel();
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AsignarCobroPagar(string id)
        {
            ViewBag.CantidadBlsHijos = 0;
            ListCobrosPendienteEmbarqueVM model = new();
            
            try
            {

                var embarqueSeleccionado = await _serviceEmbarques.ObtenerEmbarque(id);

                var listaBls = await _serviceEmbarques.ObtenerDesglosesEmbarque(id, 0,usuario.TipoEntidad,EmbarqueConstante.ProcesoSistema.ASIGNACION_PROCESO);
                ViewBag.ListaBlsHijos = new SelectList(listaBls.listaDesglose, "KEYBLD", "CONSIGNATARIO");
                ViewBag.CantidadBlsHijos = listaBls.listaDesglose.Count();

                var resultCobrosPendietne  = await CobrosPendientesEmabarque(id, embarqueSeleccionado.NROBL, "");

                model.CobrosPendientesEmbarque = resultCobrosPendietne.CobrosPendientesEmbarque;
                model.CobrosPendientesEmbarque.OrderBy(x => x.ID);
                if (model.CobrosPendientesEmbarque != null && model.CobrosPendientesEmbarque.Count() > 0)
                {
                    model.existeDesglosePendiente = 1;
                }

                model.KEYBL = id;
                model.BL = embarqueSeleccionado.NROBL;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Asignar Cobros");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LiberarCarga(string keyBl)
        {
            LiberacionCargaModel model = new LiberacionCargaModel();
            try
            {

               var listaDesglose = await _serviceEmbarques.ObtenerDesglosesEmbarque(keyBl, 1,usuario.TipoEntidad, EmbarqueConstante.ProcesoSistema.LIBERACION_CARGA);
                model.listaDesglose = listaDesglose.listaDesglose;




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en carga de vista liberación de carga");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return PartialView("_ResultListLiberarCarga", model);
        }

        [HttpGet]
        public async Task<IActionResult> ListarCobrosPagar(string KeyBLD, string BL, string BLNieto)
        {
            ListCobrosPendienteEmbarqueVM modelView = new ListCobrosPendienteEmbarqueVM();
            try
            {
                modelView = await CobrosPendientesEmabarque(KeyBLD, BL, BLNieto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al momento de obtener cobros a pagar");
                modelView.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                modelView.Message = "Error inesperado, por favor volver a intentar mas tarde";
                modelView.StatusResponse = "-100";
            }

            return PartialView("_ResultadoBusquedaCobrosPagar", modelView);
        }

        [HttpGet]
        public async Task<IActionResult> ListarCobrosPorPagarAsignados(string KeyBLD)
        {
            ListarCobrosPagarResultVM modelView = new ListarCobrosPagarResultVM();
            try
            {
                modelView = await _serviceEmbarque.ObtenerCobrosPagar(KeyBLD, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al momento de obtener cobros a pagar");
                modelView.CodigoResultado = -1;
                modelView.MensajeResultado = "Error inesperado, por favor volver a intentar mas tarde";
            }

            return PartialView("_ResultadoCobrosPorPagarAsignados", modelView);
        }

        [HttpPost]
        public async Task<JsonResult> AnularCobroPorPagarAsignado(int Id)
        {
            ActionResponse ActionResponse = new ActionResponse();

            try
            {
                var parameter = new CobrosPagarParameterVM
                {
                    Id = Id,
                    Estado = "02", // Anulado
                    IdUsuario = usuario.idUsuario
                };

                var result = await _serviceEmbarque.ActualizarCobrosPagar(parameter);
                ActionResponse.Codigo = result.CodigoResultado;
                ActionResponse.Mensaje = result.MensajeResultado;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AnularCobroPorAsignar");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde";
            }

            return new JsonResult(ActionResponse);
        }

        private async Task<ListCobrosPendienteEmbarqueVM> CobrosPendientesEmabarque(string KeyBLD, string BL, string BLNieto)
        {
            var result = new ListCobrosPendienteEmbarqueVM();

            //var resultCobrosPagar = await _serviceEmbarque.ObtenerCobrosPagar(KeyBLD, BL, BLNieto, string.Empty);

            result.CobrosPendientesEmbarque = await _serviceEmbarques.ObtenerCobrosPendienteEmbarque(KeyBLD, "0");

            result.KEYBL = KeyBLD;
            result.BL = BL;
            result.BlNietos = BLNieto;

            return result;
        }

        [HttpGet]
        public async Task<JsonResult> ValidacionAsignacionCobros(string KeyBL)
        {
            ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;

            var embarque = _serviceEmbarques.ObtenerEmbarque(KeyBL);

            try
            {

                var tieneDesglose = _serviceEmbarques.ObtenerDesglose(KeyBL, 0, 0);
                var listaBls = await _serviceEmbarques.ObtenerDesglosesEmbarque(KeyBL, 0,usuario.TipoEntidad, EmbarqueConstante.ProcesoSistema.ASIGNACION_PROCESO);
                var resultCobrosPendietne = await CobrosPendientesEmabarque(KeyBL, embarque.Result.NROBL, "");

                if (resultCobrosPendietne.CobrosPendientesEmbarque == null || resultCobrosPendietne.CobrosPendientesEmbarque.Count() == 0)
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = $"El embarque {embarque.Result.NROBL} no tiene cobros pendientes.";
                }

                if ((embarque.Result.FLAG_DESGLOSE.Equals("Y") && embarque.Result.CANTIDAD_DESGLOSE.Equals("0")))
                {

                   

                    EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                    enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                    enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyAsignarCobroReportar(embarque.Result.NROBL, $"El sistema identifica que el Forwarder requiere asignar cobros asociados al BL <b>{embarque.Result.NROBL}</b> cuyo desglose está  pendiente de ingreso, favor revisar y actualizar.", usuario.Sesion.ImagenTransGroupEmpresaSeleccionado);
                    enviarMessageCorreoParameterVM.RequestMessage.Correo = embarque.Result.OPERADOR_MAIL;
                    enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"DESGLOSE PENDIENTE DE INGRESO - BL ASOCIADO {embarque.Result.NROBL}";
                   await  _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);



                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = $"Estimado Cliente:  <br /> Desglose no se encuentra en nuestra base de datos, favor comunicarse con su Ejecutivo Comercial o a la casilla <b>{ embarque.Result.OPERADOR_MAIL}</b> para solicitar asistencia";
                }
                else if (embarque.Result.FLAG_DESGLOSE.Equals("N"))
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = $"Estimado Cliente:  <br /> El embarque {embarque.Result.NROBL} no es posible realizar la acción porque no va desglosar, favor comunicarse con su Ejecutivo Comercial o a la casilla <b>{ embarque.Result.OPERADOR_MAIL}</b> para solicitar asistencia.";

                }

          

            }
            catch (Exception ex)
            {
                ActionResponse.Codigo = 1;
                ActionResponse.Mensaje = string.Format("Ocurrió un error inesperado, intente más tarde. {0}", ex.Message);
            }

            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<JsonResult> ValidarLiberacionCarga(string KeyBL )
        {
            ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;

            var embarque = _serviceEmbarques.ObtenerEmbarque(KeyBL);

            try
            {


                EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
                enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
                enviarMessageCorreoParameterVM.RequestMessage.Contenido = new FormatoCorreoBody().formatoBodyAsignarCobroReportar(embarque.Result.NROBL, $"El sistema identifica que el Forwarder requiere asignar cobros asociados al BL <b>{embarque.Result.NROBL}</b> cuyo desglose está  pendiente de ingreso, favor revisar y actualizar.", usuario.Sesion.ImagenTransGroupEmpresaSeleccionado);
                enviarMessageCorreoParameterVM.RequestMessage.Correo = embarque.Result.OPERADOR_MAIL;
                enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"DESGLOSE PENDIENTE DE INGRESO - BL ASOCIADO {embarque.Result.NROBL}";
                var ressult = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);

                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = $"Estimado Cliente:  <br /> Desglose no se encuentra en nuestra base de datos, favor comunicarse con su Ejecutivo Comercial o a la casilla <b>{ embarque.Result.OPERADOR_MAIL}</b> para solicitar asistencia";




            }
            catch (Exception ex)
            {
                ActionResponse.Codigo = 1;
                ActionResponse.Mensaje = string.Format("Ocurrió un error inesperado, intente más tarde. {0}", ex.Message);
            }

            return Json(ActionResponse);
        }


        [HttpPost]
        public async Task<JsonResult> AsignarCobroPagar(ListCobrosPendienteEmbarqueVM model)
        {
            ActionResponse ActionResponse = new ActionResponse();
            ActionResponse.ListActionListResponse = new List<ActionErrorResponse>();

            try
            {

                var listaBls = await _serviceEmbarques.ObtenerDesglosesEmbarque(model.KEYBL, 0, usuario.TipoEntidad, EmbarqueConstante.ProcesoSistema.ASIGNACION_PROCESO);

                if (model.CobrosPendientesEmbarque.Count > 0)
                {
                    var parameter = new CobrosPagarParameterVM
                    {
                        KeyBLD = model.KEYBL,
                        BL = model.BL,
                        BLNieto = model.BlNietos,
                        IdUsuario = usuario.idUsuario
                    };

                  
                 
                    var listaDetalle = new List<CobrosPagarDetalleVM>();

                    foreach (var item in model.CobrosPendientesEmbarque)
                    {
                        string strblPagar = "";
                        string strBlpagar = "";

                        var blPagar = listaBls.listaDesglose.Where(x => x.KEYBLD == item.IdBlPagar).FirstOrDefault();

                        if (blPagar != null) {
                            strblPagar = blPagar.CONSIGNATARIO;
                            
                            strBlpagar = blPagar.NROBL;
                        }

                        listaDetalle.Add(new CobrosPagarDetalleVM
                        {
                            ConceptoCodigo = item.ConceptoCodigo,
                            Concepto = item.ConceptoCodigoDescripcion,
                            RubroCodigo = item.RubroCodigo,
                            Descripcion = item.Descripcion,
                            Importe = item.Importe,
                            Moneda = item.Moneda,
                            IGV = item.Igv,
                            Total = item.Total,
                            FlagAsignacion = item.FlagAsignacion,
                            KeyBl = item.IdBlPagar,
                            BlPagar = strblPagar,
                            IdBlPagar = item.IdBlPagar,
                            NroBL = strBlpagar,
                            IdProvision=item.ID
                            
                        });

                        await _serviceEmbarques.ActualizarProvicionCobranza(item.IdBlPagar, long.Parse(item.ID));
                        
                    }

                    parameter.ListaDetalle = listaDetalle;                  

                    var result = await _serviceEmbarque.CrearCobrosPagar(parameter);

                    ActionResponse.Codigo = result.CodigoResultado;
                    ActionResponse.Mensaje = result.MensajeResultado;

                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Por favor seleccionar un elemento de la lista.";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Asignar Cobro Por Pagar Registro");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }

            return new JsonResult(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> AsignarAgente(string codigo, int IdUsuarioAsignado, string Correo, int IdEntidadAsignado)
        {
            ActionResponse = new ActionResponse();

            if (IdUsuarioAsignado == 0)
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Debe seleccionar un agente de aduanas.";
            }
            else
            {
                var resultSesion = HttpContext.Session.GetUserContent();

                EmbarqueDetalleModel model = new EmbarqueDetalleModel();
                model.EmbarqueDetalle = new Model.EmbarqueModel();

                var embarque = await _serviceEmbarques.ObtenerEmbarque(codigo);

                var parameterCrear = new AsignarAgenteCrearParameterVM();
                parameterCrear.KEYBLD = embarque.KEYBLD;
                parameterCrear.NROOT = embarque.NROOT;
                parameterCrear.NROBL = embarque.NROBL;
                parameterCrear.NRORO = embarque.NRORO;
                parameterCrear.EMPRESA = embarque.EMPRESA;
                parameterCrear.ORIGEN = embarque.ORIGEN;
                parameterCrear.CONDICION = embarque.CONDICION;
                parameterCrear.POL = embarque.POL;
                parameterCrear.POD = embarque.POD;
                parameterCrear.ETAPOD = embarque.ETAPOD;
                parameterCrear.EQUIPAMIENTO = embarque.EQUIPAMIENTO;
                parameterCrear.MANIFIESTO = embarque.MANIFIESTO;
                parameterCrear.COD_LINEA = embarque.COD_LINEA;
                parameterCrear.DES_LINEA = embarque.DES_LINEA;
                parameterCrear.CONSIGNATARIO = embarque.CONSIGNATARIO;
                parameterCrear.COD_INSTRUCCION = embarque.COD_INSTRUCCION;
                parameterCrear.DES_INSTRUCCION = embarque.DES_INSTRUCCION;
                parameterCrear.IdUsuarioAsigna = usuario.idUsuario;
                parameterCrear.IdUsuarioAsignado = IdUsuarioAsignado;
                parameterCrear.CorreoUsuarioAsignado = Correo;
                parameterCrear.Observacion = string.Empty;
                parameterCrear.Estado = "1"; // Pendiente
                parameterCrear.IdUsuarioCrea = usuario.idUsuario;
                parameterCrear.IdUsuarioModifica = 0;
                parameterCrear.IdEntidadAsignado = IdEntidadAsignado;
                parameterCrear.IdEntidadAsigna = usuario.IdEntidad;
                parameterCrear.LogoEmpresa =$"{this.GetUriHost()}/img/{usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}" ;

                var resultCrear = await _serviceEmbarque.AsignarAgenteCrear(parameterCrear);

                ActionResponse.Codigo = resultCrear.CodigoResultado;
                ActionResponse.Mensaje = resultCrear.MensajeResultado;


            }

            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> AsignarAgenteVerificar(string KeyBl)
        {
            ActionResponse = new ActionResponse();

            VerificarAsignacionAgenteAduanasParameterVM parameter = new VerificarAsignacionAgenteAduanasParameterVM();
            parameter.KEYBLD = KeyBl;

            var resultVerificarAsignacionAgenteAduanas = await _serviceEmbarque.VerificarAsignacionAgenteAduanas(parameter);

            ActionResponse.Codigo = resultVerificarAsignacionAgenteAduanas.CodigoResultado;
            ActionResponse.Mensaje = resultVerificarAsignacionAgenteAduanas.MensajeResultado;


            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> ProgramarNotificaciones(string keyBld, string nroBl, string anio)
        {
            string mensaje = string.Empty;
            bool registrar = true;

            var listaPendiente = _serviceEmbarque.ListarNotificacionesArriboPendientes().Result
                .ListaNotificacionesPendientes
                .Where(w => w.tipoDocumento == "NA")
                .ToList();

            if (listaPendiente.Count() > 0)
                foreach (var item in listaPendiente)
                {
                    if (item.KeyBld.Equals(keyBld))
                        registrar = false;
                }

            if (registrar)
            {
                var param = new RegistrarNotificacionArriboVM();
                param.keyBld = keyBld;
                param.idUsuario = usuario.idUsuario;
                param.tipoDocumento = "NA";
                param.NumeracionEmbarque = nroBl;
                param.CodigoGtrmEmpresa = usuario.Sesion.CodigoTransGroupEmpresaSeleccionado;

                var arrAnio = anio.Split('/');

                // Regsitra la Notificación Arribo en el Servicio RegistrarDocumento
                var notificar = await _serviceEmbarques.RegistrarDocumentoTipo(usuario.Sesion.CodigoTransGroupEmpresaSeleccionado, keyBld, nroBl, short.Parse(arrAnio[2]), usuario.CorreoUsuario, "NA");

                if (notificar == 1)
                    mensaje = await _serviceEmbarque.ProgramarNotificaciones(param);
                else if (notificar == 0)
                    mensaje = "No se puede generar la solicitud de notificacion de arribo, favor de comunicarse con su soporte del area comercial.";
                else
                    mensaje = "Hubo un inconveniente con el servicio, intentarlo mas tarde.";
            }
            else
                mensaje = "Su solicitud de notificación de arribo ya fue registrado, este atento a su correo.";

            return Json(mensaje);
        }

        [HttpGet("/Embarque/CobroPendietenFacturar")]
        public async Task<IActionResult> CobroPendientePorFacturar(string keyBl)
        {
            ListCobrosPendienteEmbarqueVM model = new ListCobrosPendienteEmbarqueVM();
            model.KEYBL = keyBl;

            model.CobrosPendientesEmbarque = await _serviceEmbarques.ObtenerCobrosPendienteEmbarque(keyBl,"0");

            return PartialView("_CobrosPendientesCobrarPartial", model);
        }

        [HttpPost("/Embarque/EmbarqueSolicitarCobroPendienteFactura")]
        public async Task<IActionResult> EmbarqueSolicitarCobroPendienteFactura(ListCobrosPendienteEmbarqueVM viewmodel)
        {
            var listTipoDocumnentoResult = await _servicMaestro.ObtenerParametroPorIdPadre(38);

            if (listTipoDocumnentoResult.CodigoResultado == 0)
            {
                ViewBag.ListTipoDocumento = new SelectList(listTipoDocumnentoResult.ListaParametros, "ValorCodigo", "NombreDescripcion");
            }
            return PartialView("_SolicitudFacturacionTerceroPartial", viewmodel);

        }

   

        [HttpPost]
        public async Task<JsonResult> AprobarRechazarFacturacionTercero(int Id, string Estado, string Correo)
        {
            ActionResponse = new ActionResponse();
            string EstadoNombre = string.Empty;
            switch (Estado)
            {
                case "SA": EstadoNombre = "aprobó"; break;
                case "SR": EstadoNombre = "rechazó"; break;
            }

            try
            {
                List<int> listDetalle = new List<int>();

                var sepListarDetalle = await _serviceEmbarque.ListarFacturacionTercerosDetalle(Id);

                switch (Estado)
                {
                    case "SA":

                        foreach (var item in sepListarDetalle.ListFacturacionTerceroDetalle)
                        {
                            if (sepListarDetalle.TipoEntidad.Equals("AA"))
                            {
                                listDetalle.Add(await _serviceEmbarques.ActualizarPagosTercero(sepListarDetalle.EmbarqueKeyBL, long.Parse(item.IdProvision), "", sepListarDetalle.AgenteNumeroDocumento, sepListarDetalle.AgenteRazonSocial));
                            }
                            else
                            {
                                listDetalle.Add(await _serviceEmbarques.ActualizarPagosTercero(sepListarDetalle.EmbarqueKeyBL, long.Parse(item.IdProvision), sepListarDetalle.CodigoCliente, sepListarDetalle.NroDocumento, sepListarDetalle.ClienteNombre));
                            }
                        }

                        ; break;
                    case "SR":

                        break;

                }



                var parameter = new RegistrarFacturacionTerceroParameterVM
                {
                    Id = Id,
                    Estado = Estado,
                    Correo = Correo,
                    LogoEmpresa = $"{this.GetUriHost()}/img/{this.usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}",
                    EmbarqueNroBL = sepListarDetalle.EmbarqueNroBL,
                    IdUsuarioEvalua=usuario.idUsuario,
                    IdUsuario= usuario.idUsuario.ToString()

                };
                var result = await _serviceEmbarque.ActualizarFacturacionTercero(parameter);
                if (result.CodigoResultado == 0)
                {
                    result.MensajeResultado = string.Format("La solicitud se {0} correctamente.", EstadoNombre);
                }
                ActionResponse.Codigo = result.CodigoResultado;
                ActionResponse.Mensaje = result.MensajeResultado;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aprobar rechzar facturacion a terceros");
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }

            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AceptarExpressRelease(string keyBld, string nroBl)
        {
            string mensaje = string.Empty;

            try
            {
                ExpressReleaseParameterVM param = new ExpressReleaseParameterVM();
                param.KeyBl = keyBld;
                param.NroBl = nroBl;
                param.Idusuario = usuario.idUsuario;

                mensaje = await _serviceEmbarque.RegistrarExpressRelease(param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aceptar Express Release");
                mensaje = "Ocurrio un Error Inesperado al momento de la Ejecución del Servicio.";
            }

            return Json(mensaje);
        }

        [HttpPost]
        public async Task<JsonResult> AutorizarLiberacion( LiberacionCargaModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                var seleccions = model.listaDesglose.Where(x=>  x.Check==true);

                if (seleccions != null || seleccions.Count() > 0)
                {
                    foreach (var keyBld in seleccions)
                    {

                        await _serviceEmbarques.ActualizarAutorizacionEmbarque(keyBld.KEYBLD);

                        ActionResponse.Mensaje = "Se realizo la liberación de carga de los embarques seleccionados.";
                        ActionResponse.Codigo = 0;
                    }
                }
                else {
                    ActionResponse.Mensaje = "Debe seleccionar al menos un desglose";
                    ActionResponse.Codigo = 1;

                }

            }
            catch(Exception e)
            {
                ActionResponse.Mensaje = "Hubo un incidente en la Autorizacion de Liberación de dicho embarque, intentelo mas tarde.";
                ActionResponse.Codigo = 1;
            }

            return Json(ActionResponse);
        }
    }

}
