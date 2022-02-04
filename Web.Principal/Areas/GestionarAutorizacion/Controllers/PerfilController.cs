using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceConsumer;
using ViewModel.Datos.Perfil;
using Web.Principal.Utils;
using Web.Principal.Areas.GestionarAutorizacion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;

namespace Web.Principal.Areas.GestionarAutorizacion.Controllers
{
    [Area("GestionarAutorizacion")]
    public class PerfilController : BaseController
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("PerfilController");

        public PerfilController(
            ServicioMaestro serviceMaestro,
            ServicioAcceso serviceAcceso,
            ServicioUsuario serviceUsuario,
            IMapper mapper)
        {
            _serviceMaestro = serviceMaestro;
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ListarPerfiles(ListarPerfilesModel model)
        {
            PerfilParameterVM parameter = new();
            if (model != null)
            {
                parameter.Nombre = model.Nombre;
                parameter.Activo = model.Activo;
                parameter.Tipo = model.Tipo;
            }
            else {
                parameter.Nombre = "";
                parameter.Activo = -1;
                parameter.Tipo = "0";
            }

            var resultPerfiles = await _serviceAcceso.ObtenerPerfiles(parameter);

            model.Perfiles = resultPerfiles.Perfiles;

            var listServiceEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(76);
            var listServiceTipoPerfil = await _serviceMaestro.ObtenerParametroPorIdPadre(41);


            model.ListEstado = new SelectList(listServiceEstado.ListaParametros, "ValorCodigo", "NombreDescripcion");
            model.ListTipo = new SelectList(listServiceTipoPerfil.ListaParametros, "ValorCodigo", "NombreDescripcion");


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil(int Id)
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = Id;
            var resultPerfil = await _serviceAcceso.ObtenerPerfil(parameter);
            ViewBag.IdPerfil = Id;

            var listServiceTipoPerfil = await _serviceMaestro.ObtenerParametroPorIdPadre(41);



            ViewBag.ListTipo = new SelectList(listServiceTipoPerfil.ListaParametros, "ValorCodigo", "NombreDescripcion");



            return View(resultPerfil.perfil);

        }

        [HttpGet]
        public async Task<IActionResult> VerPerfil(int Id)
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = Id;

            var resultPerfil = await _serviceAcceso.ObtenerPerfil(parameter);

     


            return View(resultPerfil.perfil);

        }

        [HttpGet]
        public async Task<IActionResult> CrearPerfil()
        {
            PerfilModel model = new PerfilModel();

            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = 0;
            
            var result = await _serviceAcceso.ObtenerMenus();
            var listServiceTipoPerfil = await _serviceMaestro.ObtenerParametroPorIdPadre(41);

            ViewBag.ListTipo = new SelectList(listServiceTipoPerfil.ListaParametros, "ValorCodigo", "NombreDescripcion");
            ViewBag.Menus = result.Menus;


            model.UsuarioCrea = this.usuario.NombresUsuario + " " + this.usuario.ApellidoPaternousuario; ;

            return View(model);

        }

        public async Task<JsonResult> CrearPerfil(PerfilModel perfil)
        {
            ActionResponse = new ActionResponse();
            var listServiceTipoPerfil = await _serviceMaestro.ObtenerParametroPorIdPadre(41);
            ViewBag.ListTipo = new SelectList(listServiceTipoPerfil.ListaParametros, "ValorCodigo", "NombreDescripcion");

            if (ModelState.IsValid)
            {
                if (perfil.Menus != null && perfil.Menus.Length > 0)
                {
                    PerfilParameterVM parameterVM = new();
                    parameterVM.Nombre = perfil.Nombre;
                    parameterVM.Menus = perfil.Menus.ToList();
                    parameterVM.Tipo = perfil.Tipo;
                    parameterVM.IdUsuarioCrea = this.usuario.idUsuario;

                    var result = await _serviceAcceso.CrearPerfil(parameterVM);

                    ActionResponse.Codigo = result.CodigoResultado;
                    ActionResponse.Mensaje = result.MensajeResultado;
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Estimado usuario, debe seleccionar al menos un acceso.";
                }

            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario, ingresar en los campos obligatorios.";
            }

            return Json(ActionResponse);
        }

        [HttpPost]
        public async Task<JsonResult> EditarPerfil(PerfilModel perfil)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                if (perfil.Menus != null && perfil.Menus.Length > 0)
                {
                    PerfilParameterVM parameterVM = new();
                    parameterVM.IdPerfil = perfil.IdPerfil;
                    parameterVM.Activo = (perfil.Activo) ? 1 : 0;
                    parameterVM.Nombre = perfil.Nombre;
                    parameterVM.Menus = perfil.Menus.ToList();
                    parameterVM.IdUsuarioModifica =this.usuario.idUsuario;
                    parameterVM.Tipo =perfil.Tipo;

                    var result = await _serviceAcceso.EditarPerfil(parameterVM);
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = "Estimado usuario, el perfil ha sido actualizado correctamente.";
                }
                else
                {
                    ActionResponse.Codigo = 1;
                    ActionResponse.Mensaje = "Estimado usuario, debe seleccionar al menos un acceso.";
                }

            }
            else
            {
                ActionResponse.Codigo = 2;
                ActionResponse.Mensaje = "Estimado usuario, revisar los campos obligatorios.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> EliminarPerfil(int IdPerfil)
        {
            ActionResponse = new ActionResponse();

            PerfilParameterVM parameterVM = new PerfilParameterVM();
            parameterVM.IdPerfil = IdPerfil;
            var result = await _serviceAcceso.EliminarPerfil(parameterVM);
            ActionResponse.Codigo = result.CodigoResultado;
            ActionResponse.Mensaje = result.MensajeResultado;

            return Json(ActionResponse);

        }

        [HttpPost]
        public async Task<JsonResult> VerificarSiTieneAccesos(int IdPerfil)
        {
            ActionResponse = new ActionResponse();

            try
            {
                PerfilParameterVM parameterVM = new PerfilParameterVM();
                parameterVM.IdPerfil = IdPerfil;
                var result = await _serviceAcceso.VerificarPerfil(parameterVM);

                ActionResponse.Codigo = result.CodigoResultado;
                ActionResponse.Mensaje = result.MensajeResultado;

            }
            catch (Exception err) {

                _logger.LogError(err, "VerificarSiTieneAccesos");
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Estimado usuario ocurrio un error interno inesperado, por favo r volver a intentar más tarde." ;
            }

      

            return Json(ActionResponse);

        }


    }
}
