using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Common;
using Service.Common.Core;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Common.Response;
using ViewModel.Notificacion;
using Web.Principal.Util;
using Service.Common.RequestFactory;
using ViewModel.Notificacion.EnvioCorreoError;
using Service.Common.Logging.Application;

namespace Web.Principal.ServiceConsumer
{
    public class NotificacionService : IServiceConsumer
    {
        private IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext;
        private const string SERVICIO_BASE = "notificacion/";
        private const string SERVICIO_BASE_AFPNET_ERROR = "notificacionerrorafpnet/";
        private readonly string URL_BASE;
        private readonly string URL_BASE_AFPNET_ERROR;
        private static ILogger _logger = ApplicationLogging.CreateLogger("BaseController");

        //public NotificacionService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        //{
        //    this.URL_BASE = $"{configuration[AppSettingsKeys.Servicio.Notificacion]}{SERVICIO_BASE}";
        //    _httpContextAccessor = httpContextAccessor;
        //}
        //public NotificacionService(IConfiguration configuration, HttpContext httpContext)
        //{

        //    this.URL_BASE_AFPNET_ERROR = $"{configuration[AppSettingsKeys.Servicio.Notificacion]}{SERVICIO_BASE_AFPNET_ERROR}";
        //    _httpContext = httpContext;
        //}

        public async Task<ResponseViewModel<List<NotificacionVM>>> ObtenerNotificacionesPorUsuario(int codigoUsuario)
        {
            ResponseViewModel<List<NotificacionVM>> resultado = null;

            var context = _httpContextAccessor.HttpContext;
            const string SERVICIO = "obtener-notificaciones-por-usuario";
            var uri = $"{URL_BASE}{SERVICIO}/{codigoUsuario}";

            try
            {
                var bearerToken = context.Session.GetUserContent().BearerToken;
                resultado = await _RequestFactory.GetRequestObject<ResponseViewModel<List<NotificacionVM>>>(context, uri, bearerToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al Obtener Notificaciones para el usuario : " + codigoUsuario);
            }

            resultado = resultado ?? CustomResponse.GetErrorByResponse<List<NotificacionVM>>();

            if (!resultado.IsSuccess)
                _logger.LogWarning("Sucedio un error en el servicio : " + uri);

            return resultado;
        }

        public async Task<ResponseViewModel<bool>> LimpiarNotificacionesPorUsuario(int codigoUsuario)
        {
            var usuario = _httpContextAccessor.HttpContext.Session.GetUserContent();

            const string SERVICIO = "limpiar-notificaciones-por-usuario";
            var uri = $"{URL_BASE}{SERVICIO}/{codigoUsuario}";
            var respuesta = await HttpRequestFactory.Get(uri);

            var resultado = respuesta.ContentAsType<ResponseViewModel<bool>>();

            if (!resultado.IsSuccess)
                throw new Exception("Sucedio un error en el servicio : " + resultado.Message);

            return resultado;
        }

        //public async Task<EnvioCorreoErrorRespuestaVM> EnvioCorreoAfpNetError(EnvioCorreoErrorVM viewModel)
        //{
        //    const string SERVICIO = "envio-correo-error-afpnet";
        //    var uri = $"{URL_BASE_AFPNET_ERROR}{SERVICIO}";
        //    var bearerToken = _httpContext.Session.GetUserContent().BearerToken;
        //    var resultado = await _RequestFactory.SendRequest<EnvioCorreoErrorVM, EnvioCorreoErrorRespuestaVM>(viewModel, _httpContext, uri, bearerToken);
        //    return resultado;
        //}

    }
}
