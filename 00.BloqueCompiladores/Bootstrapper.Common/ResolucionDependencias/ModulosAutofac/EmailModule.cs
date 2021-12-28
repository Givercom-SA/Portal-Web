using Autofac;
using Email.Common;

namespace Bootstrapper.Common.ResolucionDependencias.ModulosAutofac
{
    public class EmailModule : Autofac.Module
    {
        private string smtpServer;
        private int port;
        private bool ssl;
        private string user;
        private string password;
        private string emailSender;

        private readonly RegistroDependencias dependencyRegistrar;

        public EmailModule(RegistroDependencias dependencyRegistrar)
        {
            this.dependencyRegistrar = dependencyRegistrar;
        }

        public EmailModule UseSMTPServer(string smtpServer)
        {
            this.smtpServer = smtpServer;
            return this;
        }

        public EmailModule UsePort(int port)
        {
            this.port = port;
            return this;
        }

        public EmailModule UseSSL(bool ssl)
        {
            this.ssl = ssl;
            return this;
        }

        public EmailModule UseUser(string user)
        {
            this.user = user;
            return this;
        }

        public EmailModule UsePassword(string password)
        {
            this.password = password;
            return this;
        }

        public EmailModule UseEmailSender(string emailSender)
        {
            this.emailSender = emailSender;
            return this;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailSMTP>()
                .WithParameter("smtpServer", smtpServer)
                .WithParameter("port", port)
                .WithParameter("ssl", ssl)
                .WithParameter("user", user)
                .WithParameter("password", password)
                .WithParameter("sender", emailSender)
                .SingleInstance();
        }

        public RegistroDependencias Register()
        {
            dependencyRegistrar.RegisterAutofacModule(this);
            return dependencyRegistrar;
        }
    }
}
