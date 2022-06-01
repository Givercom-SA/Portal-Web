using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.Util;
using ViewModel.Datos.Embarque.AsignarAgente;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.ServiceConsumer;
using AutoMapper;
using Web.Principal.ServiceExterno;
using static Utilitario.Constante.EmbarqueConstante;
using Web.Principal.Areas.GestionarSolicitudes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utilitario.Constante;
using Security.Common;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class AsignarAgenteController : BaseController
    {
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioUsuario _serviceUsuario;

        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("AsignarAgenteController");
        public AsignarAgenteController(ServicioEmbarque serviceEmbarque,
            ServicioEmbarques serviceEmbarques,
            ServicioUsuario serviceUsuario,
            ServicioMaestro serviceMaestro,
        IMapper mapper)
        {
            _serviceEmbarque = serviceEmbarque;
            _serviceEmbarques = serviceEmbarques;
            _serviceUsuario = serviceUsuario;
            _serviceMaestro = serviceMaestro;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaAsignados(string parkey)
        {
            AsignacionModel model = new AsignacionModel();
            AsignacionModel modelView = new AsignacionModel();

            if (parkey != null)
            {
                var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                string[] parametros = dataDesencriptada.Split('|');

                if (parametros.Count() > 1)
                {
                    string NROBL = parametros[0];
                    string NROOT = parametros[1];
                    string Estado = parametros[2];
                    // ListarAsignacionParameter.NROBL = &ListarAsignacionParameter.NROOT = &ListarAsignacionParameter.Estado = 0 & ListarAsignacionParameter.Estado =
                    model.ListarAsignacionParameter.NROBL = NROBL;
                    model.ListarAsignacionParameter.NROBL = NROOT;
                    model.ListarAsignacionParameter.Estado = Estado;
             

                }
            }

            modelView = model;



            if (usuario.TipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS))
            {
                modelView = await listarAsignadoAgenteAduanas(model);
                
            }
            else {
                modelView = await listarAsignacionAgenteAduanas(model);
            }

           
            var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(44);
            ViewBag.ListarEstado = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");



            modelView.TipoEntidad = usuario.TipoEntidad;


            return View(modelView);
        }

        [HttpPost]
        public async Task<JsonResult> ListarEncriptar(AsignacionModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                string url = $"{model.ListarAsignacionParameter.NROBL}|{model.ListarAsignacionParameter.NROOT}|{model.ListarAsignacionParameter.Estado}";
                // ListarAsignacionParameter.NROBL = &ListarAsignacionParameter.NROOT = &ListarAsignacionParameter.Estado = 0 & ListarAsignacionParameter.Estado =

                string urlEncriptado = this.GetUriHost() + Url.Action("ListaAsignados", "AsignarAgente", new { area = "GestionarEmbarques" }) + "?parkey=" + Encriptador.Instance.EncriptarTexto(url);

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
        public async Task<IActionResult> ListarAgenteAduanasHistorial(string IdAgenteAduana)
        {
            AsignarAgenteHistorialResultVM modelView = new AsignarAgenteHistorialResultVM();
            modelView = await _serviceEmbarque.AsignarAgenteHistorial(IdAgenteAduana);

            return PartialView("_ResultadoAsignadosHistorial", modelView);
           
        }



        [HttpPost("/AsignarAgente/PartialAsignadoAgente")]
        public async Task<IActionResult> _ListarAsignado(AsignacionModel model)
        {


            if (usuario.TipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS))
            {

                var viewModel = await listarAsignadoAgenteAduanas(model);
                return PartialView("_ResultadoAsignadosFiltro", viewModel.listAsignarAgenteResult);
            }
            else {

                var viewModel = await listarAsignacionAgenteAduanas(model);
                return PartialView("_ResultadoAsignacionFiltro", viewModel.listAsignarAgenteResult);
            }


            

        }


        [HttpGet]
        public async Task<IActionResult> ListaAsignacion(AsignacionModel model)
        {
            var viewModel = await listarAsignacionAgenteAduanas(model);
            var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(44);
            ViewBag.ListarEstado = new SelectList(listaEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            return View(viewModel);
        }

        private async Task<AsignacionModel> listarAsignadoAgenteAduanas(AsignacionModel model)
        {
            var viewModel = (model == null) ? new AsignacionModel() : model;

            viewModel.ListarAsignacionParameter.IdUsuarioAsignado = usuario.idUsuario;
            viewModel.ListarAsignacionParameter.IdEntidadAsignado = usuario.IdEntidad;
            viewModel.listAsignarAgenteResult = await _serviceEmbarque.ListarAsignados(viewModel.ListarAsignacionParameter);

            return viewModel;
        }

     

        private async Task<AsignacionModel> listarAsignacionAgenteAduanas(AsignacionModel model)
        {
            var viewModel = (model == null) ? new AsignacionModel() : model;
            viewModel.ListarAsignacionParameter.IdUsuarioAsigna = usuario.idUsuario;
            viewModel.ListarAsignacionParameter.IdEntidadAsigna = usuario.IdEntidad;
            viewModel.listAsignarAgenteResult = await _serviceEmbarque.ListarAsignacion(viewModel.ListarAsignacionParameter);

            return viewModel;
        }

        [HttpPost]
        public async Task<JsonResult> AnularAsignacion(int IdAsignacion, string Observacion, string KeyBL, string NroBl)
        {
            ActionResponse = new ActionResponse();

            if (!string.IsNullOrEmpty(Observacion))
            {
                var usuarioApiResult = await _serviceUsuario.ObtenerUsuario(usuario.idUsuario);
                string nombre = usuarioApiResult.Usuario.EntidadTipoDocumento == TipoDocumento.DNI ? usuarioApiResult.Usuario.EntidadRepresentanteNombre : usuarioApiResult.Usuario.EntidadRazonSocial;
                var resultAsginarEmbarqueExterno = await _serviceEmbarques.ActualizarAgenteAduanas(KeyBL, AgenteAduanaActualizaTipoOpereacion.Anular.ToString(), usuarioApiResult.Usuario.EntidadNroDocumneto, nombre);

                if (resultAsginarEmbarqueExterno == 1)
                {

                    var parameter = new AsignarAgenteEstadoParameterVM
                    {
                        Id = IdAsignacion,
                        IdUsuarioModifica = usuario.idUsuario,
                        Estado = "4", // Anulado
                        Observacion = Observacion,
                        LogoEmpresa=$"{this.GetUriHost()}/img/{this.usuario.Sesion.ImagenTransGroupEmpresaSeleccionado}"
                    };
                    var resultCrear = await _serviceEmbarque.AsignarAgenteCambiarEstado(parameter);
                    if (resultCrear.CodigoResultado == 0)
                    {
                        ActionResponse.Codigo = resultCrear.CodigoResultado;
                        ActionResponse.Mensaje = string.Format("La asignación al embarque {0} se anulo correctamente.", NroBl); 
                    }
                    else
                    {
                        ActionResponse.Codigo = resultCrear.CodigoResultado;
                        ActionResponse.Mensaje = resultCrear.MensajeResultado;
                    }
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "No es posible anular, intente mas tarde o comuniquese con su customer services.";
                }

            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Debe ingresar un comentario.";
            }
            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> AprobarRechazar(int Id, string Observacion, string Estado, string KeyBL, string servicio)
        {
            ActionResponse = new ActionResponse();
            string EstadoNombre = string.Empty;
            switch (Estado)
            {
                case "2": EstadoNombre = "aprobó"; break;
                case "3": EstadoNombre = "rechazó"; break;
            }


            var parameter = new AsignarAgenteEstadoParameterVM
            {
                Id = Id,
                IdUsuarioModifica = usuario.idUsuario,
                Estado = Estado, // Aprobar/Rechazar
                Observacion = Observacion
            };

            var embarqueDetalle = await _serviceEmbarques.ObtenerEmbarque(KeyBL, servicio);

            if (Estado.Equals("2"))
            {
                    var usuarioApiResult = await _serviceUsuario.ObtenerUsuario(usuario.idUsuario);


                    string nombre = usuarioApiResult.Usuario.EntidadTipoDocumento == TipoDocumento.DNI ? usuarioApiResult.Usuario.EntidadRepresentanteNombre : usuarioApiResult.Usuario.EntidadRazonSocial;
                    var resultAsginarEmbarqueExterno = await _serviceEmbarques.ActualizarAgenteAduanas(KeyBL, AgenteAduanaActualizaTipoOpereacion.Asignar.ToString(), usuarioApiResult.Usuario.EntidadNroDocumneto, nombre);

                    if (resultAsginarEmbarqueExterno == 1)
                    {
                        var resultCrear = await _serviceEmbarque.AsignarAgenteCambiarEstado(parameter);
                        if (resultCrear.CodigoResultado == 0)
                        {
                            ActionResponse.Codigo = resultCrear.CodigoResultado;
                            ActionResponse.Mensaje = string.Format("La asignación al embarque {0} se {1} correctamente.", embarqueDetalle.NROBL, EstadoNombre);



                        }
                        else
                        {
                            ActionResponse.Codigo = resultCrear.CodigoResultado;
                            ActionResponse.Mensaje = resultCrear.MensajeResultado;
                        }

                    }
                    else
                    {

                        ActionResponse.Codigo = -1;
                        ActionResponse.Mensaje = "No es posible anular, intente mas tarde o comunicarse con su customer service.";

                    }


            }
            else
            {

                if (!string.IsNullOrEmpty(Observacion))
                {

                    var resultCrear = await _serviceEmbarque.AsignarAgenteCambiarEstado(parameter);
                if (resultCrear.CodigoResultado == 0)
                {
                    ActionResponse.Codigo = resultCrear.CodigoResultado;
                    ActionResponse.Mensaje = string.Format("La asignación al embarque {0} se {1} correctamente.", embarqueDetalle.NROBL, EstadoNombre);

                }
                else
                {
                    ActionResponse.Codigo = resultCrear.CodigoResultado;
                    ActionResponse.Mensaje = resultCrear.MensajeResultado;
                }


                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe ingresar un comentario.";
                }

            }







            return Json(ActionResponse);
        }


    }
}
