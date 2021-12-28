
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using WindowsServices.Common.Cmd;
using WindowsServices.Common.Enums;
using WindowsServices.Common.StatesMachines;
using WindowsServices.Common.Win32ServiceUtils;
using WindowsServices.Common.Util;
namespace WindowsServices.Common
{
    public static class ServiceRunner<SERVICE>
    {
        public static int Run(Action<HostConfigurator<SERVICE>> runAction, ActionEnum action = ActionEnum.Run)
        {
            Directory.SetCurrentDirectory(PlatformServices.Default.Application.ApplicationBasePath);
           
            var innerConfig = new HostConfiguration<SERVICE>();
            innerConfig.Action = action;
            innerConfig.ExtraArguments = Parser.Parse(config =>
            {
                config.AddParameter(new CmdArgParam()
                {
                    Key = "username",
                    Description = "Nombre de usuario para la cuenta de servicio",
                    Value = val =>
                    {
                        innerConfig.Username = val;
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "password",
                    Description = "Password para la cuenta de servicio",
                    Value = val =>
                    {
                        innerConfig.Password = val;
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "built-in-account",
                    Description = "Password para la cuenta de servicio",
                    Value = val =>
                    {
                        switch (val.ToLower())
                        {
                            case "localsystem":
                                innerConfig.DefaultCred = Win32ServiceCredentials.LocalSystem;
                                break;
                            case "localservice":
                                innerConfig.DefaultCred = Win32ServiceCredentials.LocalService;
                                break;
                            case "networkservice":
                                innerConfig.DefaultCred = Win32ServiceCredentials.NetworkService;
                                break;
                            default:
                                innerConfig.DefaultCred = Win32ServiceCredentials.LocalSystem;
                                break;
                        }

                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "start-immediately",
                    Description = "Inicia el servicio inmediatamente despues de instalar.",
                    Value = val =>
                    {
                        if (bool.TryParse(val, out var startImmediately))
                        {
                            innerConfig.StartImmediately = startImmediately;
                        }
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "name",
                    Description = "Nombre del Servicio.",
                    Value = val =>
                    {
                        innerConfig.Name = val;
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "description",
                    Description = "Descripcíon del Servicio.",
                    Value = val =>
                    {
                        innerConfig.Description = val;
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "display-name",
                    Description = "Nombre para mostrar del servicio",
                    Value = val =>
                    {
                        innerConfig.DisplayName = val;
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "command",
                    Description = "Envia un comando personalizado al servicio.",
                    Value = val =>
                    {
                        if (int.TryParse(val, out var command))
                        {
                            innerConfig.CustomCommand = command;
                        }
                    }
                });
                config.AddParameter(new CmdArgParam()
                {
                    Key = "action",
                    Description = "Acciones que permite la consola.",
                    Value = val =>
                    {
                        switch (val)
                        {
                            case "install":
                                innerConfig.Action = ActionEnum.Install;
                                break;
                            case "pause":
                                innerConfig.Action = ActionEnum.Pause;
                                break;
                            case "continue":
                                innerConfig.Action = ActionEnum.Continue;
                                break;
                            case "start":
                                innerConfig.Action = ActionEnum.Start;
                                break;
                            case "stop":
                                innerConfig.Action = ActionEnum.Stop;
                                break;
                            case "uninstall":
                                innerConfig.Action = ActionEnum.Uninstall;
                                break;
                            case "run":
                                innerConfig.Action = ActionEnum.Run;
                                break;
                            case "run-interactive":
                                innerConfig.Action = ActionEnum.RunInteractive;
                                break;
                            case "custom-command":
                                innerConfig.Action = ActionEnum.CustomCommand;
                                break;
                            default:
                                Logger.WriteLine("{0} no se reconoce la acción, se ejecutará la aplicación de consola");
                                innerConfig.Action = ActionEnum.RunInteractive;
                                break;
                        }
                    }
                });

                config.UseDefaultHelp();
                config.UseAppDescription("Aplicacion de prueba.");
            });

            if (string.IsNullOrEmpty(innerConfig.Name))
            {
                innerConfig.Name = typeof(SERVICE).FullName;
            }

            if (string.IsNullOrEmpty(innerConfig.DisplayName))
            {
                innerConfig.DisplayName = innerConfig.Name;
            }

            if (string.IsNullOrEmpty(innerConfig.Description))
            {
                innerConfig.Description = "Sin descripcion";
            }

            var hostConfiguration = new HostConfigurator<SERVICE>(innerConfig);

            try
            {
                runAction(hostConfiguration);
                if (innerConfig.Action == ActionEnum.Run)
                    innerConfig.Service = innerConfig.ServiceFactory(innerConfig.ExtraArguments,
                        new MicroServiceController(() =>
                        {
                            var task = Task.Factory.StartNew(() =>
                            {
                                UsingServiceController(innerConfig, (sc, cfg) => StopService(cfg, sc));
                            });
                        }
                    ));
                else if (innerConfig.Action == ActionEnum.RunInteractive)
                {
                    var consoleService = new InnerService(innerConfig.Name, () => Start(innerConfig), () => Stop(innerConfig), () => Shutdown(innerConfig));
                    var consoleHost = new ConsoleServiceHost<SERVICE>(consoleService, innerConfig);

                    innerConfig.Service = innerConfig.ServiceFactory(innerConfig.ExtraArguments,
                        new MicroServiceController(() =>
                        {
                            var task = Task.Factory.StartNew(() =>
                            {
                                consoleHost.StopService();
                            });
                        }
                    ));

                    
                    return (int)consoleHost.Run();
                }
                else
                {
                    innerConfig.Service = innerConfig.ServiceFactory(innerConfig.ExtraArguments, null);
                }

                ConfigureService(innerConfig);
               
                return 0;
            }
            catch (Exception e)
            {
                Error(innerConfig, e);
                return -1;
            }
        }

        private static string GetServiceCommand(List<string> extraArguments)
        {
            var host = Process.GetCurrentProcess().MainModule.FileName;
            if (host.EndsWith("dotnet.exe", StringComparison.OrdinalIgnoreCase))
            {
                var appPath = Path.Combine(System.AppContext.BaseDirectory,
                    System.Reflection.Assembly.GetEntryAssembly().GetName().Name + ".dll");
                host = string.Format("{0} \"{1}\"", SanitiseArgument(host), appPath);
            }
            else
            {
                
                extraArguments = extraArguments.Skip(1).ToList();
            }
            var fullServiceCommand = string.Format("{0} {1} {2}", host, string.Join(" ", extraArguments.Select(arg => SanitiseArgument(arg))), "action:run");
            return fullServiceCommand;
        }

        private static string SanitiseArgument(string arg)
        {
            if (string.IsNullOrEmpty(arg))
            {
                return arg;
            }
            return arg.Contains(" ") ? "\"" + arg + "\"" : arg;
        }

        private static void Install(HostConfiguration<SERVICE> config, ServiceController sc, int counter = 0)
        {
            Win32ServiceCredentials cred = config.DefaultCred;
            if (!string.IsNullOrEmpty(config.Username))
            {
                cred = new Win32ServiceCredentials(config.Username, config.Password);
            }
            try
            {
                new Win32ServiceManager().CreateService(new ServiceDefinition(config.Name, GetServiceCommand(config.ExtraArguments))
                {
                    DisplayName = config.DisplayName,
                    Description = config.Description,
                    Credentials = cred,
                    AutoStart = true,
                    DelayedAutoStart = !config.StartImmediately,
                    ErrorSeverity = ErrorSeverity.Normal
                });
                Logger.WriteLine($@"Servicio ""{config.Name}"" instalado correctamente.");
            }
            catch (Exception)
            {
                throw;
                //Logger.WriteLine(e.Message);
                //if (e.Message.Contains("Ya existe"))
                //{
                //    Logger.WriteLine($@"Servicio ""{config.Name}"" (""{config.Description}"") ya estaba instalado. Reinstalando...");
                //    Reinstall(config, sc);
                //}
                //else if (e.Message.Contains("El servicio especificado ha sido marcado para su eliminación."))
                //{
                //    if (counter < 10)
                //    {
                //        System.Threading.Thread.Sleep(500);
                //        counter++;
                //        string suffix = "th";
                //        if (counter == 1)
                //        {
                //            suffix = "st";
                //        }
                //        else if (counter == 2)
                //        {
                //            suffix = "nd";
                //        }
                //        else if (counter == 3)
                //        {
                //            suffix = "rd";
                //        }
                //        Logger.WriteLine(string.Format("El servicio especificado ha sido marcado para su eliminación. Reintentando {0}{1} veces", counter, suffix));
                //        Install(config, sc, counter);
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
                //else
                //{
                //    throw;
                //}
            }
        }

        private static void Uninstall(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            try
            {
                if (!(sc.Status == ServiceControllerStatus.Stopped || sc.Status == ServiceControllerStatus.StopPending))
                {
                    StopService(config, sc);
                }
                new Win32ServiceManager().DeleteService(config.Name);
                Logger.WriteLine($@"Servicio ""{config.Name}"" desinstalado correctamente");
                config.OnServiceUnInstall?.Invoke(config.Service);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("does not exist"))
                {
                    throw;
                }
                Logger.WriteLine($@"Servicio ""{config.Name}"" no existe. No se realiza ninguna accion.");
            }
        }

        private static void StopService(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            if (!(sc.Status == ServiceControllerStatus.Stopped | sc.Status == ServiceControllerStatus.StopPending))
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(1000));
                Logger.WriteLine($@"Servicio ""{config.Name}"" detenido correctamente");
                config.OnServiceStop?.Invoke(config.Service);
            }
            else
            {
                Logger.WriteLine($@"Servicio ""{config.Name}"" ya esta detenido o esta deteniendose.");
            }
        }

        private static void PauseService(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            if (!(sc.Status == ServiceControllerStatus.Paused | sc.Status == ServiceControllerStatus.PausePending))
            {
                sc.Pause();
                sc.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromMilliseconds(1000));
                Logger.WriteLine($@"Servicio ""{config.Name}"" pausado correctamente");
                config.OnServicePause?.Invoke(config.Service);
            }
            else
            {
                Logger.WriteLine($@"Servicio ""{config.Name}"" (""{config.Description}"") ya esta pausado o esta pausandose.");
            }
        }

        private static void ContinueService(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            if (!(sc.Status == ServiceControllerStatus.Running | sc.Status == ServiceControllerStatus.ContinuePending))
            {
                sc.Continue();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(1000));
                Logger.WriteLine($@"Se continua con la ejecucion del servicio ""{config.Name}""");
                config.OnServiceContinue?.Invoke(config.Service);
            }
            else
            {
                Logger.WriteLine($@"Servicio ""{config.Name}"" ya esta detenido o esta deteniendose.");
            }
        }

        private static void StartService(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            if (!(sc.Status == ServiceControllerStatus.StartPending | sc.Status == ServiceControllerStatus.Running))
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(1000));
                Logger.WriteLine($@"Servicio ""{config.Name}"" iniciado correctamente");
            }
            else
            {
                Logger.WriteLine($@"Servicio ""{config.Name}"" ya esta iniciado o esta pendiente de iniciar.");
            }
        }

        private static void Reinstall(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            StopService(config, sc);
            Uninstall(config, sc);
            Install(config, sc);
        }

        private static void SendCustomCommandToService(HostConfiguration<SERVICE> config, ServiceController sc)
        {
            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.ExecuteCommand(config.CustomCommand);
                Logger.WriteLine($@"Envio satisfactorio de comando personalizado ({config.CustomCommand}) al servicio ""{config.Name}"" (""{config.Description}"")");
                config.OnServiceCustomCommand(config.Service, config.CustomCommand);
            }
            else
            {
                Logger.WriteLine($@"Servicio ""{config.Name}"" (""{config.Description}"") no se está ejecutando, no se puede enviar un comando personalizado ({config.CustomCommand})");
            }
        }

        private static void ConfigureService(HostConfiguration<SERVICE> config)
        {
            switch (config.Action)
            {
                case ActionEnum.Install:
                    UsingServiceController(config, (sc, cfg) => Install(cfg, sc));
                    break;
                case ActionEnum.Pause:
                    UsingServiceController(config, (sc, cfg) => PauseService(cfg, sc));
                    break;
                case ActionEnum.Continue:
                    UsingServiceController(config, (sc, cfg) => ContinueService(cfg, sc));
                    break;
                case ActionEnum.Uninstall:
                    UsingServiceController(config, (sc, cfg) => Uninstall(cfg, sc));
                    break;
                case ActionEnum.Run:
                    var testService = new InnerService(config.Name, () => Start(config), () => Stop(config), () => Shutdown(config));
                    var serviceHost = new Win32ServiceHost(config.Name, new ShutdownableServiceStateMachine(testService));
                    serviceHost.Run();
                    break;
                case ActionEnum.RunInteractive:
                    break;
                case ActionEnum.Stop:
                    UsingServiceController(config, (sc, cfg) => StopService(cfg, sc));
                    break;
                case ActionEnum.Start:
                    UsingServiceController(config, (sc, cfg) => StartService(cfg, sc));
                    break;
                case ActionEnum.CustomCommand:
                    UsingServiceController(config, (sc, cfg) => SendCustomCommandToService(cfg, sc));
                    break;
            }
        }

        private static void UsingServiceController(HostConfiguration<SERVICE> config, Action<ServiceController, HostConfiguration<SERVICE>> action)
        {
            using (var sc = new ServiceController(config.Name))
            {
                action(sc, config);
            }
        }

        private static void Start(HostConfiguration<SERVICE> config)
        {
            try
            {
                config.OnServiceStart(config.Service, config.ExtraArguments);
            }
            catch (Exception e)
            {
                Error(config, e);
            }
        }

        private static void Stop(HostConfiguration<SERVICE> config)
        {
            try
            {
                config.OnServiceStop(config.Service);
            }
            catch (Exception e)
            {
                Error(config, e);
            }
        }

        private static void Shutdown(HostConfiguration<SERVICE> config)
        {
            try
            {
                config.OnServiceShutdown(config.Service);
            }
            catch (Exception e)
            {
                Error(config, e);
            }
        }

        private static void Error(HostConfiguration<SERVICE> config, Exception e = null)
        {
            config.OnServiceError(e);
        }
    }
}
