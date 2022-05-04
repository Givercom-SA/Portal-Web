using FluentScheduler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Batch.Correo.BusinessLogic;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using MSMQ.Messaging;

namespace Batch.Correo
{
    class Program
    {
        static void Main(string[] args)
        {
                IServiceCollection serviceCollection = new ServiceCollection().AddLogging();

                var builder = new ConfigurationBuilder()
                    .SetBasePath("C:/sda/config/")
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var configuration = builder.Build();
                Startup application = new Startup(configuration);
                var serviceProvider = application.ConfigureServices(serviceCollection);
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                application.Configure(serviceProvider, loggerFactory);
                logger.LogInformation("Iniciando Batch.correo");
               
                //JobManager.JobException += (obj) => { Console.WriteLine(obj.Exception.Message); };
                //JobManager.Initialize(new JobRegistry(
                //    serviceProvider.GetRequiredService<RezagadosMainBusinessLogic>()
                //));
                //string id = args[0];
                string id = @"{8601899D-BC29-4925-B8C4-D2D1751AC1D3}\6148";
                id = id.Replace("{", "");
                id = id.Replace("}", "");
                // var result= new EnvioCorreoLogic().EnviarCorreo(@"f2fed0d1-7e48-4076-b338-5856b0b858e3\20510", ".\\private$\\tm_pdwac_correo");
                EnvioCorreoLogic.EnvioCorreoParameter envioCorreoParameter = new EnvioCorreoLogic.EnvioCorreoParameter();
                envioCorreoParameter.id =id;

                logger.LogInformation("ID Cola => "+ id);

                //envioCorreoParameter.id = @"f2fed0d1-7e48-4076-b338-5856b0b858e3\20517";
                envioCorreoParameter.cola = configuration[Utilitario.Constante.ConfiguracionConstante.Cola.Correo];
                envioCorreoParameter.EmailSender = configuration[Utilitario.Constante.ConfiguracionConstante.Correo.Email];
                envioCorreoParameter.EmailSenderHost = configuration[Utilitario.Constante.ConfiguracionConstante.Correo.SMTPServer];
                envioCorreoParameter.EmailSenderContrasenia = configuration[Utilitario.Constante.ConfiguracionConstante.Correo.Password];
                envioCorreoParameter.EmailSenderPuerto =Convert.ToInt32( configuration[Utilitario.Constante.ConfiguracionConstante.Correo.Port]);
                envioCorreoParameter.EmailSenderSSL =Convert.ToBoolean( configuration[Utilitario.Constante.ConfiguracionConstante.Correo.SSL]);
                envioCorreoParameter.CopiaOculta = configuration[Utilitario.Constante.ConfiguracionConstante.Correo.CopiaOculta]; 

               var resultEnvioCorreo= new EnvioCorreoLogic().EnviarCorreo(envioCorreoParameter);
                if (resultEnvioCorreo.Estado == "0")
                {
                    logger.LogInformation("Correo => " + resultEnvioCorreo.Correo);
                    logger.LogInformation("Asunto => " + resultEnvioCorreo.Asunto);

                }
                else {

                    throw  resultEnvioCorreo.exception;
                }
                

            }
            catch (Exception e)
            {
                logger.LogError(e,e.Message,null);

            }
            finally
            {
                logger.LogInformation("Finalizado Batch.correo");
            }

        }
    }
}

