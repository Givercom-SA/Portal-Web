using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Servicio.Usuario.Models.Usuario;
using Servicio.Usuario.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.Message;
using ViewModel.Datos.UsuarioRegistro;

namespace Servicio.Usuario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly ServiceConsumer.ServicioMessage _servicioMessage;
        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(IUsuarioRepository repository, 
            IMapper mapper, ServiceConsumer.ServicioMessage servicioMessage,
            ILogger<UsuarioController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
            _logger = logger;
        }

        [HttpPost]
        [Route("obtener-usuarios")]
        public ActionResult<ListarUsuariosResultVM> ObtenerSolicitudes(Models.Usuario.ListarUsuariosParameter parameter)
        {
            ListarUsuariosResult result = new ListarUsuariosResult();
            try
            {
                result = _repository.ObtenerResultados(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return _mapper.Map<ListarUsuariosResultVM>(result);
        }

        [HttpPost]
        [Route("listar-cliente-usuarios")]
        public ActionResult<ListarUsuariosResultVM> ListarClienteUsuarios(Models.Usuario.ListarUsuariosParameter parameter)
        {
            var result = new ListarUsuariosResult();
            try
            {
                result = _repository.ListarClienteUsuarios(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarUsuariosResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-usuario/{Id}")]
        public ActionResult<LeerUsuarioResultVM> ObtenerUsuario(int Id)
        {
            LeerUsuariosResult result = new LeerUsuariosResult();
            try
            {
                result = _repository.ObtenerUsuario(Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<LeerUsuarioResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-usuarios-secundarios")]
        public ActionResult<ListarUsuariosResultVM> ObtenerUsuariosSecundarios(Models.Usuario.ListarUsuariosParameter parameter)
        {
            ListarUsuariosResult result = new ListarUsuariosResult();
            try
            {
                result = _repository.ObtenerUsuariosSecundarios(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return _mapper.Map<ListarUsuariosResultVM>(result);

        }

        [HttpPost]
        [Route("crear-usuario-secundario")]
        public ActionResult<UsuarioSecundarioResultVM> CrearUsuarioSecundario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.CrearUsuarioSecundario(parameter);
                if (parameter.RequiereConfirmacion)
                {
                    if (result.IN_CODIGO_RESULTADO== 0)
                    {
                        parameter.UrlConfirmacion = string.Format("{0}?token={1}", parameter.UrlConfirmacion, result.IdUsuario);
                        enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Confirma Tu Correo",
                         new FormatoCorreoBody().formatoBodyBienvendaUsuarioSecundarioRenovada(parameter.Nombres, parameter.ContraseniaNocifrado, parameter.UrlConfirmacion, parameter.ImagenGrupoTrans));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }


        [HttpPost]
        [Route("crear-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> CrearUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.CrearUsuario(parameter);

                if (result.IN_CODIGO_RESULTADO == 0)
                {
                    if (parameter.RequiereConfirmacion)
                    {
                        parameter.UrlConfirmacion = string.Format("{0}?token={1}", parameter.UrlConfirmacion,Security.Common.Encriptador.Instance.EncriptarTexto(result.IdUsuario.ToString()));
                        enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Confirmar Correo",
                     new FormatoCorreoBody().formatoBodyBienvenidaUsuarioInterno(parameter.Nombres, parameter.ContraseniaNocifrado, parameter.UrlConfirmacion, parameter.ImagenGrupoTrans));


                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("editar-usuario-secundario")]
        public ActionResult<UsuarioSecundarioResultVM> EditarUsuarioSecundario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.EditarUsuarioSecundario(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("editar-usuario-interno")]
        public ActionResult<UsuarioSecundarioResultVM> EditarUsuarioInterno(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.EditarUsuarioInterno(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("cambiar-clave-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> CambiarClaveUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.CambiarClaveUsuario(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("habilitar-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> HabilitarUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.HabilitarUsuario(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-usuario-secundario")]
        public ActionResult<UsuarioSecundarioResultVM> ObtenerUsuarioSecundario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.ObtenerUsuarioSecundario(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> ObtenerUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.ObtenerUsuario(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-lista-usuario-menu")]
        public ActionResult<ListarUsuarioMenuResultVM> ObtenerListaUsuarioMenu(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = new ListarUsuarioMenuResult();
            try
            {
                result = _repository.ObtenerListaUsuarioMenu(parameter);

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarUsuarioMenuResultVM>(result);
        }

        [HttpPost]
        [Route("existe-usuario")]
        public ActionResult<bool> ExisteUsuario(string Correo)
        {
            bool result;
            try
            {
                result = _repository.ExisteUsuario(Correo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return result;
        }

        [HttpGet]
        [Route("confirmar-correo-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> ConfirmarCorreoUsuario(int IdUsuario)
        {
            UsuarioSecundarioResult result = new UsuarioSecundarioResult();
            try
            {
                result = _repository.ConfirmarCorreoUsuario(IdUsuario);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        private async void enviarCorreo(string _correo, string _asunto, string _contenido)
        {
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;

            await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
        }


        [HttpPost]
        [Route("cambiar-perfil-defecto")]
        public ActionResult<CambiarPerfilDefectoResultVM> CambiarPerfilDefecto(CambiarPerfilDefectoParameterVM parameter)
        {
            CambiarPerfilDefectoResult result = new CambiarPerfilDefectoResult();
            try
            {
                result = _repository.CambiarPerfilDefecto(_mapper.Map<CambiarPerfilDefectoParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<CambiarPerfilDefectoResultVM>(result);
        }

        [HttpPost]
        [Route("dasboard-cliente")]
        public ActionResult<DashboardClienteResultVM> DashboardCliente(DashboardClienteParameterVM parameter)
        {
            DashboardClienteResult result = new DashboardClienteResult();
            try
            {
                result = _repository.DashboardCliente(_mapper.Map<DashboardClienteParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<DashboardClienteResultVM>(result);
        }

        [HttpPost]
        [Route("dasboard-admin")]
        public ActionResult<DashboardAdminResultVM> DashboardAdmin(DashboardAdminParameterVM parameter)
        {
            DashboardAdminResult result = new DashboardAdminResult();
            try
            {
                result = _repository.DashboardAdmin(_mapper.Map<DashboardAdminParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<DashboardAdminResultVM>(result);
        }

    }
}