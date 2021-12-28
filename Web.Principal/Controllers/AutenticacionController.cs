using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.SolictudAcceso;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.ServiceConsumer;
using Web.Principal.Utils;

namespace Web.Principal.Controllers
{


    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AutenticacionController : BaseLibreController
    {

        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ActionResponse ActionResponse { get; set; }

        public AutenticacionController(
            ServicioAcceso servicioAcceso,
            ServicioUsuario servicioUsuario,
            IMapper mapper, IConfiguration configuration)
        {
            _serviceAcceso = servicioAcceso;
            _serviceUsuario = servicioUsuario;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("habilitar-usuario")]
        public async Task<JsonResult> HabilitarUsuario(int IdUsuario, bool Activo)
        {
            ActionResponse = new ActionResponse();

            if (ModelState.IsValid)
            {
                CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                parameterVM.IdUsuario = IdUsuario;
                parameterVM.Activo = Activo;
                var result = await _serviceUsuario.HabilitarUsuario(parameterVM);
                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = "El usuario ha sido habilitado correctamente.";
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo habilitar al usuario.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        [Route("cambiar-clave")]
        public async Task<JsonResult> CambiarClave(string Correo, string Clave)
        {
            ActionResponse = new ActionResponse();
            if (Correo.Length > 0)
            {
                CrearUsuarioSecundarioParameterVM parameterVM = new CrearUsuarioSecundarioParameterVM();
                //parameterVM.IdUsuario = IdUsuario;
                parameterVM.Correo = Correo;
                parameterVM.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(Clave);
                var result = await _serviceUsuario.CambiarClaveUsuario(parameterVM);
                ActionResponse.Codigo = result.CodigoResultado;
                ActionResponse.Mensaje = result.MensajeResultado;
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Error no se pudo cambiar la clave.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        [Route("generar-codigo-verificacion-correo")]
        public async Task<JsonResult> GenerarCodigoVerificacionCorreo(string Correo)
        {
            ActionResponse = new ActionResponse();
            // Verificar si el usuario/correo existe
            CrearUsuarioSecundarioParameterVM parameter = new CrearUsuarioSecundarioParameterVM();
            parameter.Correo = Correo;
            var result = await _serviceUsuario.ObtenerUsuarioSecundario(parameter);
            if (result.usuario != null)
            {
                // Enviar Codigo de verificacion
                CodigoGeneradoValidacionParameterVM codigoGeneradoValidacionParameterVM = new CodigoGeneradoValidacionParameterVM();
                codigoGeneradoValidacionParameterVM.Correo = Correo;
                codigoGeneradoValidacionParameterVM.Nombres = "Estimado Usuario";
                codigoGeneradoValidacionParameterVM.ImagenGrupoTrans = $"{this.GetUriHost()}/{_configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupo]}";
                var codigoGenerado = await _serviceAcceso.GenerarCodigoVerificacionCorreo(codigoGeneradoValidacionParameterVM);

                if (codigoGenerado.CodigoResultado == 0)
                {
                    ActionResponse.Codigo = 0;
                    ActionResponse.Mensaje = "Se envió su código de verificación con exito, revise su correo e ingrese el código.";
                }
                else
                {
                    ActionResponse.Codigo = -1;
                    ActionResponse.Mensaje = "El correo no se encuentra registrado en el sistema.";
                }
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "El correo no se encuentra registrado en el sistema.";
            }

            return Json(ActionResponse);

        }

        [HttpPost]
        [Route("validar-codigo-verificacion-correo")]
        public async Task<JsonResult> ValidarCodigoVerificacionCorreo(string Correo, string Codigo)
        {
            ActionResponse = new ActionResponse();
            VerificarCodigoValidacionParameterVM parameterVM = new VerificarCodigoValidacionParameterVM();
            parameterVM.Correo = Correo;
            parameterVM.CodigoVerificacion = Codigo;
            var codigoGenerado = await _serviceAcceso.VerificarCodigoValidacionCorreo(parameterVM);


            if (codigoGenerado.CodigoResultado == 0)
            {
                ActionResponse.Codigo = 0;
                ActionResponse.Mensaje = "Envio de código de verificacion es válido.";
            }
            else
            {
                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "El código de verificación no es válido.";
            }

            return Json(ActionResponse);

        }

        [HttpGet]
        public async Task<IActionResult> Metodo(int code)
        {
            var data = $"Codigo de estado {code}";
            return View("Index", data);
        }
    }
}
