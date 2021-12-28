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

namespace Web.Principal.Areas.GestionarAutorizacion.Controllers
{
    [Area("GestionarAutorizacion")]
    public class PerfilController : BaseController
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;

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
            parameter.Nombre = model.Nombre;
            parameter.Activo = model.Activo;
            var result = await _serviceAcceso.ObtenerPerfiles(parameter);

            model.Perfiles = result.Perfiles;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil(int Id)
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = Id;
            var resultPerfil = await _serviceAcceso.ObtenerPerfil(parameter);
            ViewBag.IdPerfil = Id;

            return View(resultPerfil.perfil);

        }

        [HttpGet]
        public async Task<IActionResult> VerPerfil(int Id)
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = Id;
            var resultPerfil = await _serviceAcceso.ObtenerPerfil(parameter);

            return PartialView("_VerPerfil", resultPerfil.perfil);

        }

        [HttpGet]
        public async Task<IActionResult> CrearPerfil()
        {
            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = 0;
            var result = await _serviceAcceso.ObtenerMenus();

            ViewBag.Menus = result.Menus;

            return View();

        }

        public async Task<JsonResult> CrearPerfil(PerfilModel perfil)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                if (perfil.Menus != null && perfil.Menus.Length>0)
                {
                    PerfilParameterVM parameterVM = new();
                    parameterVM.Nombre = perfil.Nombre;
                    parameterVM.Menus = perfil.Menus.ToList();

                    var result = await _serviceAcceso.CrearPerfil(parameterVM);

                    ActionResponse.Codigo = result.CodigoResultado;
                    ActionResponse.Mensaje = result.MensajeResultado;
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe Seleccionar accesos para el perfil.";
                }

            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo crear el registro.";
            }

            return Json(ActionResponse);
        }

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
                    var result = await _serviceAcceso.EditarPerfil(parameterVM);
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = "El perfil ha sido actualizado correctamente.";
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "Debe Seleccionar accesos para el perfil.";
                }

            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo actualizar el registro.";
            }

            return Json(ActionResponse);

        }
        [HttpPost]
        public async Task<JsonResult> EliminarPerfil(int IdPerfil)
        {
            ActionResponse = new ActionResponse();

            PerfilParameterVM parameterVM = new();
            parameterVM.IdPerfil = IdPerfil;
            var result = await _serviceAcceso.EliminarPerfil(parameterVM);
            ActionResponse.Codigo = result.CodigoResultado;
            ActionResponse.Mensaje = result.MensajeResultado;

            return Json(ActionResponse);

        }




    }
}
