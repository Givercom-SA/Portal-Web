using Autofac;
using CommandHandlers.Common;
using System.Reflection;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class CommandsModule : Autofac.Module
    {
        private Assembly assemblyTypeToRegister;
        private readonly RegistroDependencias dependencyRegistrar;

        public CommandsModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        public CommandsModule UseAssemblyTypes(Assembly assemblyTypeToRegister)
        {
            this.assemblyTypeToRegister = assemblyTypeToRegister;
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var ss = builder.RegisterAssemblyTypes(assemblyTypeToRegister)
                .Where(t => t.Name.EndsWith("Handler"));

            builder.RegisterAssemblyTypes(assemblyTypeToRegister)
                .Where(t => t.Name.EndsWith("Handler"))
                .AsClosedTypesOf(typeof(ICommandHandler<>));

            builder.RegisterType<CommandDispatcher>().SingleInstance();
        }
        
        public RegistroDependencias Register()
        {
            dependencyRegistrar.RegisterAutofacModule(this);
            return dependencyRegistrar;
        }
    }
}
