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

namespace Servicio.Message
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var host = HostBaseBuilder<Startup>.GenericBuildWebHost(args);

            //using (var scope = host.Services.CreateScope())
            //{
            //  //  var services = scope.ServiceProvider;
            //    //var param = services.GetRequiredService<ParametrosDataCompletation>();
            //   // var logger = services.GetRequiredService<ILogger<Program>>();

            //    try
            //    {
            //       // param.CargarAsync().Wait();

            //        //if (DatosGeneralesDto.Cargado)
            //        //{
            //        //    JobManager.JobException += (obj) => { logger.LogError(obj.Exception.Message); };
            //        //    JobManager.Initialize(new JobRegistry(
            //        //        services.GetRequiredService<AfiliacionBusinessLogic>()
            //        //    ));
            //        //}

            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex, "Ocurrió un error al cargar los parámetros iniciales.");
            //        throw ex;
            //    }
            //}

            host.Run();
        }
    }
       
}
