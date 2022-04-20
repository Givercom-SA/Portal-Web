﻿using FluentScheduler;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using Servicio.Embarque.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Jobs
{
    public class FacturaJob : IJob
    {
        private readonly ProcesoBusinessLogic procesoBusinessLogic;
        private static ILogger _logger = ApplicationLogging.CreateLogger("FacturacionJob");
        public FacturaJob(ProcesoBusinessLogic _procesoBusinessLogic)
        {
            this.procesoBusinessLogic = _procesoBusinessLogic;

        }

        public void Execute()
        {
            try
            {
                procesoBusinessLogic.FacturacionJobAsync().Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}
