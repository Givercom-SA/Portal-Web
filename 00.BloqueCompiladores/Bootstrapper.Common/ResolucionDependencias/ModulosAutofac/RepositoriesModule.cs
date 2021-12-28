using Autofac;
using Data.Common;
using Domain.Common.Contracts;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class RepositoriesModule : Autofac.Module
    {
        private readonly RegistroDependencias dependencyRegistrar;

        public RepositoriesModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>))
                .As(typeof(IRepository<>)).InstancePerDependency();
        }

        public RegistroDependencias Register()
        {
            dependencyRegistrar.RegisterAutofacModule(this);
            return dependencyRegistrar;
        }
    }
}
