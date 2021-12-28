using Autofac;
using BusinessLogic.Common;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class BusinessLogicModule : Autofac.Module
    {
        private IConfiguration configuration;
        private Assembly assemblyTypeToRegister;
        private readonly RegistroDependencias dependencyRegistrar;

        public BusinessLogicModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        public BusinessLogicModule UseConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
            return this;
        }

        public BusinessLogicModule UseAssemblyTypes(Assembly assemblyTypeToRegister)
        {
            this.assemblyTypeToRegister = assemblyTypeToRegister;
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(assemblyTypeToRegister)
                .Where(t => 
                    t.BaseType == typeof(IBusinessLogic) ||
                    t.BaseType == typeof(IBusinessValidation) ||
                    t.BaseType == typeof(IBusinessDataComplementation))
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
