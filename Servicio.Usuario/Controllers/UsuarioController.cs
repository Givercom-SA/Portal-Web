using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public UsuarioController(IUsuarioRepository repository, IMapper mapper, ServiceConsumer.ServicioMessage servicioMessage)
        {
            _repository = repository;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
        }

        [HttpPost]
        [Route("obtener-usuarios")]
        public ActionResult<ListarUsuariosResultVM> ObtenerSolicitudes(Models.Usuario.ListarUsuariosParameter parameter)
        {
            var result = _repository.ObtenerResultados(parameter);
            return _mapper.Map<ListarUsuariosResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-usuario/{Id}")]
        public ActionResult<LeerUsuarioResultVM> ObtenerUsuario(int Id)
        {
            var result = _repository.ObtenerUsuario(Id);
            return _mapper.Map<LeerUsuarioResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-usuarios-secundarios")]
        public ActionResult<ListarUsuariosResultVM> ObtenerUsuariosSecundarios(Models.Usuario.ListarUsuariosParameter parameter)
        {
            var result = _repository.ObtenerUsuariosSecundarios(parameter);
            return _mapper.Map<ListarUsuariosResultVM>(result);
        }

        [HttpPost]
        [Route("crear-usuario-secundario")]
        public async Task<ActionResult<UsuarioSecundarioResultVM>> CrearUsuarioSecundario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.CrearUsuarioSecundario(parameter);
            if (parameter.RequiereConfirmacion)
            {
                if (result.IN_CODIGO_RESULTADO > 0)
                {
                    parameter.UrlConfirmacion = string.Format("{0}?token={1}", parameter.UrlConfirmacion, result.IN_CODIGO_RESULTADO);
                }
                enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Confirmar Correo",
                 new FormatoCorreoBody().formatoBodyBienvendaUsuarioSecundarioRenovada(parameter.Nombres, parameter.ContraseniaNocifrado,parameter.UrlConfirmacion, parameter.ImagenGrupoTrans));
            }

            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }


        [HttpPost]
        [Route("crear-usuario")]
        public async Task<ActionResult<UsuarioSecundarioResultVM>> CrearUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.CrearUsuario(parameter);
            if (parameter.RequiereConfirmacion)
            {
                if (result.IN_CODIGO_RESULTADO > 0)
                {
                    parameter.UrlConfirmacion = string.Format("{0}?token={1}", parameter.UrlConfirmacion, result.IN_CODIGO_RESULTADO);
                }
                enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Confirmar Correo",
                 new FormatoCorreoBody().formatoBodyBienvendaUsuarioSecundarioRenovada(parameter.Nombres, parameter.ContraseniaNocifrado, parameter.UrlConfirmacion, parameter.ImagenGrupoTrans));
            }
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("editar-usuario-secundario")]
        public ActionResult<UsuarioSecundarioResultVM> EditarUsuarioSecundario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.EditarUsuarioSecundario(parameter);
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("editar-usuario-interno")]
        public ActionResult<UsuarioSecundarioResultVM> EditarUsuarioInterno(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.EditarUsuarioInterno(parameter);
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("cambiar-clave-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> CambiarClaveUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.CambiarClaveUsuario(parameter);
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("habilitar-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> HabilitarUsuario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.HabilitarUsuario(parameter);
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-usuario-secundario")]
        public ActionResult<UsuarioSecundarioResultVM> ObtenerUsuarioSecundario(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.ObtenerUsuarioSecundario(parameter);
            return _mapper.Map<UsuarioSecundarioResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-lista-usuario-menu")]
        public ActionResult<ListarUsuarioMenuResultVM> ObtenerListaUsuarioMenu(Models.Usuario.CrearUsuarioSecundarioParameter parameter)
        {
            var result = _repository.ObtenerListaUsuarioMenu(parameter);
            return _mapper.Map<ListarUsuarioMenuResultVM>(result);
        }

        [HttpPost]
        [Route("existe-usuario")]
        public ActionResult<bool> ExisteUsuario(string Correo)
        {
            var result = _repository.ExisteUsuario(Correo);
            return result;
        }

        [HttpGet]
        [Route("confirmar-correo-usuario")]
        public ActionResult<UsuarioSecundarioResultVM> ConfirmarCorreoUsuario(int IdUsuario)
        {
            var result = _repository.ConfirmarCorreoUsuario(IdUsuario);
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
            var result = _repository.CambiarPerfilDefecto(_mapper.Map<CambiarPerfilDefectoParameter>(parameter));
            return _mapper.Map<CambiarPerfilDefectoResultVM>(result);
        }




    }
}