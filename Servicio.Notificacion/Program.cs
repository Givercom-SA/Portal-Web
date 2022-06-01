using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Common.HostBuilder;
namespace Servicio.Notificacion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = HostBaseBuilder<Startup>.GenericBuildWebHost(args);
            host.Run();
        }
    }
}
