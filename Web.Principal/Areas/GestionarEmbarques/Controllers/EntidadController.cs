using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.Util;
using ViewModel.Datos.Embarque.AsignarAgente;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.ServiceConsumer;
using AutoMapper;
using Web.Principal.ServiceExterno;
using static Utilitario.Constante.EmbarqueConstante;
using Web.Principal.Areas.GestionarSolicitudes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utilitario.Constante;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using ViewModel.Datos.Entidad;
using Web.Principal.Areas.GestionarEmbarques.Models.Entidad;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class EntidadController : BaseController
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioUsuario _serviceUsuario;

        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("EntidadController");

        public EntidadController(ServicioEmbarque serviceEmbarque,
            ServicioEmbarques serviceEmbarques,
            ServicioUsuario serviceUsuario,
            ServicioMaestro serviceMaestro,
            ServicioAcceso serviceAcceso,
        IMapper mapper)
        {
            _serviceEmbarque = serviceEmbarque;
            _serviceEmbarques = serviceEmbarques;
            _serviceUsuario = serviceUsuario;
            _serviceMaestro = serviceMaestro;
            _mapper = mapper;
            _serviceAcceso = serviceAcceso;

        }
        public IActionResult Index()
        {
            return View();
        }

      
        [HttpGet]
        public async Task<IActionResult> ListarEntidadTipo()
        {
            EntidadTipoModel model = new EntidadTipoModel();

            try
            {


                ListarEntidadParameterVM listarEntidadParameterVM = new ListarEntidadParameterVM();

                var result = await _serviceEmbarque.ListarEntidadTipo(listarEntidadParameterVM);




                model.EntidadesTipo = result;




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarEntidadTipo");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error en obtener lista de entidades tipo";
                model.StatusResponse = "-100";
            }

            return PartialView("_ResultListarEntidadTipo", model);
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


    }


}
