using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio.Maestro.Models;
using Servicio.Maestro.Models.LibroReclamo;
using Servicio.Maestro.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using ViewModel.Datos.ListaCorreos;
using ViewModel.Datos.Parametros;
using ViewModel.Reclamo;

namespace Servicio.Maestro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametrosController : ControllerBase
    {
        private readonly IParametrosRepository _repository;
        private readonly IMapper _mapper;

        public ParametrosController(IParametrosRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("obtenerParametrosIdPadre")]
        public ActionResult<ListaParametrosVM> ObtenerParametroPorIdPadre(int idParam)
        {
            var result = _repository.ObtenerParametroPorIdPadre(idParam);
            return _mapper.Map<ListaParametrosVM>(result);
        }

        [HttpPost]
        [Route("obtenerDocumentoPorTipoentidad")]
        public ActionResult<ListarDocumentoTipoEntidadVM> ObtenerDocumentoPorTipoEntidad(ListDocumentoTipoEntidadParameterVM parameter)
        {
            var result = _repository.ObtenerDocumentoPorTipoEntidad(_mapper.Map<ListarDocumentoTipoEntidadParameter>(parameter));
            return _mapper.Map<ListarDocumentoTipoEntidadVM>(result);
        }

        [HttpPost]
        [Route("reclamo-empresas")]
        public ActionResult<ListaEmpresasResultVM> ListarEmpresasReclamo(ListaEmpresasParameterVM parameter)
        {
            var result = _repository.ListEmpresasReclamo(_mapper.Map<ListaEmpresasParameter>(parameter));
            return _mapper.Map<ListaEmpresasResultVM>(result);
        }


        [HttpGet]
        [Route("ObtenerCorreosPorPerfil")]
        public ActionResult<ListaCorreosVW> ObtenerCorreosPorPerfil(int idParam)
        {
            var result = _repository.ObtenerCorreosPorPerfil(idParam);
            return _mapper.Map<ListaCorreosVW>(result);
        }

        [HttpPost]
        [Route("reclamo-unidadnegocio-xempresa")]
        public ActionResult<ListaUnidadNegocioXEmpresasResultVM> ListarUnidadNegocioXEmpresa(ListaUnidadNegocioXEmpresaParameterVM parameter)
        {
            var result = _repository.ListarUnidadNegocioXEmpresa(_mapper.Map<ListaUnidadNegocioXEmpresaParameter>(parameter));
            return _mapper.Map<ListaUnidadNegocioXEmpresasResultVM>(result);
        }

    }
}
