using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common.HostBuilder
{

    public class HostBaseBuilder<T> where T : BaseBuilder
    {
        public static IWebHost GenericBuildWebHost(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .ConfigureLogging(logging => {
                logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Trace);
                logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Trace);
            })
           .UseStartup<T>()
           //.UseKestrel(o => o.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20))
           //.UseKestrel()
           .UseIISIntegration()
           .Build();

        public static void Run(IWebHost host)
        {
            host.Run();
        }


    }
}
