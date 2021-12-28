using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Service.Common.Logging.Application;
using Service.Common.Logging.Extensions;

namespace Service.Common.BaseServiceController
{
    public class BaseController : Controller
    {
        #region Properties
        private IConfiguration configuration;
        private static ILogger _logger = ApplicationLogging.CreateLogger("BaseController");
        #endregion


        public BaseController(IConfiguration configuration) : base()
        {
            this.configuration = configuration;
        }

        public BaseController() : base()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("idunico", out StringValues idunico);
            Log4netExtensions.SetIdUnicoTransaccion(idunico);
            context.HttpContext.Request.Headers.TryGetValue("user", out StringValues user);
            Log4netExtensions.Usuario(user);
            var totalMemoriaInicial = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var totalMemoriaProceso = Process.GetCurrentProcess().PeakWorkingSet64 / 1024;
            var proceso = Process.GetCurrentProcess().ProcessName;
            var id = Process.GetCurrentProcess().Id;
            _logger.LogInformation(string.Format("Service|Ini|{0}|MEM USADA:{1}|MEM TOPE:{2}|PROC:{3}|ID:{4}|", context.HttpContext.Request.Path.Value, totalMemoriaInicial.ToString(), totalMemoriaProceso, proceso, id));
            base.OnActionExecuting(context);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var totalMemoriaFinal = Process.GetCurrentProcess().WorkingSet64 / 1024;
            var totalMemoriaProceso = Process.GetCurrentProcess().PeakWorkingSet64 / 1024;
            var proceso = Process.GetCurrentProcess().ProcessName;
            var id = Process.GetCurrentProcess().Id;
            _logger.LogInformation(string.Format("Service|Fin|{0}|MEM USADA:{1}|MEM TOPE:{2}|PROC:{3}|ID:{4}|", context.HttpContext.Request.Path.Value, totalMemoriaFinal.ToString(), totalMemoriaProceso, proceso, id));
            context.HttpContext.Response.Headers.Add("SERVER", string.Format("{0}",Utils.Server.GetLocalIPAddress()));

        }


    }
}
