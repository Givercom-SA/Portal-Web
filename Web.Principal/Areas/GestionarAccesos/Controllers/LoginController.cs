using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Acceso;
using Web.Principal.Areas.GestionarAccesos.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Util;

namespace Web.Principal.Areas.GestionarAccesos.Controllers
{
    [Area("GestionarAccesos")]
    public class LoginController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;
        private static ILogger _logger = ApplicationLogging.CreateLogger("LoginController");
        public LoginController(ServicioAcceso serviceAcceso)
        {
            _serviceAcceso = serviceAcceso;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            CerrarSesion();
            return Redirect("~/Account/Login");
        }

        [HttpGet]
        public async Task<IActionResult> CambiarContrasenia(string nuevo)
        {
            CambiarContraseniaModel model = new CambiarContraseniaModel();

     


            if (!string.IsNullOrEmpty(nuevo))
            {



                ViewBag.mensaje = "";
                ViewBag.codigo = "";
                ViewBag.EsNuevo = nuevo;

           
                model.EsNuevo = nuevo;

            }
            else {

                
                return RedirectToAction("Home", "Inicio", new { area = "GestionarDashboards"});

            }

            return View(model);

        }


        [HttpGet]
        public async Task<JsonResult> CambiarSessionEntidad(int id, string idPerfil)
        {
        
            ActionResponse ActionResponse = new ActionResponse();

            try
            {


                var actualSesion = HttpContext.Session.GetUserContent();



                var newUserSesion = await _serviceAcceso.ObtenerUsuarioPorId(id);

                newUserSesion.Sesion = actualSesion.Sesion;
                newUserSesion.Empresas = actualSesion.Empresas;
                newUserSesion.IdUsuarioInicioSesion = actualSesion.IdUsuarioInicioSesion;

                newUserSesion.ModoAdminSistema = Utilitario.Constante.SeguridadConstante.ModoVisualizacion.ADMIN_INSPECTOR.ToString();
                newUserSesion.AdminSistema = actualSesion.AdminSistema;
                newUserSesion.Sesion.RucIngresadoUsuario = newUserSesion.NumeroDocumento;


                HttpContext.Session.SetUserContent(newUserSesion);

                HttpContext.Session.SetSession("IdPerfilSesion", idPerfil);


                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = "Exitosamente";



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CambiarSessionEntidad");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde";
            }

            return new JsonResult(ActionResponse);
        }


        [HttpGet]
        public async Task<JsonResult> CerrarSesionInspector()
        {

            ActionResponse ActionResponse = new ActionResponse();

            try
            {
                var actualSesion = HttpContext.Session.GetUserContent();
                var newUserSesion = await _serviceAcceso.ObtenerUsuarioPorId(Int32.Parse(actualSesion.IdUsuarioInicioSesion.ToString()));

                newUserSesion.Sesion = actualSesion.Sesion;

                newUserSesion.Empresas = actualSesion.Empresas;
                newUserSesion.ModoAdminSistema = Utilitario.Constante.SeguridadConstante.ModoVisualizacion.ADMINISTRADOR.ToString();
                newUserSesion.IdUsuarioInicioSesion = newUserSesion.idUsuario;

                HttpContext.Session.SetUserContent(newUserSesion);
                HttpContext.Session.SetSession("IdPerfilSesion", newUserSesion.IdPerfil);

                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = "Exitosamente";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CerrarSesionInspector");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar más tarde";
            }

            return new JsonResult(ActionResponse);
        }



        [HttpPost]
        public async Task<IActionResult> CambiarContrasenia(CambiarContraseniaModel model)
        {
            ViewBag.mensaje = "";
            ViewBag.codigo = "";
            ViewBag.EsNuevo = model.EsNuevo;


            if (ModelState.IsValid)
            {

                CambiarContrasenaParameterVM cambiarContrasenaParameterVM = new CambiarContrasenaParameterVM();
                if (usuario.AdminSistema == 1)
                {
                    cambiarContrasenaParameterVM.IdUsuario = Int32.Parse(usuario.IdUsuarioInicioSesion.ToString());
                }
                else {
                    cambiarContrasenaParameterVM.IdUsuario = usuario.idUsuario;
                }
                

                cambiarContrasenaParameterVM.ContrasenaActual = new Utilitario.Seguridad.Encrypt().GetSHA256(model.contraseniaActual);
                cambiarContrasenaParameterVM.ContrasenaNuevo = new Utilitario.Seguridad.Encrypt().GetSHA256(model.contraseniaNueva);

                if (String.IsNullOrEmpty(model.EsNuevo))
                { cambiarContrasenaParameterVM.EsUsuarioNuevo = null; }

                else if (model.EsNuevo.Equals("1")) {
                    cambiarContrasenaParameterVM.EsUsuarioNuevo = true;
                }
                else if(model.EsNuevo.Equals("0")) {
                    cambiarContrasenaParameterVM.EsUsuarioNuevo =false;
                }

                var cambiarContrasenaResultVM = await _serviceAcceso.ActualizarContrasena(cambiarContrasenaParameterVM);

                ViewBag.mensaje = cambiarContrasenaResultVM.MensajeResultado;
                ViewBag.codigo = cambiarContrasenaResultVM.CodigoResultado;

             
            }            


            return View(model);
        }

        // ======================= Eventos reutilizados =======================
        public void CerrarSesion()
        {
            //usuario = HttpContext.Session.GetUserContent();
            HttpContext.Response.Cookies.Delete("CoreSessionDemo");
            HttpContext.Session.Clear();            
        }
    }
}
