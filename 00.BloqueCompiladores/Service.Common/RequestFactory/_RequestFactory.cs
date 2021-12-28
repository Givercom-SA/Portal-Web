using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Common.Core;
using Service.Common.Logging.Application;
using Service.Common.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using ViewModel.Common.Response;
using static ViewModel.Common.Request.DataRequestViewModelResponse;

namespace Service.Common.RequestFactory
{
    public class _RequestFactory
    {

        private static ILogger _logger = ApplicationLogging.CreateLogger("_RequestFactory");
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        #region SENDREQUEST

        /**********************************************************************************/
        public static async Task<TResponse> SendRequest<TRequest, TResponse>(TRequest viewModel, HttpContext context, string uri, string bearerToken, int? timeOut = null) where TRequest : DataRequestViewModel where TResponse : DataRequestViewModelResponse
        {
            return await Execute<TRequest, TResponse>(uri, context, TypeRequest.POST, viewModel, bearerToken, timeOut);
        }
        /**********************************************************************************/

        /**********************************************************************************/
        public static async Task<TRequest> SendRequest<TRequest>(TRequest viewModel, HttpContext context, string uri, string bearerToken, int? timeOut = null) where TRequest : DataRequestViewModel
        {
            return await Execute<TRequest, TRequest>(uri, context, TypeRequest.POST, viewModel, bearerToken, timeOut);
        }
        /**********************************************************************************/

        /**********************************************************************************/
        public static async Task<List<TRequest>> SendRequestListToList<TRequest>(List<TRequest> viewModel, HttpContext context, string uri, string bearerToken, int? timeOut = null) where TRequest : DataRequestViewModel
        {
            return await Execute<List<TRequest>, List<TRequest>>(uri, context, TypeRequest.POST, viewModel, bearerToken, timeOut);
        }
        /**********************************************************************************/

        /**********************************************************************************/
        public static async Task<List<TResponse>> SendRequestToList<TRequest, TResponse>(TRequest viewModel, HttpContext context, string uri, string bearerToken, int? timeOut = null) where TRequest : DataRequestViewModel where TResponse : DataRequestViewModelResponse
        {
            return await Execute<TRequest, List<TResponse>>(uri, context, TypeRequest.POST, viewModel, bearerToken, timeOut);
        }
        /**********************************************************************************/

        /**********************************************************************************/
        public static async Task<List<T>> SendRequestToListObject<TRequest, T>(TRequest viewModel, HttpContext context, string uri, string bearerToken) where TRequest : DataRequestViewModel
        {
            var response = await Execute<TRequest>(uri, context, TypeRequest.POST, viewModel, bearerToken);
            var result = response.ContentAsType<List<T>>();
            return result;
        }
        /**********************************************************************************/

        /**********************************************************************************/
        public static async Task<T> SendRequestToObject<TRequest, T>(TRequest viewModel, HttpContext context, string uri, string bearerToken) where TRequest : DataRequestViewModel
        {
            var response = await Execute<TRequest>(uri, context, TypeRequest.POST, viewModel, bearerToken);
            var result = response.ContentAsType<T>();
            return result;
        }
        /**********************************************************************************/

        #endregion

        #region GETREQUEST
        /**********************************************************************************/
        public static async Task<TResponse> GetRequestQueryBuilder<TResponse>(HttpContext context, string uri, string bearerToken) where TResponse : DataRequestViewModelResponse
        {
            var response = await Execute<String>(uri, context, TypeRequest.GET, null, bearerToken);
            var result = response.ContentAsType<TResponse>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (result == null)
                { result.Resultado = DataRequestViewModelResponse.ResultadoServicio.Error; }
                else { if (result.Resultado == 0) result.Resultado = DataRequestViewModelResponse.ResultadoServicio.Exito; }
            }
            else
            {
                result.Resultado = DataRequestViewModelResponse.ResultadoServicio.Error;
            }
            return result;
        }
        /**********************************************************************************/

        /**********************************************************************************/
        public static async Task<List<TResponse>> GetRequestToList<TResponse>(HttpContext context, string uri, string bearerToken) where TResponse : DataRequestViewModelResponse
        {
            var response = await Execute<TResponse>(uri, context, TypeRequest.GET, null, bearerToken);
            var result = response.ContentAsType<List<TResponse>>();
            return result;
        }
        /**********************************************************************************/


        /**********************************************************************************/
        public static async Task<T> GetRequestObject<T>(HttpContext context, string uri, string bearerToken)
        {
            var response = await Execute<T>(uri, context, TypeRequest.GET, null, bearerToken);
            var result = response.ContentAsType<T>();
            return result;
        }
        /**********************************************************************************/

        #endregion

        #region SENDREQUEST GETRESPONSEBY

        /**********************************************************************************/
        public static async Task<ResponseViewModel<T>> SendRequestObject_<TRequest, T>(TRequest viewModel, HttpContext context, string uri, string bearerToken)
        {
            return await Execute<TRequest, ResponseViewModel<T>>(uri, context, TypeRequest.POST, viewModel, bearerToken);
        }
        /**********************************************************************************/
        #endregion


        private static async Task<R> Execute<T, R>(string uri, HttpContext context, TypeRequest type, object value = null, string bearerToken = null, int? timeOut = null)
        {
            HttpResponseMessage response = null;
            var result = (R)Activator.CreateInstance(typeof(R));
            var request = (T)value;
            var totalMemoriaInicial = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var parametros = request != null ? ObtenerParametros(request) : string.Empty;
            Stopwatch sw = Stopwatch.StartNew();
            var servicio = HttpResponseExtensions.GetServiceName(uri);
            var rpta = string.Empty;
            var serverIn = string.Format("{0}", Server.GetLocalIPAddress());
            var statusCode = "000";
            var metodo = type.ToString().ToLower().PadLeft(4, ' ');
            _logger.LogInformation(string.Format("RF|Service|Ini|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|", metodo, statusCode, serverIn, servicio, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaInicial.ToString(), totalMemoriaInicial.ToString(), parametros, timeOut));
            var serverOut = string.Empty;
            try
            {
                switch (type)
                {
                    case TypeRequest.GET:
                        response = await HttpRequestFactory.Get(uri, bearerToken, timeOut, "x");
                        break;
                    case TypeRequest.POST:
                        response = await HttpRequestFactory.Post(uri, value, bearerToken, timeOut, "x");
                        break;
                    default:
                        rpta = "EXECUTE:::Solicitud no controlada";
                        break;
                }
                if (response == null)
                {
                    rpta = string.Format("EXECUTE:::{0} Es nulo", result.GetType().Name);
                    response = new HttpResponseMessage(HttpStatusCode.NoContent);
                    result = default(R);
                }
                else
                {

                    try { serverOut = response.Headers.GetValues("SERVER").FirstOrDefault(); } catch { }
                    var totalRegistros = 0;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        using (var streamReader = new StreamReader(responseStream))
                        using (var jsonTextReader = new JsonTextReader(streamReader))
                            result = _jsonSerializer.Deserialize<R>(jsonTextReader);

                        /*Para invocaciones con DataRequestViewModel y DataRequestViewModelResponse*/
                        /*No es para invocaciones ResponseViewModel*/
                        if (result.GetType().BaseType.GetProperty("Resultado") != null)
                        {
                            if ((int)result.GetType().BaseType.GetProperty("Resultado").GetValue(result) == 0) result.GetType().BaseType.GetProperty("Resultado").SetValue(result, ResultadoServicio.Exito, null);
                        }
                        PropertyInfo propInfoTotalRegistros = result.GetType().GetProperty("TotalRegistros");
                        totalRegistros = propInfoTotalRegistros == null ? 0 : (Int32)propInfoTotalRegistros.GetValue(result, null);

                        rpta = string.Format("EXECUTE:::{0} {1}", result.GetType().Name, totalRegistros == 0 ? "" : "rows:" + totalRegistros.ToString());

                    }
                    else
                    {
                        rpta = string.Format("EXECUTE:::{0}", result.GetType().Name);

                        result.GetType().BaseType.GetProperty("Resultado").SetValue(result, ResultadoServicio.Error, null);
                    }
                    statusCode = string.Format("{0}", (int)response.StatusCode);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                try
                {
                    result.GetType().BaseType.GetProperty("Resultado").SetValue(result, ResultadoServicio.Error, null);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, ex.Message);
                }
            }
            sw.Stop();
            var totalMemoriaFinal = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var totalMemoriaReducida = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var parametrosResult = result != null ? ObtenerParametros(result) : string.Empty;
            _logger.LogInformation(string.Format("RF|Service|Fin|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}", metodo, statusCode, serverOut, servicio, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaFinal.ToString(), totalMemoriaReducida.ToString(), rpta, parametrosResult));

            return result;
        }


        private static async Task<HttpResponseMessage> Execute<T>(string uri, HttpContext context, TypeRequest type, object value = null, string bearerToken = null, int? timeOut = null)
        {
            HttpResponseMessage response = null;
            var request = (T)value;
            var totalMemoriaInicial = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var parametros = request != null ? ObtenerParametros(request) : string.Empty;
            Stopwatch sw = Stopwatch.StartNew();
            var servicio = HttpResponseExtensions.GetServiceName(uri);
            var statusCode = "000";
            var serverIn = string.Format("{0}", Server.GetLocalIPAddress());
            var metodo = type.ToString().ToLower().PadLeft(4, ' ');
            _logger.LogInformation(string.Format("RX|Service|Ini|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", metodo, statusCode, serverIn, servicio, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaInicial.ToString(), totalMemoriaInicial.ToString(), parametros));

            try
            {
                switch (type)
                {
                    case TypeRequest.GET:
                        response = await HttpRequestFactory.Get(uri, bearerToken, timeOut, "x");
                        break;
                    case TypeRequest.POST:
                        response = await HttpRequestFactory.Post(uri, value, bearerToken, timeOut, "x");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            var serverOut = string.Empty;
            try { serverOut = response.Headers.GetValues("SERVER").FirstOrDefault(); statusCode = string.Format("{0}", (int)response.StatusCode); } catch { }
            if (response == null) response = new HttpResponseMessage(HttpStatusCode.NoContent);

            sw.Stop();
            var totalMemoriaFinal = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var totalMemoriaReducida = Process.GetCurrentProcess().WorkingSet64 / 1024;
            _logger.LogInformation(string.Format("RX|Service|Fin|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", metodo, statusCode, serverOut, servicio, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaFinal.ToString(), totalMemoriaReducida.ToString()));

            return response;
        }

        private static bool IsNavtiveType<T>()
        {
            var x = false;
            if (typeof(T) == typeof(string) || typeof(T) == typeof(int)) x = true;
            return x;
        }




        public enum TypeRequest
        {
            GET,
            POST
        }

        public static string ObtenerParametros(object objeto)
        {
            var parametros = string.Empty;
            try
            {
                foreach (var infoMiembro in objeto.GetType().GetMembers())
                {
                    if (infoMiembro.MemberType == MemberTypes.Property)
                    {
                        var valorParam = string.Empty;
                        if (((PropertyInfo)infoMiembro).GetValue(objeto, null) != null)
                        {
                            valorParam = ((PropertyInfo)infoMiembro).GetValue(objeto, null).ToString();
                        }

                        parametros = parametros + ":::" + (infoMiembro).Name + "=" + valorParam;
                    }
                }
            }
            catch (Exception ex)
            {
                parametros = ex.Message;
            }

            return parametros;
        }
    }
}
