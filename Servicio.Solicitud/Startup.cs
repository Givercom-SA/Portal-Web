using Bootstrapper.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Service.Common.HostBuilder;
using Service.Common.Logging.Extensions;
using Servicio.Solicitud.Repositorio;
using Servicio.Solicitud.ServiceConsumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using log4netConfiguration = Service.Common.Logging.Appenders.Configuration;

namespace Servicio.Solicitud
{
    public class Startup : BaseBuilder
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public System.IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ISolicitudRepository, MsSqlSolicitud>();
            services.AddSingleton<ServicioMessage>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicio.Solicitud", Version = "v1" });
            });

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
          .Register().CreateServiceProvider()
          ;

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Servicio.Solicitud v1"));
            }

            loggerFactory.AddLog4Net(new[] { log4netConfiguration.CreateRollingFileAppender(Configuration) });
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
