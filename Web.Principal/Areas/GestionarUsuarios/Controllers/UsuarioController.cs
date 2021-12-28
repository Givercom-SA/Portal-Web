using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.Areas.GestionarUsuarios.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Areas.GestionarUsuarios.Controllers
{
    [Area("GestionarUsuarios")]
    public class UsuarioController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;

        public UsuarioController(
            ServicioAcceso serviceAcceso,
            ServicioUsuario serviceUsuario,
            IMapper mapper)
        {
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CrearUsuario()
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.Activo = 1;
            var result = await _serviceAcceso.ObtenerPerfiles(parameter);

            ViewBag.Perfiles = result.Perfiles;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(int Id)
        {
            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.Activo = 1;
            var resultPerfiles = await _serviceAcceso.ObtenerPerfiles(parameterPerfil);
            ViewBag.Perfiles = resultPerfiles.Perfiles;


            EditarUsuarioInternoModel model = new EditarUsuarioInternoModel();
            model.Correo = result.usuario.Correo;
            model.Nombres = result.usuario.Nombres;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.Activo = result.usuario.Activo;
            model.EsAdmin = result.usuario.EsAdmin;
            model.Perfil = result.usuario.IdPerfil;
            model.Items = result.usuario.Menus;
            model.IdEntidad = result.usuario.IdEntidad;

            ViewBag.IdUsuario = Id;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> VerUsuario(int Id)
        {
            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);

            return PartialView("_VerUsuario", result);
        }

        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            Models.ListarUsuariosModel model = new Models.ListarUsuariosModel();
            ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
            listarUsuarioParameterVM.ApellidoMaterno = "";
            listarUsuarioParameterVM.ApellidoPaterno = "";
            listarUsuarioParameterVM.Nombres = "";
            listarUsuarioParameterVM.Correo = "";
            listarUsuarioParameterVM.RegistroInicio = 1;
            listarUsuarioParameterVM.RegistroFin = 100;
            var result = await _serviceUsuario.ObtenerListadoUsuarios(listarUsuarioParameterVM);
            await cargarListas();
            model.ListUsuarios = result;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ListarUsuarios(ListarUsuariosModel model)
        {
            ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
            listarUsuarioParameterVM.ApellidoMaterno = model.ApellidoMaterno;
            listarUsuarioParameterVM.ApellidoPaterno = model.ApellidoPaterno;
            listarUsuarioParameterVM.Nombres = model.Nombres;
            listarUsuarioParameterVM.Correo = model.Correo;
            listarUsuarioParameterVM.IdPerdil = model.IdPerfil;
            listarUsuarioParameterVM.isActivo = model.isActivo;
            listarUsuarioParameterVM.RegistroInicio = 1;
            listarUsuarioParameterVM.RegistroFin = 100;
            var result = await _serviceUsuario.ObtenerListadoUsuarios(listarUsuarioParameterVM);

            model.ListUsuarios = result;
          await  cargarListas();

            return View(model);
        }


        private async Task  cargarListas() {

            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesActivos(new ListarPerfilActivosParameterVM());

            ViewBag.ListaPerfilActivos = resultPerfiles;
        }

        [HttpPost]
        public async Task<JsonResult> CrearUsuario(CrearUsuarioIntenoModel usuario)
        {
            ActionResponse = new ActionResponse();
            if (ModelState.IsValid)
            {
                if (usuario.Menus != null)
                {
                    var UrlInfo = Url.ActionContext.HttpContext.Request;
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdEntidad = 0;
                    parameterVM.IdPerfil = Convert.ToInt32(usuario.Perfil);
                    parameterVM.Correo = usuario.Correo;
                    parameterVM.Nombres = usuario.Nombres;
                    parameterVM.ApellidoMaterno = usuario.ApellidoMaterno;
                    parameterVM.ApellidoPaterno = usuario.ApellidoPaterno;
                    parameterVM.EsAdmin = true;
                    parameterVM.Activo = true;
                    parameterVM.IdUsuarioCrea = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.Menus = usuario.Menus.ToList();
                    parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(usuario.Contrasenia);
                    parameterVM.ContraseniaNoCifrado = usuario.Contrasenia;
                    parameterVM.RequiereConfirmacion = true;
                    
                    var result = await _serviceUsuario.CrearUsuario(parameterVM);
                    if (result.CodigoResultado > 0)
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "El usuario ha sido creado correctamente y se envió sus datos de acceso al correo.";
                    }
                    else
                    {
                        ActionResponse.Codigo = -1;
                        ActionResponse.Mensaje = "Error, no se pudo crear al usuario.";
                    }

                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe Seleccionar accesos para el usuario";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error, en la validación de los datos.";
            }

            return Json(ActionResponse);

        }

        [HttpGet]
        public async Task<IActionResult> CuentaUsuario() {

            CuentaUsuarioVM model = new CuentaUsuarioVM();
            usuario = HttpContext.Session.GetUserContent();

            model.Usuario = usuario;

            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> ActualizarUsuario(EditarUsuarioInternoModel usuario)
        {
            ActionResponse = new ActionResponse();


            if (ModelState.IsValid)
            {
                if (usuario.Menus != null)
                {
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdUsuario = usuario.IdUsuario;
                    parameterVM.IdPerfil = usuario.Perfil;
                    parameterVM.Nombres = usuario.Nombres;
                    parameterVM.ApellidoMaterno = usuario.ApellidoMaterno;
                    parameterVM.ApellidoPaterno = usuario.ApellidoPaterno;
                    parameterVM.EsAdmin = usuario.EsAdmin;
                    parameterVM.Activo = usuario.Activo;
                 
                    parameterVM.IdUsuarioModifica = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.Menus = usuario.Menus.ToList();
                    var result = await _serviceUsuario.EditarUsuarioInterno(parameterVM);
                    if (result.CodigoResultado == 0)
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "El usuario ha sido actializado correctamente.";
                    }
                    else
                    {
                        ActionResponse.Codigo = result.CodigoResultado;
                        ActionResponse.Mensaje = "Error al actualizar al usuario.";
                    }

                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe Seleccionar accesos para el usuario";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error en la validación de los datos.";
            }

            return Json(ActionResponse);

        }

        [HttpGet]
        public async Task<JsonResult> ExisteCorreo(string Correo)
        {
            bool CorreoDisponible = true;

            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.Correo = Correo;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);
            //var ExisteUsuario = await _serviceUsuario.ExisteUsuario(Correo);
            //if (ExisteUsuario)
            if (result.usuario != null)
            {
                if (result.usuario.Correo.ToLower().Equals(Correo.ToLower()))
                    CorreoDisponible = false;
            }

            return Json(CorreoDisponible);

        }

        [HttpGet]
        public async Task<IActionResult> MenusPorPerfil(int IdPerfil, int IdUsuario)
        {
            List<UsuarioMenuVM> modelMenus = new List<UsuarioMenuVM>();
            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = IdUsuario;
            parameter.IdPerfil = IdPerfil;
            var result = await _serviceUsuario.ObtenerListaUsuarioMenu(parameter);
            modelMenus = result.Menus;
            return PartialView("_MenusPorPerfil", modelMenus);
        }

        [HttpPost]
        public async Task<JsonResult> CambiarClave(int IdUsuario, string Clave)
        {
            ActionResponse = new ActionResponse();
            if (IdUsuario > 0)
            {
                CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                parameterVM.IdUsuario = IdUsuario;
                //parameterVM.Correo = Correo;
                parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(Clave);
                var result = await _serviceUsuario.CambiarClaveUsuario(parameterVM);
                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = "Su clave ha sido cambiada correctamente.";
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo crear al usuario.";
            }

            return Json(ActionResponse);

        }



    }
}
