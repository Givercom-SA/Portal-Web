using Bootstrapper.Common;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Security.Common;
using Service.Common.HostBuilder;
using Service.Common.Logging.Extensions;
using Service.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.ServiceConsumer;
using Web.Principal.ServiceExterno;
using Web.Principal.Utils;
using static Web.Principal.Utils.AppSettingsKeys;
using log4netConfiguration = Service.Common.Logging.Appenders.Configuration;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Web.Principal.Hubs;
using Web.Principal.Interface;

namespace Web.Principal
{
    public class Startup :BaseBuilder
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

            services.AddMvc()
                .AddRazorPagesOptions(option => option.Conventions.AddPageRoute("/Account/Login", ""))
                .AddSessionStateTempDataProvider();


            // Liminar carga de archivos
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = Convert.ToInt64(Configuration[AppSettingsKeys.RequestsConfig.MultipartBodyLengthLimit]);
            });

            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                //options.CookieHttpOnly = true;
                options.IdleTimeout = TimeSpan.FromSeconds(Convert.ToInt32(Configuration["Session:TimeExpired"]));
                options.Cookie.Path = "/";
                options.Cookie.Name = "CoreSessionDemo";
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifeTime)
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


            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            

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
                    pattern: "{area:exists}/{controller=SolicitarAcceso}/{action=ValidarCodigo}"
                );

                endpoints.MapAreaControllerRoute(
               name: "Embarques",
               areaName: "GestionarEmbarques",
               pattern: "{area=GestionarEmbarques}/{controller=Embarque}/{action=Buscar}"
                    );

                endpoints.MapAreaControllerRoute(
                    name: "Solicitudes",
                    areaName: "GestionarSolicitudes",
                    pattern: "{area=GestionarSolicitudes}/{controller=Solicitudes}/{action=Inicio}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "Dashboards",
                    areaName: "GestionarDashboards",
                    pattern: "{area=GestionarDashboards}/{controller=Inicio}/{action=Home}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                                 name: "Usuarios",
                                 areaName: "GestionarUsuarios",
                                 pattern: "{area=GestionarUsuarios}/{controller=Usuarios}/{action=ListarUsuarios}/{id?}"
                             );


                endpoints.MapAreaControllerRoute(
                         name: "GestionarAccesos",
                         areaName: "GestionarAccesos",
                         pattern: "{area=GestionarAccesos}/{controller=SolicitarAcceso}/{action=Index}/{id?}"
                           );


                endpoints.MapAreaControllerRoute(
                 name: "GestionarAutorizacion",
                 areaName: "GestionarAutorizacion",
                 pattern: "{area=GestionarAutorizacion}/{controller=Perfil}/{action=ListarPerfiles}/{id?}"
                   );

                endpoints.MapAreaControllerRoute(
                 name: "GestionarEmbarques",
                 areaName: "GestionarEmbarques",
                 pattern: "{area=GestionarEmbarques}/{controller=Embarque}/{action=Index}/{id?}"
                   );

                endpoints.MapAreaControllerRoute(
               name: "GestionarEmbarques",
               areaName: "GestionarEmbarques",
               pattern: "{area=GestionarEmbarques}/{controller=AsignarAgente}/{action=Lista}"
                    );

                endpoints.MapAreaControllerRoute(
               name: "GestionarEmbarques",
               areaName: "GestionarEmbarques",
               pattern: "{area=GestionarEmbarques}/{controller=Facturacion}/{action=Lista}"
                    );

            endpoints.MapAreaControllerRoute(
               name: "GestionarEmbarques",
               areaName: "GestionarEmbarques",
               pattern: "{area=GestionarEmbarques}/{controller=Direccionamiento}/{action=Lista}"
                    );

                endpoints.MapAreaControllerRoute(
               name: "GestionarEmbarques",
               areaName: "GestionarEmbarques",
               pattern: "{area=GestionarEmbarques}/{controller=Memo}/{action=Lista}"
                    );

                endpoints.MapControllerRoute(
                         name: "Seguridad",
                         pattern: "{controller=Seguridad}/{action=Index}"
                              );

                endpoints.MapControllerRoute(
                   name: "Autenticacion",
                   pattern: "{controller=Autenticacion}/{action=Index}"
                        );

     

                endpoints.MapControllerRoute(
                name: "Error",
                pattern: "{controller=Error}/{action=Index}"
                     );
                endpoints.MapControllerRoute(
            name: "Home",
            pattern: "{controller=Home}/{action=Index}"
                 );

                endpoints.MapControllerRoute(
                 name: "SolicitarAcceso",
                 pattern: "{SolicitarAcceso=Home}/{action=Index}"
              );

                //endpoints.MapHub<NotificationHub>("/NotificationHub");
                endpoints.MapHub<NotificationUserHub>("/NotificationUserHub");

            });



            loggerFactory.AddLog4Net(new[] { log4netConfiguration.CreateRollingFileAppender(Configuration, env.ApplicationName) });
            lifeTime = LifeTimeBuilder.LifeTimeRegister(lifeTime, env.ApplicationName);



        }

    }
}
