using Autofac;
using CommandHandlers.Common;
using EventBus.Common.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class IntegrationEventsModule : Autofac.Module
    {
        private Assembly assemblyTypeToRegister;
        private IConfiguration configuration;

        private readonly RegistroDependencias dependencyRegistrar;

        public IntegrationEventsModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        public IntegrationEventsModule UseConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
            return this;
        }

        public IntegrationEventsModule UseAssemblyTypes(Assembly assemblyTypeToRegister)
        {
            this.assemblyTypeToRegister = assemblyTypeToRegister;
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(assemblyTypeToRegister)
                .Where(t => t.Name.EndsWith("Handler"))
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>))
                .WithParameter(new TypedParameter(typeof(IConfiguration), configuration));

            builder.RegisterType<CommandDispatcher>().SingleInstance();
        }

        public RegistroDependencias Register()
        {
            dependencyRegistrar.RegisterAutofacModule(this);
            return dependencyRegistrar;
        }
    }
}
