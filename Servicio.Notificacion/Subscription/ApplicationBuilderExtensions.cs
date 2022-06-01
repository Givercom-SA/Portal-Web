using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Servicio.Notificacion.Subscription.Web;
using System.Collections.Generic;

namespace Servicio.Notificacion.Subscription
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseSqlTableDependency<T>(this IApplicationBuilder services, string connectionString)
            where T : IDatabaseSubscription
        {
            var serviceProvider = services.ApplicationServices;
            var subscription = serviceProvider.GetService<T>();
            subscription.Configure(connectionString);
        }

        public static void UseSignalRClient<T>(this IApplicationBuilder services, IConfigurationSection connectionString)
            where T : IWebSubscription
        {

            var conexiones = connectionString.Get<List<string>>();

            var serviceProveider = services.ApplicationServices;
            var subscription = serviceProveider.GetService<T>();
            subscription.Configure(conexiones);
        }
    }
}
