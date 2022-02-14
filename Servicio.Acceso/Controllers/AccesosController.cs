using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Perfil;
using Servicio.Acceso.Models.SolicitarAcceso;
using Servicio.Acceso.Repositorio;
using Servicio.Acceso.ServiceConsumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.Acceso;
using ViewModel.Datos.LoginInicial;
using ViewModel.Datos.Message;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.SolictudAcceso;
using ViewModel.Datos.UsuarioRegistro;

namespace Servicio.Acceso.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesosController : ControllerBase
    {
        private readonly IAccesosRepository _repository;
        private readonly IMapper _mapper;
        private readonly ServicioMessage _servicioMessage;
        
        private readonly ISolicitarAccesoRepository _repositorySoli;
        private readonly ICodigoGeneradoValidacionRepository _repositoryCodigoGenerado;

        private readonly string UrlArchivoDocbusinessPartner;
        public AccesosController(IAccesosRepository repository,
            ISolicitarAccesoRepository repositorySoli,
            ICodigoGeneradoValidacionRepository repositoryCodigo,
            IMapper mapper, IConfiguration configuration,
            ServicioMessage servicioMessage)
        {
            _repository = repository;
            _repositorySoli = repositorySoli;
            _repositoryCodigoGenerado = repositoryCodigo;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
            UrlArchivoDocbusinessPartner = $"{configuration["RutaArchivos:DocumentoBusinessPartners"]}";
        }

        [HttpPost]
        [Route("login-usuario")]
        public ActionResult<UsuarioRegistroVM> LoginUsuario([FromBody] LoginInicialVW loginUsuario)
        {
            var result = _repository.ObtenerLogin(loginUsuario.CorreoElectronico, loginUsuario.Contrasenia, loginUsuario.EntidadRuc);
            return _mapper.Map<UsuarioRegistroVM>(result);
        }

        [HttpGet]
        [Route("usuario-leer/{IdUsuario}")]
        public ActionResult<UsuarioRegistroVM> UsuarioLeer(int IdUsuario)
        {
            var result = _repository.OtenerUsuario(IdUsuario);
            return _mapper.Map<UsuarioRegistroVM>(result);
        }

        [HttpPost]
        [Route("solicitar-acceso")]
        public ActionResult<SolicitarAccesoResultVM> SolicitarAcceso([FromBody] SolicitarAccesoParameterVM parameter)
        {
            var result = _repositorySoli.RegistrarSolicitudAcceso(_mapper.Map<SolicitarAccesoParameter>(parameter));

            if (result.IN_CODIGO_RESULTADO==0)
            {
                if (!(parameter.ProcesoFacturacion == false &&
                    parameter.TipoEntidad.Exists(x => x.CodigoTipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS.ToString()))))
                {

                    enviarCorreo(parameter.Correo,
                               "!Bienvenido a Transmares Group!",
                               new FormatoCorreoBody().formatoBodyBienvenidaAprobado(
                                   string.Format("{0}: {1}", parameter.TipoDocumento, parameter.NumeroDocumento),
                                   parameter.RazonSocial,
                                   string.Format("{0} {1} {2}", parameter.RepresentaLegalNombre, parameter.RepresentaLegalApellidoPaterno,
                                   parameter.RepresentaLegalMaterno), parameter.Correo, result.Contrasenia, parameter.ImagenGrupoTrans),
                               UrlArchivoDocbusinessPartner,
                               parameter.AcuerdoSeguroCadenaSuministro);
                }
            }
            return _mapper.Map<SolicitarAccesoResultVM>(result);


        }

        private async  void enviarCorreo(string _correo, string _asunto, string _contenido, string archivo, bool adjunto = false)
        {
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
            if (adjunto)
            {
                enviarMessageCorreoParameterVM.RequestMessage.Archivos = new string[1];
                enviarMessageCorreoParameterVM.RequestMessage.Archivos[0] = archivo;
            }
             await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
        }

        [HttpPost]
        [Route("generar-codigo-validacion")]
        public ActionResult<CodigoGeneradoValidacionResultVM> GenerarCodigoValidacion([FromBody] CodigoGeneradoValidacionParameterVM parameter)
        {

            var param = _mapper.Map<CodigoGeneradoValidacionParameter>(parameter);
            param.CODIGO_VERIFICACION = GenerarCadenaLongit(6);

            enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Código de verificación",
             new FormatoCorreoBody().formatoBodyActivarCuenta(parameter.Nombres, param.CODIGO_VERIFICACION, parameter.ImagenGrupoTrans));

            var result = _repositoryCodigoGenerado.GenerarCodigoValidacion(param);

            return _mapper.Map<CodigoGeneradoValidacionResultVM>(result);

        }


        [HttpPost]
        [Route("verificar-codigo-validacion")]
        public ActionResult<VerificarCodigoValidacionResultVM> VerificarCodigoValidacion([FromBody] VerificarCodigoValidacionParameterVM parameter)
        {
            var pranVert = _mapper.Map<VerificarCodigoValidacionParameter>(parameter);

            var result = _repositoryCodigoGenerado.VerificarCodigoValidacion(pranVert);

            return _mapper.Map<VerificarCodigoValidacionResultVM>(result);
        }

        [HttpPost]
        [Route("verificar-solicitud-acceso")]
        public ActionResult<VerificarSolicitudAccesoResultVM> VerificarSolicitudAccesp([FromBody] VerificarSolicitudAccesoParameterVM parameter)
        {
            var pranVert = _mapper.Map<VerificarSolicitudAccesoParameter>(parameter);

            var result = _repositorySoli.VerificarSolicitudAcceso(pranVert);

            return _mapper.Map<VerificarSolicitudAccesoResultVM>(result);
        }



        public string GenerarCadenaLongit(int plongitud)
        {
            Random obj = new Random();
            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int longitud = posibles.Length;
            char letra;
            int longitudnuevacadena = plongitud;
            string nuevacadena = "";

            for (int i = 0; i < longitudnuevacadena; i++)
            {
                letra = posibles[obj.Next(longitud)];
                nuevacadena += letra.ToString();
            }
            return nuevacadena;

        }


        private async  void enviarCorreo(string _correo, string _asunto, string _contenido)
        {
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido = _contenido;
            enviarMessageCorreoParameterVM.RequestMessage.Correo = _correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = _asunto;
           await  _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);


        }

        [HttpPost]
        [Route("generar-codigo-validacion-correo")]
        public ActionResult<CodigoGeneradoValidacionResultVM> GenerarCodigoValidacionCorreo([FromBody] CodigoGeneradoValidacionParameterVM parameter)
        {

            var param = _mapper.Map<CodigoGeneradoValidacionParameter>(parameter);
            param.CODIGO_VERIFICACION = GenerarCadenaLongit(6);

            enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Código de verifiación",
             new FormatoCorreoBody().formatoBodyActivarCuenta(parameter.Nombres, param.CODIGO_VERIFICACION,parameter.ImagenGrupoTrans));

            var result = _repositoryCodigoGenerado.GenerarCodigoValidacionCorreo(param);

            return _mapper.Map<CodigoGeneradoValidacionResultVM>(result);

        }

        [HttpPost]
        [Route("verificar-codigo-validacion-correo")]
        public ActionResult<VerificarCodigoValidacionResultVM> VerificarCodigoValidacionCorreo([FromBody] VerificarCodigoValidacionParameterVM parameter)
        {
            var pranVert = _mapper.Map<VerificarCodigoValidacionParameter>(parameter);

            var result = _repositoryCodigoGenerado.VerificarCodigoValidacionCorreo(pranVert);

            return _mapper.Map<VerificarCodigoValidacionResultVM>(result);
        }

        [HttpPost]
        [Route("actualizar-contrasenia")]
        public ActionResult<CambiarContrasenaResultVM> ActualizarContrasenia([FromBody] CambiarContrasenaParameterVM parameter)
        {
            return    _mapper.Map<CambiarContrasenaResultVM>(_repository.ActualizarContrasenia(_mapper.Map <CambiarContrasenaParameter>(parameter)));


        }

        [HttpPost]
        [Route("obtener-perfiles")]
        public ActionResult<ListarPerfilesResultVM> ObtenerPerfiles([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.ObtenerPerfiles(parameter.Nombre, parameter.Activo,parameter.Tipo);
            return _mapper.Map<ListarPerfilesResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-perfiles-activos")]
        public ActionResult<ListarPerfilesActivosResultVM> ObtenerPerfilesActivos([FromBody] ListarPerfilActivosParameterVM parameter)
        {
            var result = _repository.ObtenerPerfilesActivos(_mapper.Map<ListarPerfilesActivosParameter>(parameter));
            return _mapper.Map<ListarPerfilesActivosResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-perfil")]
        public ActionResult<ObtenerPerfilResultVM> ObtenerPerfil([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.ObtenerPerfil(parameter.IdPerfil);
            return _mapper.Map<ObtenerPerfilResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-perfiles-entidad")]
        public ActionResult<ListarPerfilesResultVM> ObtenerPerfilesPorEntidad([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.ObtenerPerfilesPorEntidad(parameter.IdEntidad);
            return _mapper.Map<ListarPerfilesResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-menus")]
        public ActionResult<ListarMenusPerfilResultVM> ObtenerMenus()
        {
            var result = _repository.ObtenerMenus();
            return _mapper.Map<ListarMenusPerfilResultVM>(result);
        }

        [HttpPost]
        [Route("crear-perfil")]
        public ActionResult<PerfilResultVM> CrearPerfil([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.CrearPerfil(_mapper.Map<PerfilParameter>(parameter));
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpPost]
        [Route("editar-perfil")]
        public ActionResult<PerfilResultVM> EditarPerfil([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.EditarPerfil(_mapper.Map<PerfilParameter>(parameter));
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpPost]
        [Route("eliminar-perfil")]
        public ActionResult<PerfilResultVM> EliminarPerfil([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.EliminarPerfil(parameter.IdPerfil);
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpPost]
        [Route("veridicar-accesos-perfil")]
        public ActionResult<PerfilResultVM> VerificarAccesosPerfil([FromBody] PerfilParameterVM parameter)
        {
            var result = _repository.VerificarAccesoPerfil(parameter.IdPerfil);
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-transgroup-empresas")]
        public ActionResult<ListarTransGroupEmpresaVM> ObtenerTransGroupEmpresas()
        {
            var result = _repository.ObtenerTransGroupEmpresa();
            return _mapper.Map<ListarTransGroupEmpresaVM>(result);
        }

    }
}
