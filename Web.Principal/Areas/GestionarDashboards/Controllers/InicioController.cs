using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Areas.GestionarDashboards.Controllers
{
    [Area("GestionarDashboards")]
    public class InicioController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ServicioAcceso _serviceAcceso;

        public InicioController(
            ServicioUsuario serviceUsuario,
            ServicioAcceso serviceAcceso,
            IMapper mapper)
        {
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
        }




        [HttpGet]
        public async Task<IActionResult> Home()
        {
            if (!usuario.isCambioClave)
                return RedirectToAction("CambiarContrasenia", "Login", new { area = "GestionarAccesos" , nuevo = "1" });


            if (usuario.Dashboard == "Home")
            {
                return View();
            }
            else {
                return RedirectToAction(usuario.Dashboard);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Administracion()
        {
            if (!usuario.isCambioClave)
                return RedirectToAction("CambiarContrasenia", "Login", new { area = "GestionarAccesos", nuevo = "1" });

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> Operaciones()
        {
            if (!usuario.isCambioClave)
                return RedirectToAction("CambiarContrasenia", "Login", new { area = "GestionarAccesos", nuevo = "1" });

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CambiarPerfil(int IdPerfil)
        {
            var resultSesion = HttpContext.Session.GetUserContent();

          
            ViewModel.Datos.UsuarioRegistro.CambiarPerfilDefectoParameterVM cambiarPerfilDefecetoParameter = new ViewModel.Datos.UsuarioRegistro.CambiarPerfilDefectoParameterVM();
            cambiarPerfilDefecetoParameter.IdPerfil = IdPerfil;
            cambiarPerfilDefecetoParameter.IdUsuario= resultSesion.idUsuario;

            var result = await _serviceUsuario.CambiarPerfilDefecto(cambiarPerfilDefecetoParameter);

            var resultUsuario = await _serviceAcceso.ObtenerUsuarioPorId(resultSesion.idUsuario);
             resultUsuario.Sesion = resultSesion.Sesion ;

            HttpContext.Session.SetUserContent(resultUsuario);



            HttpContext.Session.SetSession("IdPerfilSesion", IdPerfil);
            return RedirectToAction("Home", "Inicio", new { area = "GestionarDashboards"});

        }
    }
}
