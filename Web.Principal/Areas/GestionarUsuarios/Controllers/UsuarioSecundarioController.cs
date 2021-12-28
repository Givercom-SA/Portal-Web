using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;
using ViewModel.Datos.Perfil;
using Web.Principal.Areas.GestionarUsuarios.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Web.Principal.Areas.GestionarUsuarios.Controllers
{
    [Area("GestionarUsuarios")]
    public class UsuarioSecundarioController : BaseController
    {
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        
        public UsuarioSecundarioController(
            ServicioUsuario serviceUsuario,
            ServicioAcceso serviceAcceso,
            IMapper mapper, IConfiguration configuration)
        {
            _serviceUsuario = serviceUsuario;
            _serviceAcceso = serviceAcceso;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> CrearUsuario()
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdEntidad = Convert.ToInt32(ViewData["IdEntidad"]);
            var result = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameter);

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
            parameterPerfil.IdEntidad = Convert.ToInt32(ViewData["Identidad"]);
            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameterPerfil);
            ViewBag.Perfiles = resultPerfiles.Perfiles;


            UsuarioSecundarioModel model = new UsuarioSecundarioModel();
            model.Correo = result.usuario.Correo;
            model.Activo = result.usuario.Activo;
            model.Perfil = result.usuario.IdPerfil;
            model.PerfilNombre = result.usuario.PerfilNombre;
            model.Menus = result.usuario.Menus;
            ViewBag.IdUsuario = Id;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> VerUsuario(int Id)
        {
            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);

            UsuarioSecundarioModel model = new UsuarioSecundarioModel();
            model.Correo = result.usuario.Correo;
            model.Activo = result.usuario.Activo;
            model.Perfil = result.usuario.IdPerfil;
            model.PerfilNombre = result.usuario.PerfilNombre;
            model.Menus = result.usuario.Menus;
            return PartialView("_VerUsuario", model);
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
        public async Task<JsonResult> CrearUsuario(CrearUsuariosModel usuario)
        {
            ActionResponse = new ActionResponse();
            if (ModelState.IsValid)
            {
                if(usuario.Menus != null)
                {
                    var UrlInfo = Url.ActionContext.HttpContext.Request;
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdEntidad = Convert.ToInt32(ViewData["IdEntidad"]);
                    parameterVM.IdPerfil = Convert.ToInt32(usuario.Perfil);
                    parameterVM.Correo = usuario.Correo;
                    parameterVM.Menus = usuario.Menus.ToList();
                    parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(usuario.Contrasenia);
                    parameterVM.RequiereConfirmacion = true;
                    parameterVM.IdUsuarioCrea = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.UrlConfirmacion = string.Format("{0}://{1}/{2}", UrlInfo.Scheme, UrlInfo.Host, "Account/ConfirmarCorreo");
                    parameterVM.ImagenGrupoTrans=$"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}";
                    var result = await _serviceUsuario.CrearUsuarioSecundario(parameterVM);
                    if (result.CodigoResultado > 0)
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "El usuario ha sido creado correctamente y se envió sus datos de acceso al correo.";
                    }
                    else
                    {
                        ActionResponse.Codigo = result.CodigoResultado;
                        ActionResponse.Mensaje = "Error al crear al usuario.";
                    }

                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe Seleccionar accesos para el usuario.";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo crear al usuario.";
            }

             return Json(ActionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> ActualizarUsuario(EditarUsuarioModel usuario)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                if (usuario.Menus != null)
                {
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdUsuario = usuario.IdUsuario;
                    parameterVM.IdPerfil = usuario.Perfil;
                    parameterVM.Activo = usuario.Activo;
                    parameterVM.IdUsuarioModifica = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.Menus = usuario.Menus.ToList();
                    var result = await _serviceUsuario.EditarUsuarioSecundario(parameterVM);
                    if (result.CodigoResultado == 0)
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "El usuario ha sido actualizado correctamente.";
                    }
                    else
                    {
                        ActionResponse.Codigo = result.CodigoResultado;
                        ActionResponse.Mensaje = "Error al crear al usuario";
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
                ActionResponse.Mensaje = "Error en la validación de datos.";
            }

            return Json(ActionResponse);

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
                ActionResponse.Mensaje = "Se actualizo su contraseña correctamente y se envió a su correo.";
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo procesar la operación.";
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
        public async Task<IActionResult> ListarUsuarios()
        {
            Models.ListarUsuariosModel model = new Models.ListarUsuariosModel();

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.IdEntidad = Convert.ToInt32(ViewData["Identidad"]);
            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameterPerfil);
 
            ViewBag.ListarEmpresas = new SelectList(resultPerfiles.Perfiles, "IdPerfil", "Nombre");

            ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
            listarUsuarioParameterVM.IdEntidad = Convert.ToInt32(ViewData["IdEntidad"]);
            listarUsuarioParameterVM.RegistroInicio = 1;
            listarUsuarioParameterVM.RegistroFin = 100;
            listarUsuarioParameterVM.isActivo = -1;
            listarUsuarioParameterVM.IdPerdil = -1;

            var result = await _serviceUsuario.ObtenerListadoUsuariosSecundarios(listarUsuarioParameterVM);
            model.ListUsuarios = result;

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ListarUsuarios(ListarUsuariosModel model)
        {

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.IdEntidad = Convert.ToInt32(ViewData["Identidad"]);
            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameterPerfil);

            ViewBag.ListarEmpresas = new SelectList(resultPerfiles.Perfiles, "IdPerfil", "Nombre");


            ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
            listarUsuarioParameterVM.IdEntidad = Convert.ToInt32(ViewData["IdEntidad"]);
            listarUsuarioParameterVM.Correo = model.Correo;
            listarUsuarioParameterVM.RegistroInicio = 1;
            listarUsuarioParameterVM.RegistroFin = 100;
            listarUsuarioParameterVM.IdPerdil = model.IdPerfil;
            listarUsuarioParameterVM.isActivo = model.isActivo;

            var result = await _serviceUsuario.ObtenerListadoUsuariosSecundarios(listarUsuarioParameterVM);
            model.ListUsuarios = result;

            return View(model);

        }

    }
}
