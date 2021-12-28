using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servicio.Maestro.Models;
using Servicio.Maestro.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using ViewModel.Datos.ListaCorreos;
using ViewModel.Datos.Parametros;

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

        [HttpGet]
        [Route("ObtenerCorreosPorPerfil")]
        public ActionResult<ListaCorreosVW> ObtenerCorreosPorPerfil(int idParam)
        {
            var result = _repository.ObtenerCorreosPorPerfil(idParam);
            return _mapper.Map<ListaCorreosVW>(result);
        }
    }
}
