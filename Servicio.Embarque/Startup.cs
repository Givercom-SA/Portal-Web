using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Servicio.Embarque.Repositorio;
using Servicio.Embarque.ServiceConsumer;
using Servicio.Embarque.ServiceExterno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IAsignarAgenteRepository, MsSqlAsignarAgenteRepository>();
            services.AddScoped<IDireccionamientoRepository, MsSqlDireccionamientoRepository>();
            services.AddScoped<IMemoRepository, MsSqlMemoRepository>();
            services.AddScoped<INotificacionArriboRepository, MsSqlNotificacionArriboRepository>();
            services.AddScoped<ICobroPagarRepository, MsSqlCobroPagarRepository>();
            services.AddSingleton<INotificacionArriboRepository, MsSqlNotificacionArriboRepository>();
            services.AddSingleton<IMemoRepository, MsSqlMemoRepository>();
            services.AddSingleton<ISolicitudFacturacionRepository, MsSqlSolicitudFacturacionRepository>();

            services.AddSingleton<ServicioUsuario>();
            services.AddSingleton<ServicioEmbarques>();
            services.AddSingleton<ServicioMessage>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicio.Embarque", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Servicio.Embarque v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
