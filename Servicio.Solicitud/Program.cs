using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Common.HostBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud
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
