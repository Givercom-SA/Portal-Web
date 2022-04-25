using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Servicio.Maestro.Models;
using Servicio.Maestro.Models.LibroReclamo;
using Servicio.Maestro.Models.Tarifario;
using Servicio.Maestro.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using ViewModel.Datos.ListaCorreos;
using ViewModel.Datos.Parametros;
using ViewModel.Reclamo;
using ViewModel.Tarifario;

namespace Servicio.Maestro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
        private readonly IParametrosRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ParametrosController> _logger;

        public ParametrosController(IParametrosRepository repository, IMapper mapper,
            ILogger<ParametrosController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("obtenerParametrosIdPadre")]
        public ActionResult<ListaParametrosVM> ObtenerParametroPorIdPadre(int idParam)
        {
            ListaParametroResult result = new ListaParametroResult();
            try
            {
                result = _repository.ObtenerParametroPorIdPadre(idParam);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListaParametrosVM>(result);
        }

        [HttpPost]
        [Route("obtenerDocumentoPorTipoentidad")]
        public ActionResult<ListarDocumentoTipoEntidadVM> ObtenerDocumentoPorTipoEntidad(ListDocumentoTipoEntidadParameterVM parameter)
        {
            ListaDocumentoTipoEntidadResult result = new ListaDocumentoTipoEntidadResult();
            try
            {
                result = _repository.ObtenerDocumentoPorTipoEntidad(_mapper.Map<ListarDocumentoTipoEntidadParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarDocumentoTipoEntidadVM>(result);
        }

        [HttpPost]
        [Route("reclamo-empresas")]
        public ActionResult<ListaEmpresasResultVM> ListarEmpresasReclamo(ListaEmpresasParameterVM parameter)
        {
            ListaEmpresasResult result = new ListaEmpresasResult();
            try
            {
                result = _repository.ListEmpresasReclamo(_mapper.Map<ListaEmpresasParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListaEmpresasResultVM>(result);
        }


        [HttpGet]
        [Route("ObtenerCorreosPorPerfil")]
        public ActionResult<ListaCorreosVW> ObtenerCorreosPorPerfil(int idParam)
        {
            ListaCorreosResult result = new ListaCorreosResult();
            try
            {
                result = _repository.ObtenerCorreosPorPerfil(idParam);
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListaCorreosVW>(result);
        }

        [HttpPost]
        [Route("reclamo-unidadnegocio-xempresa")]
        public ActionResult<ListaUnidadNegocioXEmpresasResultVM> ListarUnidadNegocioXEmpresa(ListaUnidadNegocioXEmpresaParameterVM parameter)
        {
            ListaUnidadNegocioXEmpresasResult result = new ListaUnidadNegocioXEmpresasResult();
            try
            {
                result = _repository.ListarUnidadNegocioXEmpresa(_mapper.Map<ListaUnidadNegocioXEmpresaParameter>(parameter));
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListaUnidadNegocioXEmpresasResultVM>(result);
        }
        [HttpPost]
        [Route("reclamo-registrar")]
        public ActionResult<RegistrarReclamoResultVM> RegistrarReclamo(RegistrarReclamoParameterVM parameter)
        {
            RegistraReclamoResult result = new RegistraReclamoResult();
            try
            {
                result = _repository.RegistrarReclamo(_mapper.Map<RegistrarReclamoParameter>(parameter));
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<RegistrarReclamoResultVM>(result);
        }

        [HttpPost]
        [Route("tarifario-listar")]
        public ActionResult<ListarTarifarioResultVM> ListarTarifario(ListarTarifarioParameterVM parameter)
        {
            ListarTarifarioResult result = new ListarTarifarioResult();
            try
            {
                result = _repository.ListarTarifario(_mapper.Map<ListarTarifarioParameter>(parameter));
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarTarifarioResultVM>(result);
        }
    }
}