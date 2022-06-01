using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceConsumer;
using ViewModel.Datos.Perfil;
using Web.Principal.Util;
using Web.Principal.Areas.GestionarAutorizacion.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using Security.Common;

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
        public async Task<IActionResult> ListarPerfiles(string parkey)
        {
            ListarPerfilesModel model = new ListarPerfilesModel();

            if (parkey != null)
            {
                var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);

                string[] parametros = dataDesencriptada.Split('|');

                if (parametros.Count() > 1)
                {
                    string Nombre = parametros[0];
                    string Activo = parametros[1];
                    string Tipo = parametros[2];
                
                    //   string url = $"{model.Nombre}|{model.Activo}|{model.Tipo}";

                    model.Nombre = Nombre;
                    model.Activo = Activo==""?0:Convert.ToInt32(Activo);
                    model.Tipo = Tipo ;
              


                }
            }



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


        [HttpPost]
        public async Task<JsonResult> ListarEncriptar(ListarPerfilesModel model)
        {
            ActionResponse = new ActionResponse();

            try
            {

                string url = $"{model.Nombre}|{model.Activo}|{model.Tipo}";
                //Nombre=&Activo=1&Tipo=TP01
                string urlEncriptado = this.GetUriHost() + Url.Action("ListarPerfiles", "Perfil", new { area = "GestionarAutorizacion" }) + "?parkey=" + Encriptador.Instance.EncriptarTexto(url);

                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = urlEncriptado;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarEncriptar");
                ActionResponse.Codigo = -100;
                ActionResponse.Mensaje = "Error inesperado, por favor volver a intentar mas tarde.";
            }
            return Json(ActionResponse);
        }



        [HttpGet]
        public async Task<IActionResult> EditarPerfil(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            int Id = Int32.Parse(dataDesencriptada);

            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = Id;
            var resultPerfil = await _serviceAcceso.ObtenerPerfil(parameter);
            ViewBag.IdPerfil = Id;

            var listServiceTipoPerfil = await _serviceMaestro.ObtenerParametroPorIdPadre(41);



            ViewBag.ListTipo = new SelectList(listServiceTipoPerfil.ListaParametros, "ValorCodigo", "NombreDescripcion");

            resultPerfil.perfil.IdPerfil = Id;
            resultPerfil.perfil.Menus.ForEach(x => {
                x.VistaMenu = resultPerfil.perfil.VistaMenu.Where(z=>z.IdMenu==x.IdMenu).ToArray();
                
            }); 
            
            

            return View(resultPerfil.perfil);

        }

        [HttpGet]
        public async Task<IActionResult> VerPerfil(string parkey)
        {
            var dataDesencriptada = Encriptador.Instance.DesencriptarTexto(parkey);
            int Id =Int32.Parse( dataDesencriptada);

            PerfilParameterVM parameter = new PerfilParameterVM();
            parameter.IdPerfil = Id;

            var resultPerfil = await _serviceAcceso.ObtenerPerfil(parameter);

            resultPerfil.perfil.IdPerfil = Id;
            resultPerfil.perfil.Menus.ForEach(x => {
                x.VistaMenu = resultPerfil.perfil.VistaMenu.Where(z => z.IdMenu == x.IdMenu).ToArray();

            });


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

            result.Menus.ForEach(x => {
                x.VistaMenu = result.VistaMenu.Where(z => z.IdMenu == x.IdMenu).ToArray();

            });

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

                    List<VistaMenuVM> vistaMenus = new List<VistaMenuVM>();


                    perfil.Perfiles.ForEach(x => {
                        x.Menus.ForEach(z =>
                        {
                            if (z.VistaMenu != null)
                            {
                                z.VistaMenu.ToList().ForEach(y =>
                                {

                                    if (y.IdVistaChecked != null)
                                    {
                                        y.IdMenu = z.IdMenu;
                                        y.IdPerfil = perfil.IdPerfil;
                                        vistaMenus.Add(y);
                                    }

                                });
                            }

                        });
                    });
                    parameterVM.VistasMenu = vistaMenus;

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

                    List<VistaMenuVM> vistaMenus = new List<VistaMenuVM>();


                    perfil.Perfiles.ForEach(x=> {
                        x.Menus.ForEach(z =>
                        {
                            if (z.VistaMenu != null)
                            {
                                z.VistaMenu.ToList().ForEach(y =>
                                {

                                    if (y.IdVistaChecked != null)
                                    {
                                        y.IdMenu =z.IdMenu;
                                        y.IdPerfil = perfil.IdPerfil;
                                        vistaMenus.Add(y);
                                    }

                                });
                            }

                        });
                    });
                    parameterVM.VistasMenu = vistaMenus;

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
