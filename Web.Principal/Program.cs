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
using Utilitario.Constante;
using ViewModel.Notificacion;

namespace Web.Principal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = HostBaseBuilder<Startup>.GenericBuildWebHost(args);

            using (var scope=host.Services.CreateScope()) {

                var services = scope.ServiceProvider;
                var cache = services.GetRequiredService<IMemoryCache>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                try {

                    cache.GetOrCreate(SistemaConstante.Cache.Notificaciones, s =>
                    {
                        return new Dictionary<int, List<NotificacionVM>>();
                    });
                }
                catch (Exception err) {

                    logger.LogError(err,"Ocurrio un error al cargar los parametros iniciales");

                    throw err;
                }

            }


            host.Run();

        }

     
    }
}
