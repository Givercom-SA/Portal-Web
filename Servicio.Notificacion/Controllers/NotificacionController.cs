using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QueryHandlers.Common;
using Service.Common;
using Service.Common.BaseServiceController;
using Servicio.Notificacion.QueryHandler.LimpiarNotificacionesPorUsuario;
using Servicio.Notificacion.QueryHandler.ObtenerNotificacionesPorUsuario;
using ViewModel.Notificacion;

namespace Servicio.Notificacion.Controllers
{
    [Route("api/[controller]")]
    public class NotificacionController : BaseController
    {
        private readonly QueryDispatcher _queryDispatcher;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificacionController> _logger;

        public NotificacionController(
            QueryDispatcher queryDispatcher,
            IMapper mapper,
            ILogger<NotificacionController> logger)
        {
            _queryDispatcher = queryDispatcher;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("obtener-notificaciones-por-usuario/{codigoUsuario}")]
        public IActionResult ObtenerNotificacionPorUsuario(int codigoUsuario)
        {

            try
            {
                var param = new ObtenerNotificacionesPorUsuarioParameter { CodigoUsuario = codigoUsuario };
                var result = (ObtenerNotificacionesPorUsuarioResult)_queryDispatcher.Dispatch(param);
                var responseErrores = CustomResponse.GetOkByResponse(_mapper.Map<List<NotificacionVM>>(result.Elementos));

                return Ok(responseErrores);

                //return Ok(CustomResponse.GetOkByResponse(_mapper.Map<List<NotificacionVM>>(result.Elementos)));

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al obtener notificaciones por usuario : " + codigoUsuario);

                return StatusCode(500, CustomResponse.GetErrorByResponse<List<NotificacionVM>>(e.Message));
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("limpiar-notificaciones-por-usuario/{codigoUsuario}")]
        public IActionResult LimpiarNotificacionPorUsuario(int codigoUsuario)
        {
            try
            {
                var param = new LimpiarNotificacionesPorUsuarioParameter { CodigoUsuario = codigoUsuario };
                _queryDispatcher.Dispatch(param);

                return Ok(CustomResponse.GetOkByResponse(true));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al obtener notificaciones por usuario : " + codigoUsuario);

                return StatusCode(500, CustomResponse.GetErrorByResponse<bool>(e.Message));
            }

        }

    }
}
