using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QueryHandlers.Common.EnvioCorreoError;
using System;
using System.Diagnostics;
using System.Reflection;

namespace QueryHandlers.Common
{
    public class QueryDispatcher
    {
        private readonly ILifetimeScope container;
        private readonly ILogger<QueryDispatcher> _logger;
        private readonly IConfiguration _configuration;
        public QueryDispatcher(ILifetimeScope container, ILogger<QueryDispatcher> logger, IConfiguration configuration)
        {
            this.container = container;
            this._logger = logger;
            _configuration = configuration;
        }

        public QueryResult Dispatch<T>(T parameter) where T : QueryParameter
        {
            Stopwatch sw = Stopwatch.StartNew();
            var nameQuery = string.Empty;
            using (var scope = container.BeginLifetimeScope())
            {
                QueryResult result = null;
                var handlerType = typeof(IQueryHandler<>).MakeGenericType(parameter.GetType());
                var parametros = ObtenerParametros(parameter);
                dynamic handler = scope.ResolveOptional(handlerType);
                nameQuery = ((object)handler).GetType().FullName;
                var totalMemoriaInicial = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024;
                _logger.LogInformation(string.Format("Query|Ini|{0}|{1}|{2}|{3}|{4}|{5}|{6}|","00", nameQuery, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaInicial.ToString(), totalMemoriaInicial.ToString(), parametros));


                try
                {
                    result = handler.Handle(parameter);

                }
                catch (Exception e)
                {
                    e = Unwrap(e);
                    _logger.LogError(e, e.Message);
                    var parameterEnvioCorreo = new EnvioCorreoErrorParameter { Mensaje = string.Format("{0} | {1} | {2} | {3} | {4}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), nameQuery, e.Message, e.Source, e.StackTrace), TipoAsunto = "BD" };
                    var queryEnvioCorreo = new EnvioCorreoErrorQuery(_configuration, _logger);
                    var resultEnvioCorreo = new EnvioCorreoErrorResult();
                    queryEnvioCorreo.Handle(parameterEnvioCorreo);
                    sw.Stop();
                    var parametrosResult_ = result != null ? ObtenerParametros(result) : string.Empty;
                    var totalMemoriaFinal_ = Process.GetCurrentProcess().WorkingSet64 / 1024;
                    //GC.Collect();
                    var totalMemoriaReducida_ = Process.GetCurrentProcess().WorkingSet64 / 1024;
                    _logger.LogInformation(string.Format("Query|Fin|{0}|{1}|{2}|{3}|{4}|{5}|{6}|","ER", nameQuery, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaFinal_.ToString(), totalMemoriaReducida_.ToString(), parametrosResult_));

                    throw e;
                }
                sw.Stop();
                var parametrosResult = result != null ? ObtenerParametros(result) : string.Empty;
                var totalMemoriaFinal = Process.GetCurrentProcess().WorkingSet64 / 1024;
                //GC.Collect();
                var totalMemoriaReducida = Process.GetCurrentProcess().WorkingSet64 / 1024;
                _logger.LogInformation(string.Format("Query|Fin|{0}|{1}|{2}|{3}|{4}|{5}|{6}|","OK", nameQuery, sw.Elapsed, totalMemoriaInicial.ToString(), totalMemoriaFinal.ToString(), totalMemoriaReducida.ToString(), parametrosResult));

                return result;
            }
        }

        private static Exception Unwrap(Exception ex)
        {
            while (null != ex.InnerException)
            {
                ex = ex.InnerException;
            }

            return ex;
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
