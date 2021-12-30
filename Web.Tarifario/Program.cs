using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Common.HostBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Notificacion;

namespace Web.Tarifario
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = HostBaseBuilder<Startup>.GenericBuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {

                var services = scope.ServiceProvider;
                var cache = services.GetRequiredService<IMemoryCache>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {

                   
                }
                catch (Exception err)
                {

                    logger.LogError(err, "Ocurrio un error al cargar los parametros iniciales");

                    throw err;
                }

            }


            host.Run();

            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
