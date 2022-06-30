using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceExterno;
using Web.Principal.Util;
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
using ViewModel.Datos.Parametros;
using Security.Common;
using ViewModel.Datos.Embarque.LiberacionCarga;

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
        public async Task<IActionResult> Buscar(string parkey)
        {
            var model = new EmbarqueBuscarModel();

          
            try
            {
                if (parkey != null)
                {
                    var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                    string[] parametros = dataDesencriptada.Split('|');

                    if (parametros.Count() > 1)
                    {
                        string servicio = parametros[0];
                        string origen = parametros[1];
                        string anio = parametros[2];
                        string tipofiltro = parametros[3];
                        string filtro = parametros[4];

                        model.Servicio = servicio;
                        model.Origen = origen;
                        model.Anio = anio;
                        model.TipoFiltro = tipofiltro;
                        model.Filtro = filtro;

                    }
                }


                if (model.Anio == null)
                    model.Anio = "";

                if (model.TipoFiltro == null)
                    model.TipoFiltro = "";

                var listServiceAnios = await _servicMaestro.ObtenerParametroPorIdPadre(53);
                var anioActual = DateTime.Now.Year;
                List<ParametrosVM >listaParametroAniosVM = new List<ParametrosVM>();            

                for (var itemAnios = Convert.ToInt32(listServiceAnios.ListaParametros.ElementAt(0).ValorCodigo); itemAnios <= anioActual; itemAnios++) {

                    listaParametroAniosVM.Add(new ParametrosVM() {  ValorCodigo= itemAnios.ToString() ,NombreDescripcion= itemAnios.ToString() });
                }

                listServiceAnios.ListaParametros = listaParametroAniosVM;

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
                                                                            short.Parse(model.TipoFiltro),
                                                                            model.Filtro == null ? "" : model.Filtro,
                                                                            resultSesion.obtenerTipoEntidadTransmares(), 
                                                                            resultSesion.Sesion.RucIngresadoUsuario,
                                                                            model.Servicio,
                                                                            model.Origen);
                }

            }
            catch (Exception err)
            {
                _logger.LogError(err, "Buscar");
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

            string[] parametros = dataDesencriptada.Split('|');
          

            string codigo = parametros[0]; 
            string anio = parametros[1]; 
            string tipofiltro= parametros[2];
            string filtro = parametros[3]; 
            string servicio = parametros[4]; 
            string origen = parametros[5];

            var resultSesion = HttpContext.Session.GetUserContent();
        

            EmbarqueDetalleModel model = new EmbarqueDetalleModel();
            model.EmbarqueDetalle = new Model.EmbarqueModel();
            model.Servicio = servicio;
            model.Origen = origen;
            model.ParKey = parkey;

            try
            {

                var tieneDesglose = _serviceEmbarques.ObtenerDesglose(codigo, 0, 0);

                model.TieneDesgloses = tieneDesglose.Result.ToString().Trim();


                model.TipoEntidad = usuario.TipoEntidad;

                if (TempData["MensajeDesglose"] != null)
                    ViewBag.MensajeDesglose = TempData["MensajeDesglose"];

                var embarque = await _serviceEmbarques.ObtenerEmbarque(codigo, servicio);
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

               //model.MostrarOpcionRegistroFacturacionTercero = Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.NO;

                if (model.EmbarqueDetalle.FLAG_COBROS_FACTURADOS.Trim().Equals("0"))
                {
                    if (model.EmbarqueDetalle.TIPO_PADRE.ToUpper().Equals("MASTER") &&
                        model.EmbarqueDetalle.FLAG_DESGLOSE.ToUpper().Equals("N"))
                    {
                        model.MostrarOpcionRegistroFacturacionTercero =
                                                        Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_INGRESO;
                    }
                    else if (model.EmbarqueDetalle.TIPO_PADRE.ToUpper().Equals("MASTER") &&
                             model.EmbarqueDetalle.FLAG_DESGLOSE.ToUpper().Equals("Y") &&
                             model.EmbarqueDetalle.CANTIDAD_DESGLOSE.Equals("0"))
                    { 
                        model.MensajeOpcionRegistroFacturacionTercero = $"No existe desgloses ingresados para el embarque {model.EmbarqueDetalle.NROBL}, por favor comunicarse con su asistente comercial.";
                        model.MostrarOpcionRegistroFacturacionTercero =
                                                        Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_NOINGRESA_MENSAJE;
                    }
                    else if (model.EmbarqueDetalle.TIPO_PADRE.ToUpper().Equals("MASTER") &&
                             model.EmbarqueDetalle.FLAG_DESGLOSE.ToUpper().Equals("Y") &&
                             int.Parse(model.EmbarqueDetalle.CANTIDAD_DESGLOSE) > 0)
                    {  
                        if (model.ExisteCobrosPagarRegistrado == false)
                        {
                            model.MostrarOpcionRegistroFacturacionTercero =
                                Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_NOINGRESA_MENSAJE;
                            model.MensajeOpcionRegistroFacturacionTercero = $"No hay cobros asignados por pagar para el embarque {model.EmbarqueDetalle.NROBL}. por favor de realizar dicho procedimiento.";
                        }
                        else
                        {
                            model.MostrarOpcionRegistroFacturacionTercero =
                                Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_INGRESO;
                        }

                    }
                    else if (model.EmbarqueDetalle.TIPO_PADRE.ToUpper().Equals("HOUSE"))
                    {
                        
                        var listarCobrosPagarPadreBeyBlResultVM = await _serviceEmbarque.ObtenerCobroPagarKeyBL(codigo); // ObtenerCobroPagarKeyBL

                        if (listarCobrosPagarPadreBeyBlResultVM.EmbarquePadreKeyBl != null)
                        {
                            model.MostrarOpcionRegistroFacturacionTercero =
                                Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_NOINGRESA_MENSAJE;
                            model.MensajeOpcionRegistroFacturacionTercero = $"No hay cobros asignados por pagar para el embarque {model.EmbarqueDetalle.NROBL}, por favor comunicarse con su forwarder.";
                        }
                        else
                        {
                            model.MostrarOpcionRegistroFacturacionTercero =
                                Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_INGRESO;
                        }

                    }

                }
                else {

                    model.MostrarOpcionRegistroFacturacionTercero =
                                                   Utilitario.Constante.EmbarqueConstante.EmbarqueMostrarOpcionRegFacturacionTercero.SI_NOINGRESA_MENSAJE;
                    model.MensajeOpcionRegistroFacturacionTercero = "No tiene cobros pendientes por facturar.";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Detalle");
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
        public async Task<IActionResult> ObtenerCobros(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            string[] parametros = dataDesencriptada.Split('|');
            string id = parametros[0];
            string servicio = parametros[1];

            ListaCobrosModel model = new ListaCobrosModel();

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(id, servicio);

                model = await _serviceEmbarques.ObtenerCobros(id, servicio);

                model.NroEmbarque = embarque.NROBL;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ObtenerCobros");

                model.listaCobros = new List<EmbarqueCobrosModel>();
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTracking(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

            string[] parametros = dataDesencriptada.Split('|');
            string id = parametros[0];
            string servicio = parametros[1];

            ListaTrackingModel model = new ListaTrackingModel();

            try
            {
                var embarque = await _serviceEmbarques.ObtenerEmbarque(id, servicio);

                model = await _serviceEmbarques.ObtenerTracking(id);
                model.NroEmbarque = embarque.NROBL;
                ViewBag.IdPerfil = usuario.IdPerfil;
                ViewBag.Condicion = embarque.DES_CONDICION.Trim().ToUpper();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ObtenerTracking");

                model.Tracking = new EmbarqueTrackingModel();
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AsignarCobroPagar(string parkey)
        {
            ViewBag.CantidadBlsHijos = 0;


        

            ListCobrosPendienteEmbarqueVM model = new();
            
            try
            {



                var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);


                string[] parametros = dataDesencriptada.Split('|');


                string IdKeyBl = parametros[0];
                string anio = parametros[1];
                string tipofiltro = parametros[2];
                string filtro = parametros[3];
                string servicio = parametros[4];
                string origen = parametros[5];


                model.Servicio = servicio;
                model.Origen = origen;
                model.ParKey = parkey;

                var embarqueSeleccionado = await _serviceEmbarques.ObtenerEmbarque(IdKeyBl, servicio);

                var listaBls = await _serviceEmbarques.ObtenerDesglosesEmbarque(IdKeyBl, 0,usuario.TipoEntidad,EmbarqueConstante.ProcesoSistema.ASIGNACION_PROCESO);
                ViewBag.ListaBlsHijos = new SelectList(listaBls.listaDesglose, "KEYBLD", "CONSIGNATARIO");
                ViewBag.CantidadBlsHijos = listaBls.listaDesglose.Count();

                var resultCobrosPendietne  = await CobrosPendientesEmabarque(IdKeyBl, embarqueSeleccionado.NROBL, "");

                model.CobrosPendientesEmbarque = resultCobrosPendietne.CobrosPendientesEmbarque;
                model.CobrosPendientesEmbarque.OrderBy(x => x.ID);
                if (model.CobrosPendientesEmbarque != null && model.CobrosPendientesEmbarque.Count() > 0)
                {
                    model.existeDesglosePendiente = 1;
                }

                model.KEYBL = IdKeyBl;
                model.BL = embarqueSeleccionado.NROBL;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AsignarCobroPagar");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LiberarCarga(LiberacionCargaListarModel liberacionCargaModel)
        {

            LiberacionCargaModel model = new LiberacionCargaModel();



            try
            {


                var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(liberacionCargaModel.ParKey);


                string[] parametros = dataDesencriptada.Split('|');


                string IdKeyBl = parametros[0];
                string anio = parametros[1];
                string tipofiltro = parametros[2];
                string filtro = parametros[3];
                string servicio = parametros[4];
                string origen = parametros[5];



                model.KeyBl = IdKeyBl;
                model.NroBl = liberacionCargaModel.NroBl;
                model.Servicio = servicio;
                model.Origen = origen;

                var listaDesglose = await _serviceEmbarques.ObtenerDesglosesEmbarque(IdKeyBl, 1,usuario.TipoEntidad, EmbarqueConstante.ProcesoSistema.LIBERACION_CARGA);
                model.listaDesglose = listaDesglose.listaDesglose;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LiberarCarga");
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
        public async Task<JsonResult> ValidacionAsignacionCobros(string parkey)
        {

            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

            string[] parametros = dataDesencriptada.Split('|');

            string KeyBL = parametros[0];
            string servicio = parametros[1];
        

            ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;

            var embarque = _serviceEmbarques.ObtenerEmbarque(KeyBL, servicio);

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
                _logger.LogError(ex, "ValidacionAsignacionCobros");
                ActionResponse.Codigo = 1;
                ActionResponse.Mensaje = string.Format("Ocurrió un error inesperado, intente más tarde. {0}", ex.Message);
               
            }

            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<JsonResult> ValidarLiberacionCarga(string KeyBL, string servicio)
        {
            ActionResponse = new ActionResponse();
            ActionResponse.Codigo = 0;

            var embarque = _serviceEmbarques.ObtenerEmbarque(KeyBL, servicio);

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
                _logger.LogError(ex, "ValidarLiberacionCarga");

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
                _logger.LogError(ex, "AsignarCobroPagar");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }

            return new JsonResult(ActionResponse);
        }

  

        [HttpPost]
        public async Task<JsonResult> BuscarEncriptar(EmbarqueBuscarModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                string url =$"{model.Servicio}|{model.Origen}|{model.Anio}|{model.TipoFiltro}|{model.Filtro}";
 

                string urlEncriptado = this.GetUriHost() +Url.Action("Buscar","Embarque",new { area="GestionarEmbarques"})+"?parkey=" + Encriptador.Instance.EncriptarTexto(url);

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




    
        [HttpPost]
        public async Task<JsonResult> ProgramarNotificaciones(string keyBld, string nroBl, string anio)
        {
            string mensaje = string.Empty;
            bool registrar = true;

            try{ 
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ProgramarNotificaciones");
               
                mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }

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
                _logger.LogError(ex, "AceptarExpressRelease");
                mensaje = "Ocurrio un Error Inesperado al momento de la Ejecución del Servicio.";
            }

            return Json(mensaje);
        }

        [HttpPost]
        public async Task<JsonResult> AutorizarLiberacion(CrearLiberacionCargaModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                CrearLiberacionCargaParameterVM crearLiberacionCargaParameterVM = new CrearLiberacionCargaParameterVM();
                crearLiberacionCargaParameterVM.Detalles = new List<LiberacionCargaDetalleVM>();

                crearLiberacionCargaParameterVM.KeyBLD = model.KeyBl;
                crearLiberacionCargaParameterVM.NroBL = model.NroBl;
                crearLiberacionCargaParameterVM.Servicio = model.Servicio;
                crearLiberacionCargaParameterVM.Origen = model.Origen;
                crearLiberacionCargaParameterVM.IdEmpresaGtrm = this.usuario.Sesion.CodigoTransGroupEmpresaSeleccionado;
                crearLiberacionCargaParameterVM.IdUsuarioCrea =this.usuario.idUsuario;
                crearLiberacionCargaParameterVM.IdSesion =Convert.ToInt32( this.usuario.Sesion.CodigoSesion);

                var seleccions = model.listaDesglose.Where(x=>  x.Check==true);

                if (seleccions != null || seleccions.Count() > 0)
                {
                    foreach (var keyBld in seleccions)
                    {
                       int rptaServicio= await _serviceEmbarques.ActualizarAutorizacionEmbarque(keyBld.KEYBLD);

                        ActionResponse.Mensaje = "Se realizo la liberación de carga de los embarques seleccionados.";
                        ActionResponse.Codigo = 0;

                        if (rptaServicio == 1)
                        {
                            LiberacionCargaDetalleVM liberacionCargaDetalle = new LiberacionCargaDetalleVM();
                            liberacionCargaDetalle.Consignatario = keyBld.CONSIGNATARIO;
                            liberacionCargaDetalle.KeyBl = keyBld.KEYBLD;
                            liberacionCargaDetalle.NroBl = keyBld.NROBL;
                            crearLiberacionCargaParameterVM.Detalles.Add(liberacionCargaDetalle);
                        }
                    }
                }
                else {
                    ActionResponse.Mensaje = "Debe seleccionar al menos un desglose";
                    ActionResponse.Codigo = 1;
                }


                if (crearLiberacionCargaParameterVM.Detalles.Count > 0) {

                   await _serviceEmbarque.CrearLiberacionCarga(crearLiberacionCargaParameterVM);
                }



            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "AutorizarLiberacion");
                ActionResponse.Mensaje = "Hubo un incidente en la Autorizacion de Liberación de dicho embarque, intentelo mas tarde.";
                ActionResponse.Codigo = 1;
            }

            return Json(ActionResponse);
        }


    }

}
