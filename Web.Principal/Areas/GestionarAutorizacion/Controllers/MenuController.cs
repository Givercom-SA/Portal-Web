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
using Web.Principal.Areas.GestionarAutorizacion.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Util;

namespace Web.Principal.Areas.GestionarAutorizacion.Controllers
{
    [Area("GestionarAutorizacion")]
    public class MenuController : BaseController
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("MenuController");

        public MenuController(
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
            ListarMenusModel model = new ListarMenusModel();
            
            if (parkey != null)
            {
                var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                string[] parametros = dataDesencriptada.Split('|');

                if (parametros.Count() > 1)
                {
                    string Nombre = parametros[0];
                    string TipoMenu = parametros[1];
                    string idMenuPadre = parametros[2];
                    string Activo = parametros[3];

                    model.Nombre = Nombre;
                    model.Activo = Activo == "" ? 0 : Convert.ToInt32(Activo);
                    model.IdMenuPadre = Int32.Parse(idMenuPadre);
                    model.TipoMenu = TipoMenu;
                    
                }
            }

            ListarMenusParameterVM listarMenusParameterVM = new ListarMenusParameterVM();
            listarMenusParameterVM.IdMenuPadre = model.IdMenuPadre;
            listarMenusParameterVM.Estado = model.Activo;
            listarMenusParameterVM.TipoMenu = model.TipoMenu;
            listarMenusParameterVM.Nombre = model.Nombre;

            var resultListarMenus = await _serviceAcceso.ListarMenus(listarMenusParameterVM);
            model.Menus = resultListarMenus.Menus;

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

            var listTipoMenu = await _serviceMaestro.ObtenerParametroPorIdPadre(87);
            model.ListTipoMenu = new SelectList(listTipoMenu.ListaParametros, "ValorCodigo", "NombreDescripcion");


            var listarTodoMenusResultVM = await _serviceAcceso.ListarTodoMenus();
            model.ListMenusPadre= new SelectList(listarTodoMenusResultVM.Menus, "IdMenu", "Nombre");


            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> ListarEncriptar(ListarMenusModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                string url = $"{model.Nombre}|{model.TipoMenu}|{model.IdMenuPadre}|{model.Activo}";
                
                string urlEncriptado = this.GetUriHost() + Url.Action("Listar", "Menu", new { area = "GestionarAutorizacion" }) + "?parkey=" + Encriptador.Instance.EncriptarTexto(url);

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


        [HttpGet]
        public async Task<IActionResult> Detalle(string parkey)
        {
            LeerMenusModel model = new LeerMenusModel();
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            int Id = Int32.Parse(dataDesencriptada);

            var resultListarMenus = await _serviceAcceso.LeerMenu(Id);
            model.Menu= resultListarMenus.Menu;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string parkey)
        {
            EditarMenusModel model = new EditarMenusModel();
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            int Id = Int32.Parse(dataDesencriptada);

            var resultListarMenus = await _serviceAcceso.LeerMenu(Id);
            model.Menu = resultListarMenus.Menu;

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");

            var listTipoMenu = await _serviceMaestro.ObtenerParametroPorIdPadre(87);
            model.ListTipoMenu = new SelectList(listTipoMenu.ListaParametros, "ValorCodigo", "NombreDescripcion");

            var listarTodoMenusResultVM = await _serviceAcceso.ListarTodoMenus();
            model.ListMenuPadre = new SelectList(listarTodoMenusResultVM.Menus, "IdMenu", "Nombre");

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Editar(NuevoMenusModel model)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                MantenimientoMenuParameterVM mantenimientoMenuParameter = new MantenimientoMenuParameterVM();
                mantenimientoMenuParameter.Menu = model.Menu;
                mantenimientoMenuParameter.Menu.IdUsuarioModifica = this.usuario.idUsuario;
                mantenimientoMenuParameter.Menu.IdSesion = Convert.ToInt32(this.usuario.Sesion.CodigoSesion);

                var vistaResultVM = await _serviceAcceso.MenuModificar(mantenimientoMenuParameter);

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
        public async Task<IActionResult> Nuevo( )
        {
            NuevoMenusModel model = new NuevoMenusModel();

            var listTipoMenu = await _serviceMaestro.ObtenerParametroPorIdPadre(87);
            model.ListTipoMenu = new SelectList(listTipoMenu.ListaParametros, "ValorCodigo", "NombreDescripcion");

            var listarTodoMenusResultVM = await _serviceAcceso.ListarTodoMenus();
            model.ListMenuPadre = new SelectList(listarTodoMenusResultVM.Menus, "IdMenu", "Nombre");

            model.Menu = new MenuVM();
            model.Menu.Visible = true;
            model.Menu.Orden = 0;


            var resultVistarParaMenus = await _serviceAcceso.ListarTodasVistasParaMenu();
            model.Menu.Vistas = resultVistarParaMenus.Menu.Vistas;

            return View(model);

         
        }
        public async Task<JsonResult> Nuevo(NuevoMenusModel model)
        {
            ActionResponse = new ActionResponse();


            if (ModelState.IsValid)
            {


                MantenimientoMenuParameterVM mantenimientoMenuParameter = new MantenimientoMenuParameterVM();
                mantenimientoMenuParameter.Menu = model.Menu;
                
                mantenimientoMenuParameter.Menu.Activo = true;
                mantenimientoMenuParameter.Menu.IdUsuarioCrea = this.usuario.idUsuario;
                mantenimientoMenuParameter.Menu.IdSesion = Convert.ToInt32(this.usuario.Sesion.CodigoSesion);


                var vistaResultVM = await _serviceAcceso.MenuRegistrar(mantenimientoMenuParameter);


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
