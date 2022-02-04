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
    public class UsuarioController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioMessage _servicioMessage;
        
        private static ILogger _logger = ApplicationLogging.CreateLogger("UsuarioController");

        public UsuarioController(
            ServicioAcceso serviceAcceso,
            ServicioUsuario serviceUsuario,
            IMapper mapper,
            IConfiguration configuration,
            ServicioMaestro serviceMaestro,
             ServicioMessage servicioMessage)
        {
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
            _configuration = configuration;
            _serviceMaestro = serviceMaestro;
            _servicioMessage = servicioMessage;
        }

        [HttpGet]
        public async Task<IActionResult> CrearUsuario()
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
            else {
                ViewBag.Perfiles = resultPerfiles.Perfiles.Where(x => x.Tipo.Equals(Utilitario.Constante.SeguridadConstante.TipPerfil.EXTERNO));
            }
            
       

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
            model.IdUsuario = result.usuario.IdUsuario;

            model.UsuarioModifica = result.usuario.UsuarioModifica;
            model.UsuarioCrea = result.usuario.UsuarioCrea;
            model.FechaCrea = result.usuario.FechaRegistro;
            model.FechaModifica = result.usuario.FechaModificacion;
            model.CambioContrasenia = result.usuario.CambioContrasenia;
            model.ConfirmarCuenta = result.usuario.CorreoConfirmado;


            return View(model);
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
            await cargarListas(Utilitario.Constante.EmbarqueConstante.TipoPerfil.INTERNO);
            model.ListUsuarios = result;


   

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);

            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
 


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

            await cargarListas(Utilitario.Constante.EmbarqueConstante.TipoPerfil.INTERNO);

            return View(model);
        }


        private async Task  cargarListas(string tipo) {
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
                     strNuevaContrasenia,
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

    }
}
