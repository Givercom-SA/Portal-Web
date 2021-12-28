using log4net;
using log4net.Appender;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Filter;
using System;

namespace Service.Common.Logging.Extensions
{
    public static class Log4netExtensions
    {
        public static class Separador
        {
            public static readonly string Campo = "|";
            public static readonly string Propiedad = "|";

        }
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, IAppender[] appenders)
        {
            factory.AddProvider(new Log4NetProvider(appenders));
            //factory.WithFilter(new Log4NetFilter(factory));

            Application.ApplicationLogging.LoggerFactory = factory;
            LogicalThreadContext.Properties["idunico"] = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper().Substring(1, 10);
            LogicalThreadContext.Properties["user"] = string.Format("{0}{1}{2}", "--------", Separador.Propiedad, "--------");
            return factory;
        }

        public static void SetIdUnicoTransaccion(string idUnico = null)
        {
            var id = string.Empty;
            if (string.IsNullOrEmpty(idUnico)) id = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper().Substring(1, 10);
            else id = idUnico;
            LogicalThreadContext.Properties["idunico"] = id;
        }

        public static string GetIdUnicoTransaccion()
        {
            var id = string.Empty;
            id = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper().Substring(1, 10);
            LogicalThreadContext.Properties["idunico"] = id;
            return id;
        }



        public static void Usuario(string codigo, string nombreCuenta = null)
        {
            var usu = "sin datos";

            if (string.IsNullOrEmpty(nombreCuenta)) usu = codigo;
            else
            {
                var cod = codigo.PadLeft(8, '0');
                var nom = nombreCuenta.PadLeft(8, 'X');
                usu = string.Format("{0}{1}{2}", cod, Separador.Propiedad, nom);
            }


            LogicalThreadContext.Properties["user"] = usu;
        }

        public static void Action(string area, string controller, string action, string method)
        {
            var areaT = string.IsNullOrEmpty(area) ? "--------" : area;
            var controllerT = string.IsNullOrEmpty(controller) ? "--------" : controller;
            var actionT = string.IsNullOrEmpty(action) ? "--------" : action;
            var methodT = string.IsNullOrEmpty(method) ? "--------" : method;
            LogicalThreadContext.Properties["action"] = string.Format("{0}{1}{2}{3}{4}{5}{6}", areaT, Separador.Propiedad, controllerT, Separador.Propiedad, actionT, Separador.Propiedad, methodT, Separador.Propiedad);
        }


    }
}
