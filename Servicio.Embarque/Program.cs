using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.Common.HostBuilder;
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
            // CreateHostBuilder(args).Build().Run();
            var host = HostBaseBuilder<Startup>.GenericBuildWebHost(args);

            //JobManager.Initialize(new JobRegistry());

            host.Run();
        }

        //    public static IHostBuilder CreateHostBuilder(string[] args) =>
        //        Host.CreateDefaultBuilder(args)
        //            .ConfigureWebHostDefaults(webBuilder =>
        //            {
        //                webBuilder.UseStartup<Startup>();
        //            }).ConfigureServices(services => {
        //                services.AddHostedService<BackgroundNotificacionArribo>();
        //                services.AddHostedService<BackgroundMemo>();
        //                services.AddHostedService<BackgroundFacturar>();
        //            });
        //}
    }
}
