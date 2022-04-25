using FluentScheduler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Common.HostBuilder;
using Servicio.Embarque.BusinessLogic;
using Servicio.Embarque.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = HostBaseBuilder<Startup>.GenericBuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                        JobManager.JobException += (obj) => { logger.LogError(obj.Exception.Message); };
                        JobManager.Initialize(new JobRegistry(services.GetRequiredService<ProcesoBusinessLogic>()));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ocurrió un error al cargar los parámetros iniciales.");
                    throw ex;
                }
            }

            host.Run();
        }


    }
}


