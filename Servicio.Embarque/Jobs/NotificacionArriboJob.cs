using FluentScheduler;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using Servicio.Embarque.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Jobs
{
    public class NotificacionArriboJob : IJob
    {
        private readonly ProcesoBusinessLogic procesoBusinessLogic;
        private static ILogger _logger = ApplicationLogging.CreateLogger("NotificacionArriboJob");
        public NotificacionArriboJob(ProcesoBusinessLogic _procesoBusinessLogic)
        {
            this.procesoBusinessLogic = _procesoBusinessLogic;

        }

        public void Execute()
        {
            try
            {
                procesoBusinessLogic.NotificacionArriboAsync().Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
