using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Acceso;
using Web.Principal.Areas.GestionarAccesos.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Areas.GestionarAccesos.Controllers
{
    [Area("GestionarAccesos")]
    public class LoginController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;

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

        [HttpPost]
        public async Task<IActionResult> CambiarContrasenia(CambiarContraseniaModel model)
        {
            ViewBag.mensaje = "";
            ViewBag.codigo = "";
            ViewBag.EsNuevo = model.EsNuevo;


            if (ModelState.IsValid)
            {

                CambiarContrasenaParameterVM cambiarContrasenaParameterVM = new CambiarContrasenaParameterVM();
                cambiarContrasenaParameterVM.IdUsuario = usuario.idUsuario;
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
