using AutoMapper;
using Bootstrapper.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Common.HostBuilder;
using Service.Common.Logging.Extensions;
using Servicio.Notificacion.Subscription;
using Servicio.Notificacion.Subscription.Web;
using Servicio.Notificacion.Util;
using System.Globalization;
using System.Text;
using log4netConfiguration = Service.Common.Logging.Appenders.Configuration;

namespace Servicio.Notificacion
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
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicio.Notificacion", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       //ValidIssuer = Configuration["Jwt:Issuer"],
                       //ValidAudience = Configuration["Jwt:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                   };
               });

            
            services.AddAutoMapper(typeof(Startup));


            //services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddSingleton<WebClient, WebClient>();
            services.AddSingleton<NotificacionSubscription, NotificacionSubscription>();

            var serviceProvider = new RegistroDependencias(services)
                .QueryModule
                    .UseConnectionStringPCR(Configuration.GetConnectionString("mssqldb"))
                    //.UseConnectionStringAfiliado(Configuration.GetConnectionString("mssqldb.Afiliado"))
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .Register()
                .ServiceModule
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .UseServiceConfiguration(Configuration)
                    .Register()
                 .BusinessLogicModule
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .Register()
                .CreateServiceProvider();

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifeTime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net(new[] { log4netConfiguration.CreateRollingFileAppender(Configuration) });

            lifeTime = LifeTimeBuilder.LifeTimeRegister(lifeTime, env.ApplicationName);

            var defaultCulture = "es-PE";
            var ci = new CultureInfo(defaultCulture);
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            var supportedCultures = new[] { ci };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es-PE"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseAuthentication();

          

            app.UseSignalRClient<WebClient>(Configuration.GetSection(AppSettingsKeys.ConnectionStrings.AFPnetHub));
            app.UseSqlTableDependency<NotificacionSubscription>(Configuration[AppSettingsKeys.ConnectionStrings.AFPnetServiceBroker]);

            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
