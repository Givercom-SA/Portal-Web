using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueryHandlers.Common;
using QueryHandlers.Common.EnvioCorreoError;
using Service.Common;
using Service.Common.BaseServiceController;
using Servicio.Notificacion.QueryHandler.LimpiarContadorNotificacionPorUsuario;
using Servicio.Notificacion.QueryHandler.LimpiarNotificacionesPorUsuario;
using Servicio.Notificacion.QueryHandler.ObtenerNotificacionesPorUsuario;
using ViewModel.Notificacion;
using ViewModel.Notificacion.EnvioCorreoError;
using static ViewModel.Common.Request.DataRequestViewModelResponse;

namespace Servicio.Notificacion.Controllers
{
    [Route("api/[controller]")]
    public class NotificacionErrorController : BaseController
    {
        private readonly QueryDispatcher _queryDispatcher;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificacionErrorController> _logger;
        private readonly IConfiguration _configuration;
        public NotificacionErrorController(
            QueryDispatcher queryDispatcher,
            IConfiguration configuration,
            ILogger<NotificacionErrorController> logger)
        {
            _queryDispatcher = queryDispatcher;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("envio-correo-error-afpnet")]
        public async Task<IActionResult> EnvioCorreoErrorAfpNet([FromBody] EnvioCorreoErrorVM viewModel)
        {
            var result = new EnvioCorreoErrorRespuestaVM();
            try
            {
                var parameter = new EnvioCorreoErrorParameter { Mensaje = viewModel.Mensaje, Asunto = viewModel.Asunto, TipoAsunto = viewModel.TipoAsunto };
                var handler = new EnvioCorreoErrorQuery(_configuration, _logger);
                var queryResult = (EnvioCorreoErrorResult)handler.Handle(parameter);
                result = new EnvioCorreoErrorRespuestaVM
                {
                    Resultado = ResultadoServicio.Exito,
                    ResultadoCorreo = true
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                result.Resultado = ResultadoServicio.Error;
                return StatusCode(500);

            }
            return Ok(result);

        }
    }
}
