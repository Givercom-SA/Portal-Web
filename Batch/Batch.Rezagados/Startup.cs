using AutoMapper;
using Bootstrapper.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Extensions;
using System;
using log4netConfiguration = Service.Common.Logging.Appenders.Configuration;

namespace Batch.Correo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
          

            var serviceProvider = new RegistroDependencias(services)
                .QueryModule
                    .UseConnectionStringPCR(Configuration.GetConnectionString("mssqldb"))
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .Register()
                .CommandsModule
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .Register()
                .ServiceModule
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .UseServiceConfiguration(Configuration)
                    .Register()
                 .BusinessLogicModule
             
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .UseConfiguration(Configuration)
                    .Register()
                .CreateServiceProvider();

            return serviceProvider;                    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IServiceProvider serviceProvider,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net(new[] { log4netConfiguration.CreateRollingFileAppender(Configuration) });

        }
    }
}

