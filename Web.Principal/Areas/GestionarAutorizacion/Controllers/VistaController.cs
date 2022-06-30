using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Security.Common;
using Service.Common.Logging.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Menu;
using ViewModel.Datos.Vista;
using Web.Principal.Areas.GestionarAutorizacion.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Util;

namespace Web.Principal.Areas.GestionarAutorizacion.Controllers
{
    [Area("GestionarAutorizacion")]
    public class VistaController : BaseController
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("VistaController");

        public VistaController(
          ServicioMaestro serviceMaestro,
          ServicioAcceso serviceAcceso,
          ServicioUsuario serviceUsuario,
          IMapper mapper)
        {
            _serviceMaestro = serviceMaestro;
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Listar");
        }

        [HttpGet]
        public async Task<IActionResult> Listar(string parkey)
        {
            ListarVistasModel model = new ListarVistasModel();

            if (parkey != null)
            {
                var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                string[] parametros = dataDesencriptada.Split('|');

                if (parametros.Count() > 1)
                {
                    string Nombre = parametros[0];
                    string Activo = parametros[1];
                    string IdPadre = parametros[2];
                    string Area = parametros[3];
                    string Controller = parametros[4];
                    string Action = parametros[5];


                    model.Nombre = Nombre;
                    model.Activo = Activo == "" ? 0 : Convert.ToInt32(Activo);
                    model.IdVistaPadre = Int32.Parse(IdPadre);
                    model.Area = Area;
                    model.Controller = Controller;
                    model.Action = Action;
                }
            }

            ListarVistaParameterVM listarMenusParameterVM = new ListarVistaParameterVM();
            listarMenusParameterVM.IdVistaPadre = model.IdVistaPadre;
            listarMenusParameterVM.Estado = model.Activo;
            listarMenusParameterVM.Nombre = model.Nombre;
            listarMenusParameterVM.Area = model.Area;
            listarMenusParameterVM.Controller = model.Controller;
            listarMenusParameterVM.Action = model.Action;

            var resultListarMenus = await _serviceAcceso.ListarVistas(listarMenusParameterVM);
            model.Vistas = resultListarMenus.Vistas;

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
           
            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

            var listarTodoVistaResult = await _serviceAcceso.ListarTodoVistas();

            model.ListarVistaPadres = new SelectList(listarTodoVistaResult.Vistas, "IdVista", "Nombres");

            var resultListarSoloArea = await _serviceAcceso.ListarSoloArea(new ViewModel.Datos.Vista.ListarAreaControllerActionParameterVM());
            model.ListArea = new SelectList(resultListarSoloArea.AreasControllersActions, "Area", "Area"); ;

            var resultListarSoloController = await _serviceAcceso.ListarSoloController(
                new ViewModel.Datos.Vista.ListarAreaControllerActionParameterVM() { Area=model.Area });
            model.ListController = new SelectList(resultListarSoloController.AreasControllersActions, "Controller", "Controller"); 

            var resultListarSoloAction = await _serviceAcceso.ListarSoloAction(
                new ViewModel.Datos.Vista.ListarAreaControllerActionParameterVM() { Area = model.Area, Controller = model.Controller });
            model.ListAction = new SelectList(resultListarSoloAction.AreasControllersActions, "Action", "Action"); 




            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> ListarEncriptar(ListarVistasModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {
                string url = $"{model.Nombre}|{model.Activo}|{model.IdVistaPadre}|{model.Area}|{model.Controller}|{model.Action}";
                string urlEncriptado = this.GetUriHost() + Url.Action("Listar", "Vista", new { area = "GestionarAutorizacion" }) + "?parkey=" + Encriptador.Instance.EncriptarTexto(url);

                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = urlEncriptado;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarEncriptar");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }
            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> ListarSoloController(ListarAreaControllerActionParameterVM model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                var resultListarSoloController = await _serviceAcceso.ListarSoloController(model);

                ActionResponse.Codigo = resultListarSoloController.CodigoResultado;
                ActionResponse.Mensaje = resultListarSoloController.MensajeResultado;
                ActionResponse.Objeto = resultListarSoloController.AreasControllersActions;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarSoloController");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }
            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> ListarSoloAction(ListarAreaControllerActionParameterVM model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                var resultListarSoloAction = await _serviceAcceso.ListarSoloAction(model);

                ActionResponse.Codigo = resultListarSoloAction.CodigoResultado;
                ActionResponse.Mensaje = resultListarSoloAction.MensajeResultado;
                ActionResponse.Objeto = resultListarSoloAction.AreasControllersActions;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarSoloAction");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }
            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(string parkey)
        {
            LeerVistasModel model = new LeerVistasModel();
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            int Id = Int32.Parse(dataDesencriptada);
            var resultVistas = await _serviceAcceso.LeerVista(Id);
            model.Vista = resultVistas.Vista;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string parkey)
        {
            EditarVistasModel model = new EditarVistasModel();
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            int Id = Int32.Parse(dataDesencriptada);
            var resultVistas = await _serviceAcceso.LeerVista(Id);
            model.NuevaVista = new MantenimientoVistaParameterVM();
            model.NuevaVista.Vista = resultVistas.Vista;

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            
            var listarTodoVistaResult = await _serviceAcceso.ListarTodoVistas();
            model.ListarVistaPadres = new SelectList(listarTodoVistaResult.Vistas, "IdVista", "Nombres");

            return View(model);
            
        }

        [HttpPost]
        public async Task<JsonResult> Editar(EditarVistasModel model)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                MantenimientoVistaParameterVM mantenimientoVistaParameterVM = new MantenimientoVistaParameterVM();
                mantenimientoVistaParameterVM.Vista = model.NuevaVista.Vista;
                mantenimientoVistaParameterVM.Vista.Activo = true;
                mantenimientoVistaParameterVM.Vista.IdUsuarioModifica = this.usuario.idUsuario;
                mantenimientoVistaParameterVM.Vista.IdSesion = Convert.ToInt32(this.usuario.Sesion.CodigoSesion);

                var vistaResultVM = await _serviceAcceso.VistaModificar(mantenimientoVistaParameterVM);

                ActionResponse.Codigo = vistaResultVM.CodigoResultado;
                ActionResponse.Mensaje = vistaResultVM.MensajeResultado;
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario, ingresar los campos obligatorios.";
            }

            return Json(ActionResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            NuevoVistasModel model = new NuevoVistasModel();
            model.NuevaVista = new MantenimientoVistaParameterVM();
            model.NuevaVista.Vista = new VistaVM();
            model.NuevaVista.Vista.VistaPrincipal = -1;
            model.NuevaVista.Vista.VistaOpcion = -1;
            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            model.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy");
            model.UsuarioCrea = this.usuario.NombresUsuario;
            var listarTodoVistaResult = await _serviceAcceso.ListarTodoVistas();
            model.ListarVistaPadres = new SelectList(listarTodoVistaResult.Vistas, "IdVista", "Nombres");

            return View(model);
        }

        public async Task<JsonResult> Nuevo(NuevoVistasModel model)
        {
            ActionResponse = new ActionResponse();


            if (ModelState.IsValid)
            {
                MantenimientoVistaParameterVM mantenimientoVistaParameterVM = new MantenimientoVistaParameterVM();
                mantenimientoVistaParameterVM.Vista = model.NuevaVista.Vista;
              
                mantenimientoVistaParameterVM.Vista.Activo = true;
                mantenimientoVistaParameterVM.Vista.IdUsuarioCrea = this.usuario.idUsuario;
                mantenimientoVistaParameterVM.Vista.IdSesion = Convert.ToInt32(this.usuario.Sesion.CodigoSesion);

                var vistaResultVM = await _serviceAcceso.VistaRegistrar(mantenimientoVistaParameterVM);

                ActionResponse.Codigo = vistaResultVM.CodigoResultado;
                ActionResponse.Mensaje = vistaResultVM.MensajeResultado;
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario, ingresar los campos obligatorios.";
            }

            return Json(ActionResponse);
        }
    }
}
