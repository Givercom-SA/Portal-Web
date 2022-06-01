using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.Areas.GestionarDashboards.Models;
using Web.Principal.ServiceConsumer;
using Web.Principal.Util;

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
            HomeModel model = new HomeModel();
            if (!usuario.isCambioClave)
                return RedirectToAction("CambiarContrasenia", "Login", new { area = "GestionarAccesos" , nuevo = "1" });


            if (usuario.Dashboard == "Home")
            {
                DashboardClienteParameterVM dashboardParameterVM = new DashboardClienteParameterVM();
                
                dashboardParameterVM.IdUsuario = this.usuario.idUsuario;
                dashboardParameterVM.CodigoEmpresaGtrm =this.usuario.Sesion.CodigoTransGroupEmpresaSeleccionado;
                model.Dashboard = await _serviceUsuario.DashboardCliente(dashboardParameterVM);


                return View(model);
            }
            else
            {
                return RedirectToAction(usuario.Dashboard);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Administracion()
        {

            if (!usuario.isCambioClave)
                return RedirectToAction("CambiarContrasenia", "Login", new { area = "GestionarAccesos", nuevo = "1" });

            AdminModel model = new AdminModel();

            DashboardAdminParameterVM dashboardParameterVM = new DashboardAdminParameterVM();

            
            model.Dashboard = await _serviceUsuario.DashboardAdmin(dashboardParameterVM);
            

            return View(model);
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
            var actualSesion = HttpContext.Session.GetUserContent();

            var resultCambioPerfil = await _serviceUsuario.CambiarPerfilDefecto(new ViewModel.Datos.UsuarioRegistro.CambiarPerfilDefectoParameterVM() { IdUsuario = actualSesion.idUsuario, IdPerfil = IdPerfil });
            var sesionUsuario = await _serviceAcceso.ObtenerUsuarioPorId(actualSesion.idUsuario);

            actualSesion.MenusUserSecundario = sesionUsuario.MenusUserSecundario;
            actualSesion.Menus = sesionUsuario.Menus;

            HttpContext.Session.SetUserContent(actualSesion);
            HttpContext.Session.SetSession("IdPerfilSesion", IdPerfil);

            return RedirectToAction("Home", "Inicio", new { area = "GestionarDashboards" });

        }

        [HttpPost]
        public async Task<IActionResult> CambiarEmpresa(string CodigoEmpresa)
        {
            var resultSesion = HttpContext.Session.GetUserContent();

            var empresaSelect = resultSesion.Empresas.Empresa.Where(x => x.Codigo == CodigoEmpresa).FirstOrDefault();

            resultSesion.Sesion.CodigoTransGroupEmpresaSeleccionado = empresaSelect.Codigo;
            resultSesion.Sesion.RucTransGroupEmpresaSeleccionado = empresaSelect.Ruc;
            resultSesion.Sesion.NombreTransGroupEmpresaSeleccionado = empresaSelect.Nombres;
            resultSesion.Sesion.ImagenTransGroupEmpresaSeleccionado = empresaSelect.Imagen;
       

         
                HttpContext.Session.SetUserContent(resultSesion);


                return RedirectToAction("Home", "Inicio", new { area = "GestionarDashboards" });

        }

    }
}
