using Autofac;
using EventBus.Common.Abstractions;
using EventBus.EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bootstrapper.Common.ResolucionDependencias.Servicios
{
    public class EventBusService
    {
        private readonly RegistroDependencias dependencyRegistrar;
        private readonly IServiceCollection services;
        private string eventBusConnectionString;
        private string exchange = "Desconocido";
        private string queue = "Desconocido";
        private string routingKey = "Desconocido";

        public EventBusService(RegistroDependencias dependencyRegistrar, IServiceCollection services)
        {
            this.dependencyRegistrar = dependencyRegistrar;
            this.services = services;
        }

        public EventBusService UseConnectionString(string eventBusConnectionString)
        {
            this.eventBusConnectionString = eventBusConnectionString;
            return this;
        }

        public EventBusService Setup(string exchange, string queue, string routingKey)
        {
            this.exchange = exchange;
            this.queue = queue;
            this.routingKey = routingKey;
            return this;
        }

        public RegistroDependencias Register()
        {
            services.AddSingleton<IEventBus, EventBusEasyNetQ>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<EventBusEasyNetQ>>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                return new EventBusEasyNetQ(eventBusConnectionString, logger, iLifetimeScope, exchange, queue, routingKey);
            });

            return dependencyRegistrar;
        }
    }
}
