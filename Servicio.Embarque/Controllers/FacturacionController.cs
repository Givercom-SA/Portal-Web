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

namespace Servicio.Embarque.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturacionController : ControllerBase
    {
        private readonly ISolicitudFacturacionRepository _repository;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly IMapper _mapper;

        private static ILogger _logger = ApplicationLogging.CreateLogger("FacturacionController");


        public FacturacionController(ISolicitudFacturacionRepository repository,
                                          ServicioEmbarques serviceEmbarques,
                                          IMapper mapper)
        {
            _repository = repository;
            _serviceEmbarques = serviceEmbarques;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("solicitud-facturacion-bandeja")]
        public async Task<ActionResult<ListarSolicitudFacturacionBandejaResultVM>> ListarSolicitudFacturacionBandeja([FromBody] ListarSolicitudFacturacionBandejaParameterVM parameter)
        {
            var result = _repository.ObtenerFacturacionListaBandeja(_mapper.Map<ListarSolicitudFacturacionBandejaParameter>(parameter));
            return _mapper.Map<ListarSolicitudFacturacionBandejaResultVM>(result);
        }

        [HttpPost]
        [Route("solicitud-facturacion-bandeja-leer")]
        public async Task<ActionResult<LeerSolicitudFacturacionBandejaResultVM>> LeerSolicitudFacturacionBandeja([FromBody] LeerSolicitudFacturacionBandejaParameterVM parameter)
        {
            var result = _repository.ObtenerFacturacionBandeja(_mapper.Map<LeerSolicitudFacturacionBandejaParameter>(parameter));
            return _mapper.Map<LeerSolicitudFacturacionBandejaResultVM>(result);
        }


        [HttpPost]
        [Route("solicitud-facturacion-estado")]
        public async Task<ActionResult<SolicitarFacturacionEstadoResultVM>> RegistrarSolicitudFacturacionEstado([FromBody] SolicitarFacturacionEstadoParameterVM parameter)
        {
            var result = _repository.RegistrarSolicitudFacturacionEstado(_mapper.Map<SolicitarFacturacionEstadoParameter>(parameter));
            return _mapper.Map<SolicitarFacturacionEstadoResultVM>(result);
        }

        [HttpGet]
        [Route("solicitud-facturacion-listar-keybl")]
        public async Task<ActionResult<LeerSolicitudFacturacionKeyBlResultVM>> ListarSolicitudFacturacionKeyBl(string  keyBl)
        {
            var result = _repository.ListarSolicitudFacturacionPorKeyBl(keyBl);
            return _mapper.Map<LeerSolicitudFacturacionKeyBlResultVM>(result);
        }

        [HttpPost]
        [Route("solicitud-facturacion-registrar")]
        public ActionResult<SolicitarFacturacionResultVM> SolicitudFacturacionCrear(SolicitarFacturacionParameterVM parameter)
        {
            var result = _repository.SolicituFacturacion(_mapper.Map<SolicitarFacturacionParameter>(parameter));
            return _mapper.Map<SolicitarFacturacionResultVM>(result);
        }

    }
}
