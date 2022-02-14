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
using TransMares.Core;
using ViewModel.Datos.Message;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.Areas.GestionarUsuarios.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Areas.GestionarUsuarios.Controllers
{
    [Area("GestionarUsuarios")]
    public class ClienteController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioMessage _servicioMessage;
        private readonly ServicioSolicitud _serviceSolicitud;

        private static ILogger _logger = ApplicationLogging.CreateLogger("ClienteController");

        public ClienteController(
            ServicioAcceso serviceAcceso,
            ServicioUsuario serviceUsuario,
            IMapper mapper,
            IConfiguration configuration,
            ServicioMaestro serviceMaestro,
             ServicioMessage servicioMessage,
             ServicioSolicitud serviceSolicitud)
        {
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
            _configuration = configuration;
            _serviceMaestro = serviceMaestro;
            _servicioMessage = servicioMessage;
            _serviceSolicitud = serviceSolicitud;
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.Activo = 1;
            var result = await _serviceAcceso.ObtenerPerfiles(parameter);
            ViewBag.Perfiles = result.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.INTERNO));

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

            if (result.usuario.IdEntidad == 0)
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.INTERNO));
            }
            else
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.EXTERNO));
            }

            EditarClienteModel model = new EditarClienteModel();
            model.Correo = result.usuario.Correo;
            model.Nombres = result.usuario.Nombres;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.Activo = result.usuario.Activo;
            model.EsAdmin = result.usuario.EsAdmin;
            model.Perfil = result.usuario.IdPerfil;
            model.Items = result.usuario.Menus;
            model.IdEntidad = result.usuario.IdEntidad;
            model.IdUsuario = result.usuario.IdUsuario;
            model.UsuarioModifica = result.usuario.UsuarioModifica;
            model.UsuarioCrea = result.usuario.UsuarioCrea;
            model.FechaCrea = result.usuario.FechaRegistro;
            model.FechaModifica = result.usuario.FechaModificacion;
            model.CambioContrasenia = result.usuario.CambioContrasenia;
            model.ConfirmarCuenta = result.usuario.CorreoConfirmado;

            model.Usuario = await _serviceAcceso.ObtenerUsuarioPorId(Id);


            model.Grupos = new List<GruposAutorizacion>();

            var grupos = model.Usuario.MenusUserSecundario.Select(x => x.Grupo).Distinct().ToList();
            for (int ii = 0; ii < grupos.Count(); ii++)
            {

                GruposAutorizacion grupo = new GruposAutorizacion();
                grupo.Nombre = grupos[ii];


                grupo.Menus = new List<MenuAutoricacion>();

       

                var itemMenu = model.Usuario.MenusUserSecundario.Where(y => y.Grupo.Equals(grupos[ii])).Select(x => x.Nombre).Distinct().ToList();


                for(int jj = 0; jj < itemMenu.Count(); jj++) {


                    MenuAutoricacion menu = new MenuAutoricacion();
                    menu.Nombre = itemMenu[jj];
                    menu.Perfiles = new List<PerfilAutorizacion>();

                    for (int i = 0; i < model.Usuario.Perfiles.Count(); i++){

                        var resultMenu = model.Usuario.MenusUserSecundario.Where(x => x.Grupo == grupos[ii]
                                                                                               && x.IdPerfil == model.Usuario.Perfiles[i].IdPerfil
                                                                                                   && x.Nombre.Equals(itemMenu[jj])
                                                                                               ).ToList();
                     

                        menu.IdMenu = resultMenu[0].IdMenu;

                        PerfilAutorizacion perfil = new PerfilAutorizacion();
                        perfil.IdPerfil = model.Usuario.Perfiles[i].IdPerfil;
                        perfil.Nombre = model.Usuario.Perfiles[i].Nombre;
                        perfil.Checked = resultMenu[0].Permiso;

                        menu.Perfiles.Add(perfil);


                    }


                        grupo.Menus.Add(menu);


                }



                model.Grupos.Add(grupo);



            }

                /*
                   
                         

                                  


                                    @for (int jj =0;jj < itemMenu.Count(); jj++)
                                    {
                                        <tr>
                                            <td>
                                                <label class="">
                                                    @itemMenu[jj]
                                                </label>
                                            </td>


                                            @for (int i = 0; i < Model.Perfiles.Count(); i++)
                                            {

                                             <td class="text-center">

                                            @{ var resultMenu = Model.MenusUserSecundario.Where(x => x.Grupo == grupos[ii]
                                                                                               && x.IdPerfil == Model.Perfiles[i].IdPerfil
                                                                                                   && x.Nombre.Equals(itemMenu[jj])
                                                                                               ).ToList(); }

                                            @if (resultMenu.Count() > 0)
                                            {

                                                if (Model.EsAdmin == 0)
                                                {

                                                    if (resultMenu[0].Permiso)
                                                    {
                                                        <input class="form-check-input border-primary" type="checkbox" name="Grupos[@ii].Menus[@jj].Perfiles[@i].Checked" checked value="true" @Disabled>
                                                    }
                                                    else
                                                    {
                                                        <input class="form-check-input border-primary" type="checkbox" name="Grupos[@ii].Menus[@jj].Perfiles[@i].Checked" value="false" @Disabled>
                                                    }
                                                }
                                                else
                                                {
                                                    <input class="form-check-input border-primary" type="checkbox" name="Grupos[@ii].Menus[@jj].Perfiles[@i].Checked" checked value="true" @Disabled>
                                                }

                                                <input type="hidden" name="Grupos[@ii].Nombre" value="@grupos[ii]" />
                                                <input type="hidden" name="Grupos[@ii].Menus[@jj].IdMenu" value="@resultMenu[0].IdMenu" />
                                                <input type="hidden" name="Grupos[@ii].Menus[@jj].Nombre" value="@resultMenu[0].Nombre" />
                                                <input type="hidden" name="Grupos[@ii].Menus[@jj].Perfiles[@i].IdPerfil" value="@Model.Perfiles[i].IdPerfil" />
                                                <input type="hidden" name="Grupos[@ii].Menus[@jj].Perfiles[@i].Nombre" value="@Model.Perfiles[i].Nombre" />

                                            }
                                        </td>
                                            }

                                        </tr>

                                    }

                                

                 */


                return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> VerUsuario(int Id)
        {
            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.IdUsuario = Id;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);

            PerfilParameterVM parameterPerfil = new PerfilParameterVM();
            parameterPerfil.Activo = 1;

            var resultPerfiles = await _serviceAcceso.ObtenerPerfiles(parameterPerfil);

            ViewBag.Perfiles = resultPerfiles.Perfiles;

            if (result.usuario.IdEntidad == 0)
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.INTERNO));
            }
            else
            {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.EXTERNO));
            }

            EditarClienteModel model = new EditarClienteModel();
            model.Correo = result.usuario.Correo;
            model.Nombres = result.usuario.Nombres;
            model.ApellidoMaterno = result.usuario.ApellidoMaterno;
            model.ApellidoPaterno = result.usuario.ApellidoPaterno;
            model.Activo = result.usuario.Activo;
            model.EsAdmin = result.usuario.EsAdmin;
            model.Perfil = result.usuario.IdPerfil;
            model.Items = result.usuario.Menus;
            model.IdEntidad = result.usuario.IdEntidad;
            model.IdUsuario = result.usuario.IdUsuario;
            model.UsuarioModifica = result.usuario.UsuarioModifica;
            model.UsuarioCrea = result.usuario.UsuarioCrea;
            model.FechaCrea = result.usuario.FechaRegistro;
            model.FechaModifica = result.usuario.FechaModificacion;
            model.CambioContrasenia = result.usuario.CambioContrasenia;
            model.ConfirmarCuenta = result.usuario.CorreoConfirmado;

            model.Usuario = await _serviceAcceso.ObtenerUsuarioPorId(Id);

            model.Grupos = new List<GruposAutorizacion>();

            var grupos = model.Usuario.MenusUserSecundario.Select(x => x.Grupo).Distinct().ToList();
            for (int ii = 0; ii < grupos.Count(); ii++)
            {

                GruposAutorizacion grupo = new GruposAutorizacion();
                grupo.Nombre = grupos[ii];


                grupo.Menus = new List<MenuAutoricacion>();



                var itemMenu = model.Usuario.MenusUserSecundario.Where(y => y.Grupo.Equals(grupos[ii])).Select(x => x.Nombre).Distinct().ToList();


                for (int jj = 0; jj < itemMenu.Count(); jj++)
                {


                    MenuAutoricacion menu = new MenuAutoricacion();
                    menu.Nombre = itemMenu[jj];
                    menu.Perfiles = new List<PerfilAutorizacion>();

                    for (int i = 0; i < model.Usuario.Perfiles.Count(); i++)
                    {

                        var resultMenu = model.Usuario.MenusUserSecundario.Where(x => x.Grupo == grupos[ii]
                                                                                               && x.IdPerfil == model.Usuario.Perfiles[i].IdPerfil
                                                                                                   && x.Nombre.Equals(itemMenu[jj])
                                                                                               ).ToList();


                        menu.IdMenu = resultMenu[0].IdMenu;

                        PerfilAutorizacion perfil = new PerfilAutorizacion();
                        perfil.IdPerfil = model.Usuario.Perfiles[i].IdPerfil;
                        perfil.Nombre = model.Usuario.Perfiles[i].Nombre;
                        perfil.Checked = resultMenu[0].Permiso;

                        menu.Perfiles.Add(perfil);


                    }


                    grupo.Menus.Add(menu);


                }



                model.Grupos.Add(grupo);



            }

            return View(model);
        }

      
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            Models.ClienteModel model = new Models.ClienteModel();
            ListarClienteParameterVM listarClienteParameter = new ListarClienteParameterVM();
            listarClienteParameter.isActivo =null;
            listarClienteParameter.IdPerfil =null;
            listarClienteParameter.RazonSocialRepresentanteLegal =null;
            listarClienteParameter.TipoDocumento = null;
            listarClienteParameter.NumeroDocumento = null;
         

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            var listServiceTipoDocumento = await _serviceMaestro.ObtenerParametroPorIdPadre(38);

            model.ListarEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            model.ListarTipoDocumento = new SelectList(listServiceTipoDocumento.ListaParametros, "ValorCodigo", "NombreDescripcion");

            ListarPerfilActivosParameterVM parameter = new ListarPerfilActivosParameterVM();
            parameter.Tipo = Utilitario.Constante.EmbarqueConstante.TipoPerfil.EXTERNO;

            var listPerfiles= await _serviceAcceso.ObtenerPerfilesActivos(parameter);
            model.ListarPerfiles = new SelectList(listPerfiles.Perfiles, "IdPerfil", "Nombre");


            model.ListarClientes = await _serviceUsuario.ListarClientes(listarClienteParameter);
            



            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(Int64 id)
        {

            ClienteDetalleModel model = new ClienteDetalleModel();
            model.Solicitud = new ViewModel.Datos.Solicitud.SolicitudVM();


             var resultEntidad  = await _serviceUsuario.LeerCliente(id);
            model.Entidad = resultEntidad.Cliente;
            model.Solicitud = await _serviceSolicitud.leerSolicitud(model.Entidad.IdSolicitud);


            model.CodigoSolicitud = model.Solicitud.CodigoSolicitud;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Listar(ClienteModel model)
        {
            ListarClienteParameterVM listarClienteParameter = new ListarClienteParameterVM();
            if (model.isActivo < 0)
            {
                listarClienteParameter.isActivo = null;
            }
            else if (model.isActivo == 0)
            {
                listarClienteParameter.isActivo = false;
            }
            else {
                listarClienteParameter.isActivo = true;
            }

             
            listarClienteParameter.IdPerfil = model.IdPerfil;
            listarClienteParameter.RazonSocialRepresentanteLegal = model.RazonSocuialRepresentanteLegal;
            listarClienteParameter.TipoDocumento = model.IdTipoDocumento;
            listarClienteParameter.NumeroDocumento = model.NumeroDocumento;

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            var listServiceTipoDocumento = await _serviceMaestro.ObtenerParametroPorIdPadre(38);

            model.ListarEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            model.ListarTipoDocumento = new SelectList(listServiceTipoDocumento.ListaParametros, "ValorCodigo", "NombreDescripcion");

            ListarPerfilActivosParameterVM parameter = new ListarPerfilActivosParameterVM();
            parameter.Tipo = Utilitario.Constante.EmbarqueConstante.TipoPerfil.EXTERNO;

            var listPerfiles = await _serviceAcceso.ObtenerPerfilesActivos(parameter);
            model.ListarPerfiles = new SelectList(listPerfiles.Perfiles, "IdPerfil", "Nombre");


            model.ListarClientes = await _serviceUsuario.ListarClientes(listarClienteParameter);



            return View(model);
        }





        private async Task  cargarListas(string tipo)
        { 
            ListarPerfilActivosParameterVM parameter = new ListarPerfilActivosParameterVM();
            parameter.Tipo = tipo;
            var resultPerfiles = await _serviceAcceso.ObtenerPerfilesActivos(parameter);

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
                    parameterVM.EsAdmin = false;
                    parameterVM.Activo = true;
                    parameterVM.IdUsuarioCrea = Convert.ToInt32(ViewData["IdUsuario"]);
                    parameterVM.Menus = usuario.Menus.ToList();
                    parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(usuario.Contrasenia);
                    parameterVM.ContraseniaNoCifrado = usuario.Contrasenia;
                    parameterVM.RequiereConfirmacion = true;
                    parameterVM.UrlConfirmacion = string.Format("{0}/{1}", this.GetUriHost(), "Account/ConfirmarCorreo");
                    parameterVM.ImagenGrupoTrans = $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}"; ;
                    
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
        public async Task<JsonResult> ActualizarUsuario(EditarClienteModel usuario)
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
                    parameterVM.IdUsuarioModifica = this.usuario.idUsuario;
                    parameterVM.Menus = usuario.Menus.ToList();

                    var result = await _serviceUsuario.EditarUsuarioInterno(parameterVM);
                    ActionResponse.Codigo = result.CodigoResultado;
                    ActionResponse.Mensaje = result.MensajeResultado ;

                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Estimado usuario, debe Seleccionar al menos un acceso.";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario, ocurrio un error en la validación de los datos.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> DesactivarUsuario(string IdUsuario)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
                CrearUsuarioSecundarioParameterVM crearUsuarioSecundarioParameter = new CrearUsuarioSecundarioParameterVM();
                crearUsuarioSecundarioParameter.IdUsuario = Convert.ToInt32(IdUsuario);
                crearUsuarioSecundarioParameter.Activo = false;
                var result = await _serviceUsuario.HabilitarUsuario(crearUsuarioSecundarioParameter);

                actionResponse.Codigo = result.CodigoResultado;
                actionResponse.Mensaje = result.MensajeResultado;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "DesactivarUsuario");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Estimado usuario, error inesperado por favor volver a intentar más tarde.";
            }

            return Json(actionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> ActivarUsuario(string IdUsuario)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
                CrearUsuarioSecundarioParameterVM crearUsuarioSecundarioParameter = new CrearUsuarioSecundarioParameterVM();
                crearUsuarioSecundarioParameter.IdUsuario = Convert.ToInt32(IdUsuario);
                crearUsuarioSecundarioParameter.Activo = true;
                var result = await _serviceUsuario.HabilitarUsuario(crearUsuarioSecundarioParameter);

                actionResponse.Codigo = result.CodigoResultado;
                actionResponse.Mensaje = result.MensajeResultado;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "DesactivarUsuario");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Estimado usuario, error inesperado por favor volver a intentar más tarde.";
            }

            return Json(actionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> RestablercContrasenia(string IdUsuario)
        {
            ActionResponse actionResponse = new ActionResponse();

            try
            {
              var resultUsuario=await  _serviceUsuario.ObtenerUsuario(Convert.ToInt32(IdUsuario));

                CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                parameterVM.IdUsuario = Convert.ToInt32(IdUsuario);
                parameterVM.Correo = resultUsuario.Usuario.Correo;

                string strContrasenia = new Utilitario.Seguridad.SeguridadCodigo().GenerarCadenaLongitud(6);
                strContrasenia = strContrasenia.ToUpper();
                string strNuevaContrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(strContrasenia);
                parameterVM.Contrasenia = strNuevaContrasenia;
                var result = await _serviceUsuario.CambiarClaveUsuario(parameterVM);

                actionResponse.Codigo = result.CodigoResultado;
                actionResponse.Mensaje = result.MensajeResultado;

                if (actionResponse.Codigo == 0) {
                    enviarCorreo(new FormatoCorreoBody().formatoBodyRestablecerContrasenia(resultUsuario.Usuario.Nombres,
                     strContrasenia,
                     $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}"),
                     resultUsuario.Usuario.Correo,
                     "!Transmares Group! Restablecer Contraseña"
                     
                     );
                }

            }
            catch (Exception err)
            {
                _logger.LogError(err, "RestablercContrasenia");
                actionResponse.Codigo = -100;
                actionResponse.Mensaje = "Estimado usuario, error inesperado por favor volver a intentar más tarde.";
            }

            return Json(actionResponse);

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

        private async void enviarCorreo(string _contenido, string _correo, string _asunto) {

            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
            await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
        }


        [HttpGet]
        public async Task<IActionResult> ListarUsuarios(Int64 id)
        {
            Models.ListarUsuariosModel model = new Models.ListarUsuariosModel();

            try
            {
                ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
                listarUsuarioParameterVM.ApellidoMaterno = "";
                listarUsuarioParameterVM.ApellidoPaterno = "";
                listarUsuarioParameterVM.Nombres = "";
                listarUsuarioParameterVM.Correo = "";
                listarUsuarioParameterVM.IsAdmin = -1;
                listarUsuarioParameterVM.isActivo = -1;
                listarUsuarioParameterVM.IdEntidad =id;
                listarUsuarioParameterVM.RegistroFin = 500;
                listarUsuarioParameterVM.RegistroInicio = 0;

                var result = await _serviceUsuario.ListarClienteUsuarios(listarUsuarioParameterVM);
                model.ListUsuarios = result;
                model.IdEntidad = id;
                model.TipoUsuario = -1;
                model.isActivo = -1;
                model.IdPerfil = 0;

             ListarPerfilActivosParameterVM parameter = new ListarPerfilActivosParameterVM();
                parameter.Tipo = Utilitario.Constante.EmbarqueConstante.TipoPerfil.EXTERNO;
                var resultPerfiles = await _serviceAcceso.ObtenerPerfilesActivos(parameter);
                model.Perfiles = new SelectList(resultPerfiles.Perfiles, "IdPerfil", "Nombre");

                var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
                model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en carga de vista liberación de carga");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return PartialView("_UsuariosEntidad", model);
        }


        [HttpPost]
        public async Task<IActionResult> FiltrarUsuarios(ListarUsuariosModel model)
        {
          
            try
            {


                ListarUsuarioParameterVM listarUsuarioParameterVM = new ListarUsuarioParameterVM();
                listarUsuarioParameterVM.ApellidoMaterno = model.ApellidoMaterno;
                listarUsuarioParameterVM.ApellidoPaterno = model.ApellidoPaterno;
                listarUsuarioParameterVM.Nombres = model.Nombres;
                listarUsuarioParameterVM.Correo = model.Correo;
                listarUsuarioParameterVM.isActivo =model.isActivo;
                listarUsuarioParameterVM.IsAdmin = model.TipoUsuario;
                listarUsuarioParameterVM.IdEntidad = model.IdEntidad;
                listarUsuarioParameterVM.RegistroFin = 500;
                listarUsuarioParameterVM.RegistroInicio = 0;
                var result = await _serviceUsuario.ListarClienteUsuarios(listarUsuarioParameterVM);
           
                model.ListUsuarios = result;

            


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en carga de vista liberación de carga");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error inesperado, por favor volver a intentar mas tarde";
                model.StatusResponse = "-100";
            }

            return PartialView("_ResultadoFiltro", model.ListUsuarios.Usuarios);
        }

    }
}
