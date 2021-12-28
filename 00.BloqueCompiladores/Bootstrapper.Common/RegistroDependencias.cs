using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Bootstrapper.Common.ResolucionDependencias.ModulosAutofac;
using Bootstrapper.Common.ResolucionDependencias.Servicios;
using Microsoft.Extensions.DependencyInjection;
using Security.Common;
using System;

namespace Bootstrapper.Common
{
    public class RegistroDependencias
    {
        private readonly IServiceCollection services;
        private ContainerBuilder container;

        public RegistroDependencias(IServiceCollection services)
        {
            this.services = services;
            this.container = new ContainerBuilder();
        }

        public CommandsModule CommandsModule
        {
            get { return new CommandsModule(this); }
        }

        public QuerysModule QueryModule
        {
            get { return new QuerysModule(this); }
        }

        public ServiceModule ServiceModule
        {
            get { return new ServiceModule(this); }
        }

        public BusinessLogicModule BusinessLogicModule
        {
            get { return new BusinessLogicModule(this); }
        }

        public IntegrationEventsModule IntegrationEventsModule
        {
            get { return new IntegrationEventsModule(this); }
        }

        public RepositoriesModule RepositoriesModule
        {
            get { return new RepositoriesModule(this); }
        }

        public EmailModule EmailModule
        {
            get { return new EmailModule(this); }
        }

        public EventBusService EventBusService
        {
            get { return new EventBusService(this, services); }
        }

        public RegistroDependencias RegisterValidation<T>(params Type[] types)
        {
            return this;
        }

        public RegistroDependencias RegisterEncryption<T>(string password) where T : IEncrypter
        {
            container.RegisterType(typeof(T)).As<IEncrypter>()
                .WithParameter("password", password);

            return this;
        }

        public IServiceProvider CreateServiceProvider()
        {
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        public void RegisterAutofacModule(IModule module)
        {
            container.RegisterModule(module);
        }
    }
}
