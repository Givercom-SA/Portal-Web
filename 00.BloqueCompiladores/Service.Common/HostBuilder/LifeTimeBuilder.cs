using log4net;
using log4net.Appender;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using Service.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Service.Common.HostBuilder
{
    public class LifeTimeBuilder
    {
        private static ILogger _logger = ApplicationLogging.CreateLogger("LifeTimeBuilder");
        private static string _applicationName;
        public static IApplicationLifetime LifeTimeRegister(IApplicationLifetime lifeTime, string applicationName)
        {
            _applicationName = applicationName;
            lifeTime.ApplicationStarted.Register(OnAppStarted);

            lifeTime.ApplicationStopping.Register(OnAppStopping);

            lifeTime.ApplicationStopped.Register(OnAppStopped);
            return lifeTime;
        }

        private static void OnAppStarted()
        {
            var serviceOn = !string.IsNullOrEmpty(FileVersion.Version())? string.Format("Servicio {0} iniciado correctamente , version {1}", _applicationName, FileVersion.Version()): string.Format("Servicio {0} iniciado correctamente.", _applicationName);
            _logger.LogInformation(serviceOn);
        }


        private static void OnAppStopping()
        {
            _logger.LogInformation(string.Format("Servicio {0} esta deteniendose", _applicationName));
        }


        private static void OnAppStopped()
        {
            _logger.LogInformation(string.Format("Servicio {0} detenido correctamente", _applicationName));
            ((RollingFileAppender)LogManager.GetRepository(Assembly.GetExecutingAssembly()).GetAppenders().FirstOrDefault()).Close();
        }
    }
}
