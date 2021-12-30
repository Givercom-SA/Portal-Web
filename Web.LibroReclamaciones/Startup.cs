using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bootstrapper.Common;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Security.Common;
using Service.Common.HostBuilder;
using Service.Common.Logging.Extensions;
using Service.Common.Utils;
using System.Globalization;
using System.IO;


using log4netConfiguration = Service.Common.Logging.Appenders.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Web.LibroReclamaciones.Utilitario;

namespace Web.LibroReclamaciones
{
    public class Startup : BaseBuilder
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        //public System.IServiceProvider
        public IServiceProvider
          ConfigureServices(IServiceCollection services)
        {

            services.AddMemoryCache();

            services.AddRazorPages()
                .AddRazorRuntimeCompilation();

            services.AddControllers();

            services.AddMvc();

            // Liminar carga de archivos
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = Convert.ToInt64(Configuration[AppSettingsKeys.RequestsConfig.MultipartBodyLengthLimit]);
            });

            services.AddControllersWithViews();
            services.AddSignalR();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //options.CookieHttpOnly = true;
                options.IdleTimeout = TimeSpan.FromSeconds(Convert.ToInt32(Configuration["Session:TimeExpired"]));
                options.Cookie.Path = "/";
                options.Cookie.Name = "SessionTarifario";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.HttpOnly = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper(typeof(Startup));



            //services.AddScoped<ServicioAcceso>();
            //services.AddScoped<ServicioMaestro>();
            //services.AddScoped<ServicioSolicitud>();
            //services.AddScoped<ServicioUsuario>();
            //services.AddScoped<ServicioEmbarques>();
            //services.AddScoped<ServicioEmbarque>();
            //services.AddScoped<ServicioMessage>();

            services.AddSingleton<IFileProvider>(
          new PhysicalFileProvider(Directory.GetCurrentDirectory()));
            services.AddSingleton<IVersionService>(new VersionService());



            var serviceProvider = new RegistroDependencias(services)
                .ServiceModule
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .UseServiceConfiguration(Configuration)
                    .Register()
                .BusinessLogicModule
                    .UseAssemblyTypes(typeof(Startup).Assembly)
                    .UseConfiguration(Configuration)
                    .Register()

                .RegisterEncryption<Encrypter>(Configuration[AppSettingsKeys.Seguridad.LlaveEncriptamiento])
                .CreateServiceProvider();

            return serviceProvider;
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILoggerFactory loggerFactory, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifeTime)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error/PaginaError");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");



            //app.UseStatusCodePages("text/plain","Pagina de codigos de estado, codigo {0}");
            //app.UseStatusCodePages(async context => {
            //    await context.HttpContext.Response.WriteAsync(
            //        "Pagina de codigos de estado, codigo:" +
            //        context.HttpContext.Response.StatusCode);
            //});

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();




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


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();

                endpoints.MapControllerRoute(
                  name: "Projects",
                  pattern: "{area:exists}/{controller=Maestro}/{action=ValidarCodigo}"
              );
         

            });

            loggerFactory.AddLog4Net(new[] { log4netConfiguration.CreateRollingFileAppender(Configuration, env.ApplicationName) });
            lifeTime = LifeTimeBuilder.LifeTimeRegister(lifeTime, env.ApplicationName);

        }

    }
}
