using Autofac;
using QueryHandlers.Common;
using System.Reflection;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class QuerysModule : Autofac.Module
    {
        private string connectionStringPRC;
        private string connectionStringAfiliado;

        private Assembly assemblyTypeToRegister;
        private readonly RegistroDependencias dependencyRegistrar;

        public QuerysModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        public QuerysModule UseConnectionStringPCR(string connectionString)
        {
            this.connectionStringPRC = connectionString;
            return this;
        }

        public QuerysModule UseConnectionStringAfiliado(string connectionString)
        {
            this.connectionStringAfiliado = connectionString;
            return this;
        }

        public QuerysModule UseAssemblyTypes(Assembly assemblyTypeToRegister)
        {
            this.assemblyTypeToRegister = assemblyTypeToRegister;
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(assemblyTypeToRegister)
                .Where(t => t.Name.EndsWith("Query"))
                .AsClosedTypesOf(typeof(IQueryHandler<>))
                .WithParameter("connectionStringPCR", connectionStringPRC)
                .WithParameter("connectionStringAfiliado", connectionStringAfiliado);

            builder.RegisterType<QueryDispatcher>().SingleInstance();
        }

        public RegistroDependencias Register()
        {
            dependencyRegistrar.RegisterAutofacModule(this);
            return dependencyRegistrar;
        }
    }
}
