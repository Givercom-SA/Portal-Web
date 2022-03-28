using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Servicio.Embarque.Repositorio;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using AutoMapper;
using System.Net;
using TransMares.Core;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using Servicio.Embarque.ServiceExterno;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using Utilitario.Constante;
using Servicio.Embarque.Models.SolicitudFacturacion;
using ViewModel.Datos.Embarque.SolicitudFacturacion;
using Servicio.Embarque.Models.Entidad;
using ViewModel.Datos.Entidad;

namespace Servicio.Embarque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntidadController : ControllerBase
    {
        private readonly IEntidadRepository _repositoryEntidad;
        private readonly IMapper _mapper;

        private static ILogger _logger = ApplicationLogging.CreateLogger("EntidadController");


        public EntidadController(IEntidadRepository repository,
                                       
                                          IMapper mapper)
        {
            _repositoryEntidad = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("entidad-listar-tipo")]
        public async Task<ActionResult<ListarEntidadResultVM>> ListarEntidadTipo([FromBody] ListarEntidadParameterVM parameter)
        {
            var result = _repositoryEntidad.ListarTipoEnidad(_mapper.Map<ListarEntidadParameter>(parameter));
            return _mapper.Map<ListarEntidadResultVM>(result);
        }

    }
}
