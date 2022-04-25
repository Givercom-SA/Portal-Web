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
    public class MemoJob : IJob
    {
        private readonly ProcesoBusinessLogic procesoBusinessLogic;
        private static ILogger _logger = ApplicationLogging.CreateLogger("MemoJob");
        public MemoJob(ProcesoBusinessLogic _procesoBusinessLogic)
        {
            this.procesoBusinessLogic = _procesoBusinessLogic;

        }

        public void Execute()
        {
            try
            {
                //_logger.LogInformation("Job - Iniciando Memo");
                procesoBusinessLogic.MemoJobAsync().Wait();
                //_logger.LogInformation("Job - Finalizado Memo");
            }
            catch (Exception e)
            {
                //_logger.LogError(e, "MemoJob: " + e.Message);
            }
        }
    }
}
