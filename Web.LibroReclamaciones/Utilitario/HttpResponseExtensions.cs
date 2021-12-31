using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Web.LibroReclamaciones.Utilitario
{
    public static class HttpResponseExtensions
    {
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            if (response.Content == null) return default(T);
            using (var responseStream = response.Content.ReadAsStreamAsync().Result)
            using (var streamReader = new StreamReader(responseStream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
                return jsonTextReader == null ? default(T) : _jsonSerializer.Deserialize<T>(jsonTextReader);
        }

        public static string ContentAsJson(this HttpResponseMessage response)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        public static string ContentAsString(this HttpResponseMessage response)
        {
            return response.Content.ReadAsStringAsync().Result;
        }

        public static string GetServiceName(string url)
        {
            var final = string.Empty;
            try
            {
                var diccionarioAntiguaDistribucion = new Dictionary<string, string> {
                    { "10011", "Servicio.Seguridad" },
                    { "10012", "Servicio.Usuario" },
                    { "10001", "Servicio.Afiliados"},
                    { "10013", "Servicio.Afiliacion"},
                    { "10014", "Servicio.Mef"},
                    { "10006", "Servicio.Maestro"},
                    { "10004", "Servicio.Empleador"},
                    { "10008", "Servicio.Planilla"},
                    { "10009", "Servicio.Reniec"},
                    { "10005", "Servicio.LiquidacionPrevia"},
                    { "10019", "Servicio.ObligacionPago"},
                    { "10017", "Servicio.Regularizacion"},
                    { "10020", "Servicio.Pagos"},
                    { "10003", "Servicio.Notificacion"}
            };

                var diccionarioNuevaDistribucion = new Dictionary<string, string> {
                    { "10001", "Servicio.Maestro" },
                    { "10002", "Servicio.Planillas" },
                    { "10003", "Servicio.Seguridad"},
                    { "10004", "Servicio.Empleador"},
                    { "10005", "Servicio.Pagos"}
            };


                var x = url.Substring(url.LastIndexOf("/api/") - 1, 1);
                if (int.TryParse(x, out int o))
                {
                    foreach (var item in diccionarioAntiguaDistribucion)
                    {
                        if (url.Contains(item.Key))
                        {
                            final = string.Format("{0}{1}", item.Value, url.Substring(url.LastIndexOf("/api/")));
                            break;
                        }
                    }
                }
                else
                {
                    foreach (var item in diccionarioNuevaDistribucion)
                    {
                        if (url.Contains(item.Key))
                        {
                            final = string.Format("{0}{1}", item.Value, url.Substring(url.Substring(0, url.LastIndexOf("/api/")).LastIndexOf("/")));
                            break;
                        }
                    }
                }

                Uri myUri = new Uri(url);
                if (string.IsNullOrEmpty(final))
                {
                    final = "servicio";
                }
                else
                {
                    try
                    {
                        final = myUri.Host + "/" + final;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception e)
            {
                final = "servicio";
            }


            return final;
        }
    }
}
