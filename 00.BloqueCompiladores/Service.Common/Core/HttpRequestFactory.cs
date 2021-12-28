using log4net;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using Service.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Common.Core
{
    public static class HttpRequestFactory
    {
        //private const int _timeOut = 20;
        //private static readonly HttpClient client = new HttpClient();

        public static async Task<HttpResponseMessage> Get(
            string requestUri, string bearerToken = null, int? timeOut = null, string x = null)
        {
            return await SendAsync(HttpMethod.Get, requestUri, null, bearerToken, new TimeSpan(0, timeOut ?? 40, 00), x);
        }

        public static async Task<HttpResponseMessage> Post(
           string requestUri, object value, string bearerToken = null, int? timeOut = null, string x = null)
        {
            return await SendAsync(HttpMethod.Post, requestUri, new JsonContent(value), bearerToken, new TimeSpan(0, timeOut ?? 40, 00), x);
        }

        private async static Task<HttpResponseMessage> SendAsync(HttpMethod method, string requestUri, HttpContent content, string bearerToken, TimeSpan timeout, string x = null)
        {
            var acceptHeader = "application/json";

            ILogger logger = null;
            try { logger = ApplicationLogging.CreateLogger("HttpRequestFactory"); } catch { };
            var servicio = HttpResponseExtensions.GetServiceName(requestUri);
            var serverIn = string.Format("{0}", Server.GetLocalIPAddress());
            var metodo = method.ToString().ToLower().PadLeft(4, ' ');
            var statusCode = "000";
            if (string.IsNullOrEmpty(x) && logger != null) logger.LogInformation(string.Format("RB|Service|Ini|{0}|{1}|{2}|{3}", metodo, statusCode, serverIn, servicio == "servicio" ? requestUri : servicio));
            using (var client = new HttpClient() { Timeout = timeout })
            using (var request = new HttpRequestMessage(method, new Uri(requestUri)))
            {
                if (content != null)
                    request.Content = content;

                if (!string.IsNullOrEmpty(bearerToken))
                    request.Headers.Authorization =
                      new AuthenticationHeaderValue("Bearer", bearerToken);

                request.Headers.Accept.Clear();
                if (!string.IsNullOrEmpty(acceptHeader))
                    request.Headers.Accept.Add(
                       new MediaTypeWithQualityHeaderValue(acceptHeader));

                if (LogicalThreadContext.Properties["idunico"] != null) request.Headers.Add("idunico", LogicalThreadContext.Properties["idunico"].ToString());
                if (LogicalThreadContext.Properties["user"] != null) request.Headers.Add("user", LogicalThreadContext.Properties["user"].ToString());



                var send = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                var serverOut = string.Empty;
                statusCode = ((int)send.StatusCode).ToString();
                try { serverOut = send.Headers.GetValues("SERVER").FirstOrDefault(); } catch { }
                if (string.IsNullOrEmpty(x) && logger != null) logger.LogInformation(string.Format("RB|Service|Fin|{0}|{1}|{2}|{3}", metodo, statusCode, serverOut, servicio));
                return send;
            }
        }


    }
}
