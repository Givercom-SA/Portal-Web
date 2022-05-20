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
using ViewModel.Datos.Autorizacion;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using Security.Common;

namespace Web.Principal.Areas.GestionarUsuarios.Controllers
{
    [Area("GestionarUsuarios")]
    public class UsuarioSecundarioController : BaseController
    {
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private static ILogger _logger = ApplicationLogging.CreateLogger("UsuarioSecundarioController");
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
            CrearUsuariosModel model = new CrearUsuariosModel();

            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdEntidad = Convert.ToInt32(ViewData["IdEntidad"]);
            var result = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameter);

           
            var user = await _serviceAcceso.ObtenerUsuarioPorId(this.usuario.idUsuario);
            model.MenuPerfil = new MenuPerfilModel();
            model.MenuPerfil.Perfiles = user.Perfiles;
            model.MenuPerfil.Grupos = new List<GruposAutorizacion>();

            var grupos = user.MenusUserSecundario.Select(x => x.Grupo).Distinct().ToList();

            for (int ii = 0; ii < grupos.Count(); ii++)
            {

                GruposAutorizacion grupo = new GruposAutorizacion();
                grupo.Nombre = grupos[ii];
                grupo.Menus = new List<MenuAutoricacion>();

                var itemMenu = user.MenusUserSecundario.Where(y => y.Grupo.Equals(grupos[ii])).Select(x => x.Nombre).Distinct().ToList();


                for (int jj = 0; jj < itemMenu.Count(); jj++)
                {
                    MenuAutoricacion menu = new MenuAutoricacion();
                    menu.Nombre = itemMenu[jj];
                    menu.Perfiles = new List<PerfilAutorizacion>();

                    for (int i = 0; i < user.Perfiles.Count(); i++)
                    {
                        var resultMenu = user.MenusUserSecundario.Where(x => x.Grupo == grupos[ii]
                                                                                               && x.IdPerfil == user.Perfiles[i].IdPerfil
                                                                                                   && x.Nombre.Equals(itemMenu[jj])
                                                                                               ).ToList();
                        if (resultMenu.Count() > 0)
                        {
                            menu.IdMenu = resultMenu[0].IdMenu;
                            PerfilAutorizacion perfil = new PerfilAutorizacion();
                            perfil.IdPerfil = user.Perfiles[i].IdPerfil;
                            perfil.Nombre = user.Perfiles[i].Nombre;
                            perfil.Checked = resultMenu[0].Permiso;

                            menu.Perfiles.Add(perfil);
                        }
                    }

                    grupo.Menus.Add(menu);
                }

                model.MenuPerfil.Grupos.Add(grupo);

            }


           // ViewBag.Perfiles = result.Perfiles;

            return View(model);
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



            EditarUsuarioInternoModel model = new EditarUsuarioInternoModel();


            model.IdUsuario = Id;
            model.Correo = result.usuario.Correo;
            model.Activo = result.usuario.Activo;

            model.Nombres = result.usuario.Nombres;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;




           var user = await _serviceAcceso.ObtenerUsuarioPorId(Id);

            model.MenuPerfil = new MenuPerfilModel();
            model.MenuPerfil.Perfiles = user.Perfiles;


            model.MenuPerfil.Perfiles.Clear();
            foreach (var itemPerfil in resultPerfiles.Perfiles) {
                PerfilLoginVM perfilLoginVM = new PerfilLoginVM();
                perfilLoginVM.IdPerfil = itemPerfil.IdPerfil;
                perfilLoginVM.Nombre = itemPerfil.Nombre;
                model.MenuPerfil.Perfiles.Add(perfilLoginVM);
            }
            


            model.MenuPerfil.Grupos = new List<GruposAutorizacion>();



            var grupos = user.MenusUserSecundario.Select(x => x.Grupo).Distinct().ToList();

            for (int ii = 0; ii < grupos.Count(); ii++)
            {

                GruposAutorizacion grupo = new GruposAutorizacion();
                grupo.Nombre = grupos[ii];
                grupo.Menus = new List<MenuAutoricacion>();

                var itemMenu = user.MenusUserSecundario.Where(y => y.Grupo.Equals(grupos[ii])).Select(x => x.Nombre).Distinct().ToList();


                for (int jj = 0; jj < itemMenu.Count(); jj++)
                {
                    MenuAutoricacion menu = new MenuAutoricacion();
                    menu.Nombre = itemMenu[jj];
                    menu.Perfiles = new List<PerfilAutorizacion>();

                    for (int i = 0; i < user.Perfiles.Count(); i++)
                    {
                        var resultMenu = user.MenusUserSecundario.Where(x => x.Grupo == grupos[ii]
                                                                                               && x.IdPerfil == user.Perfiles[i].IdPerfil
                                                                                                   && x.Nombre.Equals(itemMenu[jj])
                                                                                               ).ToList();
                        if (resultMenu.Count() > 0)
                        {
                            menu.IdMenu = resultMenu[0].IdMenu;
                            PerfilAutorizacion perfil = new PerfilAutorizacion();
                            perfil.IdPerfil = user.Perfiles[i].IdPerfil;
                            perfil.Nombre = user.Perfiles[i].Nombre;
                            perfil.Checked = resultMenu[0].Permiso;

                            menu.Perfiles.Add(perfil);
                        }
                    }

                    grupo.Menus.Add(menu);
                }

                model.MenuPerfil.Grupos.Add(grupo);

            }

            ViewBag.IdUsuario = Id;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> VerUsuario(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            Int32 Id = Convert.ToInt32(dataDesencriptada);

            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;

            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.IdEntidad = Convert.ToInt32(ViewData["Identidad"]);
            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameterPerfil);



            EditarUsuarioInternoModel model = new EditarUsuarioInternoModel();


            model.IdUsuario = Id;
            model.Correo = result.usuario.Correo;
            model.Activo = result.usuario.Activo;

            model.Nombres = result.usuario.Nombres;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;
            model.ConfirmarCuenta = result.usuario.CorreoConfirmado;
            model.CambioContrasenia = result.usuario.CambioContrasenia;

            var user = await _serviceAcceso.ObtenerUsuarioPorId(Id);

            model.MenuPerfil = new MenuPerfilModel();
            model.MenuPerfil.Perfiles = user.Perfiles;


            model.MenuPerfil.Perfiles.Clear();
            foreach (var itemPerfil in resultPerfiles.Perfiles)
            {
                PerfilLoginVM perfilLoginVM = new PerfilLoginVM();
                perfilLoginVM.IdPerfil = itemPerfil.IdPerfil;
                perfilLoginVM.Nombre = itemPerfil.Nombre;
                model.MenuPerfil.Perfiles.Add(perfilLoginVM);
            }



            model.MenuPerfil.Grupos = new List<GruposAutorizacion>();



            var grupos = user.MenusUserSecundario.Select(x => x.Grupo).Distinct().ToList();

            for (int ii = 0; ii < grupos.Count(); ii++)
            {

                GruposAutorizacion grupo = new GruposAutorizacion();
                grupo.Nombre = grupos[ii];
                grupo.Menus = new List<MenuAutoricacion>();

                var itemMenu = user.MenusUserSecundario.Where(y => y.Grupo.Equals(grupos[ii])).Select(x => x.Nombre).Distinct().ToList();


                for (int jj = 0; jj < itemMenu.Count(); jj++)
                {
                    MenuAutoricacion menu = new MenuAutoricacion();
                    menu.Nombre = itemMenu[jj];
                    menu.Perfiles = new List<PerfilAutorizacion>();

                    for (int i = 0; i < user.Perfiles.Count(); i++)
                    {
                        var resultMenu = user.MenusUserSecundario.Where(x => x.Grupo == grupos[ii]
                                                                                               && x.IdPerfil == user.Perfiles[i].IdPerfil
                                                                                                   && x.Nombre.Equals(itemMenu[jj])
                                                                                               ).ToList();
                        if (resultMenu.Count() > 0)
                        {
                            menu.IdMenu = resultMenu[0].IdMenu;
                            PerfilAutorizacion perfil = new PerfilAutorizacion();
                            perfil.IdPerfil = user.Perfiles[i].IdPerfil;
                            perfil.Nombre = user.Perfiles[i].Nombre;
                            perfil.Checked = resultMenu[0].Permiso;

                            menu.Perfiles.Add(perfil);
                        }
                    }

                    grupo.Menus.Add(menu);
                }

                model.MenuPerfil.Grupos.Add(grupo);

            }

            ViewBag.IdUsuario = Id;

            return View(model);
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
                if(usuario.Grupos != null)
                {
                    var UrlInfo = Url.ActionContext.HttpContext.Request;
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdEntidad = this.usuario.IdEntidad;
                    parameterVM.IdPerfil = Convert.ToInt32(this.usuario.IdPerfil);
                    parameterVM.Correo = usuario.Correo;
                    parameterVM.Nombres = usuario.Nombres;
                    parameterVM.ApellidoPaterno = usuario.ApellidoPaterno;
                    parameterVM.ApellidoMaterno = usuario.ApellidoMaterno;
                    parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(usuario.Contrasenia);
                    parameterVM.ContraseniaNoCifrado = usuario.Contrasenia;
                    parameterVM.RequiereConfirmacion = true;
                    parameterVM.IdUsuarioCrea = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.UrlConfirmacion = string.Format("{0}/{1}", this.GetUriHost(), "Account/ConfirmarCorreo");
                    parameterVM.ImagenGrupoTrans=$"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}";

                    parameterVM.MenusPerfil = new List<MenuVM>();
                    usuario.Grupos.ForEach(x => {
                        x.Menus.ForEach(m =>
                        {
                            m.Perfiles.ForEach(p => {
                                if (p.Checked)
                                {
                                    MenuVM menuVM = new MenuVM();
                                    menuVM.IdMenu = m.IdMenu;
                                    menuVM.IdPerfil = p.IdPerfil;
                                    parameterVM.MenusPerfil.Add(menuVM);
                                }
                            });

                        });

                    });



                    var result = await _serviceUsuario.CrearUsuarioSecundario(parameterVM);
                    ActionResponse.Codigo = result.CodigoResultado;
                    ActionResponse.Mensaje = result.MensajeResultado;
                    

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
        public async Task<JsonResult> ActualizarUsuario(EditarUsuarioInternoModel usuario)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                if (usuario.Grupos != null)
                {
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdUsuario = usuario.IdUsuario;
                    parameterVM.Nombres =usuario.Nombres;
                    parameterVM.ApellidoPaterno = usuario.ApellidoPaterno;
                    parameterVM.ApellidoMaterno = usuario.ApellidoMaterno;
                    parameterVM.Activo = usuario.Activo;
                    parameterVM.IdUsuarioModifica = Convert.ToInt32(ViewData["IdUsuario"]);
                   
                    parameterVM.MenusPerfil = new List<MenuVM>();
                    usuario.Grupos.ForEach(x => {
                        x.Menus.ForEach(m =>
                        {
                            m.Perfiles.ForEach(p => {
                                if (p.Checked)
                                {
                                    MenuVM menuVM = new MenuVM();
                                    menuVM.IdMenu = m.IdMenu;
                                    menuVM.IdPerfil = p.IdPerfil;
                                    parameterVM.MenusPerfil.Add(menuVM);
                                }
                            });

                        });

                    });

                    var result = await _serviceUsuario.EditarUsuarioSecundario(parameterVM);
                    if (result.CodigoResultado == 0)
                    {
                        ActionResponse.Codigo = 0;
                        ActionResponse.Mensaje = "Estimado usuario, el usuario ha sido actualizado correctamente.";
                    }
                    else
                    {
                        ActionResponse.Codigo = result.CodigoResultado;
                        ActionResponse.Mensaje = "Estimado usuario, ocurrio un error inesperado.";
                    }
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Estimado usuario, debe seleccionar al menus un acceso.";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario, ocurrio un error en la validación de datos.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> CambiarClave(CambiarContraseniaModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                if (model.IdUsuario > 0)
                {
                    CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                    parameterVM.IdUsuario = model.IdUsuario;
                    parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(model.Contrasenia);
                    var result = await _serviceUsuario.CambiarClaveUsuario(parameterVM);
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = "Estimado usuario, se actualizo su contraseña correctamente y se envió a su correo.";
                }
                else
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = "Estimado usuario, no se identifico el ID del usuario, por favor volver a iniciar sesión.";
                }
            }
            catch (Exception err)
            {
                _logger.LogError(err, "CambiarClave");

                ActionResponse.Codigo = -2;
                ActionResponse.Mensaje = "Estimado usuario, ocurrio un error inesperado por favor volver a intentar.";
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
            model.ReturnUrl = "";

            return View(model);

        }

        //[HttpPost]
        //public async Task<IActionResult> ListarUsuarios(ListarUsuariosModel model)
        //{

        //    PerfilParameterVM parameterPerfil = new PerfilParameterVM();
        //    parameterPerfil.IdEntidad = Convert.ToInt32(ViewData["Identidad"]);
        //    var resultPerfiles = await _serviceAcceso.ObtenerPerfilesPorEntidad(parameterPerfil);

        //    ViewBag.ListarEmpresas = new SelectList(resultPerfiles.Perfiles, "IdPerfil", "Nombre");


        //    ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
        //    listarUsuarioParameterVM.IdEntidad = Convert.ToInt32(ViewData["IdEntidad"]);
        //    listarUsuarioParameterVM.Correo = model.Correo;
        //    listarUsuarioParameterVM.RegistroInicio = 1;
        //    listarUsuarioParameterVM.RegistroFin = 100;
        //    listarUsuarioParameterVM.IdPerdil = model.IdPerfil;
        //    listarUsuarioParameterVM.isActivo = model.isActivo;

        //    var result = await _serviceUsuario.ObtenerListadoUsuariosSecundarios(listarUsuarioParameterVM);
        //    model.ListUsuarios = result;

        //    return View(model);

        //}

    }
}
