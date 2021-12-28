using Autofac;
using Microsoft.Extensions.Configuration;
using Service.Common;
using System.Reflection;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class ServiceModule : Autofac.Module
    {
        private IConfiguration configuration;
        private Assembly assemblyTypeToRegister;
        private readonly RegistroDependencias dependencyRegistrar;

        public ServiceModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        public ServiceModule UseServiceConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
            return this;
        }

        public ServiceModule UseAssemblyTypes(Assembly assemblyTypeToRegister)
        {
            this.assemblyTypeToRegister = assemblyTypeToRegister;
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(assemblyTypeToRegister)
                //.Where(t => t.BaseType == typeof(IServiceConsumer) && (t.Name.EndsWith("Model") || t.Name.EndsWith("Service")))
                .Where(t => t.BaseType == typeof(IServiceConsumer) )
                .WithParameter(new TypedParameter(typeof(IConfiguration), configuration))
                .SingleInstance();
        }

        public RegistroDependencias Register()
        {
            dependencyRegistrar.RegisterAutofacModule(this);
            return dependencyRegistrar;
        }
    }
}
