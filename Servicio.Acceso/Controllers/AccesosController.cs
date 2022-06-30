﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.Menu;
using Servicio.Acceso.Models.Perfil;
using Servicio.Acceso.Models.SolicitarAcceso;
using Servicio.Acceso.Models.Vista;
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
using ViewModel.Datos.Menu;
using ViewModel.Datos.Message;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.SolictudAcceso;
using ViewModel.Datos.UsuarioRegistro;
using ViewModel.Datos.Vista;

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
        private readonly ILogger<AccesosController> _logger;
        private readonly string UrlArchivoDocbusinessPartner;
        public AccesosController(IAccesosRepository repository,
            ISolicitarAccesoRepository repositorySoli,
            ICodigoGeneradoValidacionRepository repositoryCodigo,
            IMapper mapper, IConfiguration configuration,
            ServicioMessage servicioMessage,
            ILogger<AccesosController> logger)
        {
            _repository = repository;
            _repositorySoli = repositorySoli;
            _repositoryCodigoGenerado = repositoryCodigo;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
            UrlArchivoDocbusinessPartner = $"{configuration["RutaArchivos:DocumentoBusinessPartners"]}";
            _logger = logger;
        }

        [HttpPost]
        [Route("login-usuario")]
        public ActionResult<UsuarioRegistroVM> LoginUsuario([FromBody] LoginInicialVW loginUsuario)
        {
            Models.UsuarioResult result = new Models.UsuarioResult();
            try { 
             result = _repository.ObtenerLogin(loginUsuario.CorreoElectronico, loginUsuario.Contrasenia, loginUsuario.EntidadRuc);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioRegistroVM>(result);
        }
        [HttpPost]
        [Route("menu-registrar")]
        public ActionResult<MantenimientoMenuResultVM> MenuRegistrar([FromBody] MantenimientoMenuParameterVM parameter)
        {
            MantenimientoMenuResult result = new MantenimientoMenuResult();
            try
            {
                result = _repository.RegistrarMenu(_mapper.Map<MantenimientoMenuParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<MantenimientoMenuResultVM>(result);
        }
        [HttpPost]
        [Route("menu-modificar")]
        public ActionResult<MantenimientoMenuResultVM> MenuModificar([FromBody] MantenimientoMenuParameterVM parameter)
        {
            MantenimientoMenuResult result = new MantenimientoMenuResult();
            try
            {
                result = _repository.ModificarMenu(_mapper.Map<MantenimientoMenuParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<MantenimientoMenuResultVM>(result);
        }
        [HttpPost]
        [Route("vista-registrar")]
        public ActionResult<MantenimientoVistaResultVM> VistaRegistrar([FromBody] MantenimientoVistaParameterVM parameter)
        {
            MantenimientoVistaResult result = new MantenimientoVistaResult();
            try
            {
                result = _repository.RegistrarVista(_mapper.Map<MantenimientoVistaParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<MantenimientoVistaResultVM>(result);
        }
        [HttpPost]
        [Route("vista-modificar")]
        public ActionResult<MantenimientoVistaResultVM> VistaModificar([FromBody] MantenimientoVistaParameterVM parameter)
        {
            MantenimientoVistaResult result = new MantenimientoVistaResult();
            try
            {
                result = _repository.ModificarVista(_mapper.Map<MantenimientoVistaParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<MantenimientoVistaResultVM>(result);
        }
        [HttpGet]
        [Route("usuario-leer/{IdUsuario}")]
        public ActionResult<UsuarioRegistroVM> UsuarioLeer(int IdUsuario)
        {
            Models.UsuarioResult result = new Models.UsuarioResult();
            try { 
             result = _repository.OtenerUsuario(IdUsuario);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<UsuarioRegistroVM>(result);
        }

        [HttpGet]
        [Route("vistas-todos")]
        public ActionResult<ListarTodoVistaResultVM> ListarTodoVistas()
        {
            ListarTodoVistaResult result = new ListarTodoVistaResult();
            try
            {
                result = _repository.ListarTodoVistas();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarTodoVistaResultVM>(result);
        }

        [HttpGet]
        [Route("menus-todos")]
        public ActionResult<ListarTodoMenusResultVM> ListarTodoMenus()
        {
            ListarTodoMenuResult result = new ListarTodoMenuResult();
            try
            {
                result = _repository.ListarTodoMenus();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarTodoMenusResultVM>(result);
        }

        [HttpGet]
        [Route("leer-menu/{IdMenu}")]
        public ActionResult<LeerMenusResultVM> LeerMenu(int IdMenu)
        {
            LeerMenuResult leerMenusResultVM = new LeerMenuResult();
            try
            {
                leerMenusResultVM = _repository.LeerMenus(IdMenu);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<LeerMenusResultVM>(leerMenusResultVM);
        }
        [HttpGet]
        [Route("vistas-para-menu")]
        public ActionResult<LeerMenusResultVM> ListarTodasVistasParaMenu()
        {
            LeerMenuResult leerMenusResultVM = new LeerMenuResult();
            try
            {
                leerMenusResultVM = _repository.ListarTodasVistasParaMenu();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<LeerMenusResultVM>(leerMenusResultVM);
        }

        [HttpGet]
        [Route("leer-vista/{IdVista}")]
        public ActionResult<LeerVistaResultVM> LeerVista(int IdVista)
        {
            LeerVistaResult leerVista = new LeerVistaResult();
            try
            {
                leerVista = _repository.LeerVista(IdVista);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<LeerVistaResultVM>(leerVista);
        }


        [HttpPost]
        [Route("solicitar-acceso")]
        public ActionResult<SolicitarAccesoResultVM> SolicitarAcceso([FromBody] SolicitarAccesoParameterVM parameter)
        {
            SolicitarAccesoResult result = new SolicitarAccesoResult();
            try { 
             result = _repositorySoli.RegistrarSolicitudAcceso(_mapper.Map<SolicitarAccesoParameter>(parameter));

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
                                   parameter.RepresentaLegalMaterno), parameter.Correo, result.Contrasenia, parameter.ImagenGrupoTrans,
                                   parameter.UrlTransmares),
                               UrlArchivoDocbusinessPartner,
                               parameter.AcuerdoSeguroCadenaSuministro);
                }
            }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
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
            CodigoGeneradoValidacionResult result =new CodigoGeneradoValidacionResult();
            try { 
            var param = _mapper.Map<CodigoGeneradoValidacionParameter>(parameter);
            param.CODIGO_VERIFICACION = GenerarCadenaLongit(6);

            enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Código de verificación",
             new FormatoCorreoBody().formatoBodyActivarCuenta(parameter.Nombres, param.CODIGO_VERIFICACION, parameter.ImagenGrupoTrans));

             result = _repositoryCodigoGenerado.GenerarCodigoValidacion(param);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return _mapper.Map<CodigoGeneradoValidacionResultVM>(result);

        }


        [HttpPost]
        [Route("verificar-codigo-validacion")]
        public ActionResult<VerificarCodigoValidacionResultVM> VerificarCodigoValidacion([FromBody] VerificarCodigoValidacionParameterVM parameter)
        {
            VerificarCodigoValidacionResult result =new VerificarCodigoValidacionResult();
            try { 
            var pranVert = _mapper.Map<VerificarCodigoValidacionParameter>(parameter);

             result = _repositoryCodigoGenerado.VerificarCodigoValidacion(pranVert);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<VerificarCodigoValidacionResultVM>(result);
        }

        [HttpPost]
        [Route("verificar-solicitud-acceso")]
        public ActionResult<VerificarSolicitudAccesoResultVM> VerificarSolicitudAccesp([FromBody] VerificarSolicitudAccesoParameterVM parameter)
        {
            VerificarSolicitudAccesoResult result =new VerificarSolicitudAccesoResult();
            try { 
            var pranVert = _mapper.Map<VerificarSolicitudAccesoParameter>(parameter);

             result = _repositorySoli.VerificarSolicitudAcceso(pranVert);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
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
            CodigoGeneradoValidacionResult result =new CodigoGeneradoValidacionResult();
            try { 
            var param = _mapper.Map<CodigoGeneradoValidacionParameter>(parameter);
            param.CODIGO_VERIFICACION = GenerarCadenaLongit(6);

            enviarCorreo(parameter.Correo, "!Bienvenido a Transmares Group! Código de verifiación",
             new FormatoCorreoBody().formatoBodyActivarCuenta(parameter.Nombres, param.CODIGO_VERIFICACION,parameter.ImagenGrupoTrans));

             result = _repositoryCodigoGenerado.GenerarCodigoValidacionCorreo(param);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<CodigoGeneradoValidacionResultVM>(result);

        }

        [HttpPost]
        [Route("verificar-codigo-validacion-correo")]
        public ActionResult<VerificarCodigoValidacionResultVM> VerificarCodigoValidacionCorreo([FromBody] VerificarCodigoValidacionParameterVM parameter)
        {
            VerificarCodigoValidacionResult result = new VerificarCodigoValidacionResult();
            try { 
            var pranVert = _mapper.Map<VerificarCodigoValidacionParameter>(parameter);

             result = _repositoryCodigoGenerado.VerificarCodigoValidacionCorreo(pranVert);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<VerificarCodigoValidacionResultVM>(result);
        }

        [HttpPost]
        [Route("actualizar-contrasenia")]
        public ActionResult<CambiarContrasenaResultVM> ActualizarContrasenia([FromBody] CambiarContrasenaParameterVM parameter)
        {
            CambiarContrasenaResult result =new CambiarContrasenaResult(); 
            try {
                result = _repository.ActualizarContrasenia(_mapper.Map<CambiarContrasenaParameter>(parameter));

           
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<CambiarContrasenaResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-perfiles")]
        public ActionResult<ListarPerfilesResultVM> ObtenerPerfiles([FromBody] PerfilParameterVM parameter)
        {
            ListarPerfilesResult result =new ListarPerfilesResult();
            try { 
             result = _repository.ObtenerPerfiles(parameter.Nombre, parameter.Activo,parameter.Tipo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarPerfilesResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-perfiles-activos")]
        public ActionResult<ListarPerfilesActivosResultVM> ObtenerPerfilesActivos([FromBody] ListarPerfilActivosParameterVM parameter)
        {
            ListarPerfilesActivosResult result =new ListarPerfilesActivosResult();
            try { 
             result = _repository.ObtenerPerfilesActivos(_mapper.Map<ListarPerfilesActivosParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarPerfilesActivosResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-perfil")]
        public ActionResult<ObtenerPerfilResultVM> ObtenerPerfil([FromBody] PerfilParameterVM parameter)
        {
            ObtenerPerfilResult result =new ObtenerPerfilResult();
            try { 
             result = _repository.ObtenerPerfil(parameter.IdPerfil);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ObtenerPerfilResultVM>(result);
        }


        [HttpPost]
        [Route("obtener-perfil-usuario")]
        public ActionResult<ObtenerPerfilResultVM> ObtenerPerfilUsuario([FromBody] PerfilParameterVM parameter)
        {
            ObtenerPerfilResult result = new ObtenerPerfilResult();
            try
            {
                result = _repository.ObtenerPerfilPorUsuario(parameter.IdPerfil,parameter.IdUsuario);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ObtenerPerfilResultVM>(result);
        }

        [HttpPost]
        [Route("obtener-menus")]
        public ActionResult<ListarMenusResultVM> ObtenerMenus([FromBody] ListarMenusParameterVM parameter)
        {
            ListarMenuResult result = new ListarMenuResult();
            try
            {
                result = _repository.ListarMenus(_mapper.Map<ListarMenuParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarMenusResultVM>(result);
        }
        [HttpPost]
        [Route("obtener-vistas")]
        public ActionResult<ListarVistasResultVM> ObtenerVistas([FromBody] ListarVistaParameterVM parameter)
        {
            ListarVistaResult result = new ListarVistaResult();
            try
            {
                result = _repository.ListarVistas(_mapper.Map<ListarVistaParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarVistasResultVM>(result);
        }
        [HttpPost]
        [Route("obtener-perfiles-entidad")]
        public ActionResult<ListarPerfilesResultVM> ObtenerPerfilesPorEntidad([FromBody] PerfilParameterVM parameter)
        {
            ListarPerfilesResult result =new ListarPerfilesResult();
            try { 
             result = _repository.ObtenerPerfilesPorEntidad(parameter.IdEntidad);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarPerfilesResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-menus")]
        public ActionResult<ListarMenusPerfilResultVM> ObtenerMenus()
        {
            ListarMenusPerfilResult result =new ListarMenusPerfilResult();
            try { 
             result = _repository.ObtenerMenus();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarMenusPerfilResultVM>(result);
        }

        [HttpPost]
        [Route("crear-perfil")]
        public ActionResult<PerfilResultVM> CrearPerfil([FromBody] PerfilParameterVM parameter)
        {
            PerfilResult result =new PerfilResult();
            try { 
             result = _repository.CrearPerfil(_mapper.Map<PerfilParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<PerfilResultVM>(result);
        }


        [HttpPost]
        [Route("vista-listar-area")]
        public ActionResult<ListarAreaControllerActionResultVM> ListarSoloArea([FromBody] ListarAreaControllerActionParameterVM parameter)
        {
            ListarAreaControllerActionResult result = new ListarAreaControllerActionResult();
            try
            {
                result = _repository.ListarSoloAreas(_mapper.Map<ListarAreaControllerActionParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarAreaControllerActionResultVM>(result);
        }

        [HttpPost]
        [Route("vista-listar-controller")]
        public ActionResult<ListarAreaControllerActionResultVM> ListarSoloController([FromBody] ListarAreaControllerActionParameterVM parameter)
        {
            ListarAreaControllerActionResult result = new ListarAreaControllerActionResult();
            try
            {
                result = _repository.ListarSoloController(_mapper.Map<ListarAreaControllerActionParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarAreaControllerActionResultVM>(result);
        }
        [HttpPost]
        [Route("vista-listar-action")]
        public ActionResult<ListarAreaControllerActionResultVM> ListarSoloAction([FromBody] ListarAreaControllerActionParameterVM parameter)
        {
            ListarAreaControllerActionResult result = new ListarAreaControllerActionResult();
            try
            {
                result = _repository.ListarSoloAction(_mapper.Map<ListarAreaControllerActionParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarAreaControllerActionResultVM>(result);
        }

        [HttpPost]
        [Route("editar-perfil")]
        public ActionResult<PerfilResultVM> EditarPerfil([FromBody] PerfilParameterVM parameter)
        {
            PerfilResult result = new PerfilResult();
            try { 
             result = _repository.EditarPerfil(_mapper.Map<PerfilParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpPost]
        [Route("eliminar-perfil")]
        public ActionResult<PerfilResultVM> EliminarPerfil([FromBody] PerfilParameterVM parameter)
        {
            PerfilResult result = new PerfilResult();
            try { 
             result = _repository.EliminarPerfil(parameter.IdPerfil);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpPost]
        [Route("veridicar-accesos-perfil")]
        public ActionResult<PerfilResultVM> VerificarAccesosPerfil([FromBody] PerfilParameterVM parameter)
        {
            PerfilResult result = new PerfilResult();
            try { 
             result = _repository.VerificarAccesoPerfil(parameter.IdPerfil);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<PerfilResultVM>(result);
        }

        [HttpGet]
        [Route("obtener-transgroup-empresas")]
        public ActionResult<ListarTransGroupEmpresaVM> ObtenerTransGroupEmpresas()
        {
            ListarTransGroupEmpresaResult result = new ListarTransGroupEmpresaResult();
            try { 
             result = _repository.ObtenerTransGroupEmpresa();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarTransGroupEmpresaVM>(result);
        }

    }
}
