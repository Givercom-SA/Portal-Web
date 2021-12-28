using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsServices.Common.Util;

namespace WindowsServices.Common
{
    class ConsoleServiceHost<SERVICE>
    {
        private InnerService _consoleService = null;
        private HostConfiguration<SERVICE> _innerConfig = null;
        private ExitCode _exitCode = 0;
        private ManualResetEvent _exit = null;
        private volatile bool _hasCancelled = false;

        public ConsoleServiceHost(InnerService consoleService, HostConfiguration<SERVICE> innerConfig)
        {
            _consoleService = consoleService
                ?? throw new ArgumentNullException(nameof(consoleService));

            _innerConfig = innerConfig
                ?? throw new ArgumentNullException(nameof(innerConfig));
        }

        internal ExitCode Run()
        {
            AppDomain.CurrentDomain.UnhandledException += CatchUnhandledException;

            bool started = false;
            try
            {
                _exit = new ManualResetEvent(false);
                _exitCode = ExitCode.Ok;

                Console.Title = _consoleService.ServiceName;
                Console.CancelKeyPress += HandleCancelKeyPress;

                _consoleService.Start(_innerConfig.ExtraArguments.ToArray(), () => Logger.WriteLine("Deteniendo aplicación de consola"));
                started = true;

                _exit.WaitOne();
            }
            catch (Exception ex)
            {
                Logger.WriteLine(string.Format("Ocurrio una excepcion {0}", ex));

                return ExitCode.AbnormalExit;
            }
            finally
            {
                if (started)
                    StopService();

                _exit.Close();
                (_exit as IDisposable).Dispose();
            }

            return _exitCode;
        }

        internal void StopService()
        {
            try
            {
                if (_hasCancelled)
                    return;

                Task stopTask = Task.Run(() => _consoleService.Stop());
                if (!stopTask.Wait(TimeSpan.FromMilliseconds(_innerConfig.ServiceTimeout)))
                    throw new Exception("El servicio no puede detenerse");

                _exitCode = ExitCode.Ok;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(string.Format("Error , no se puede detener el servicio : {0}", ex.ToString()));
                _exitCode = ExitCode.AbnormalExit;
            }
            finally
            {
                Logger.WriteLine(string.Format("El servicio {0} ha sido detenido.", _consoleService.ServiceName));
                _exitCode = ExitCode.Ok;
            }
        }

        private void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey == ConsoleSpecialKey.ControlBreak)
            {
                Logger.WriteLine("Interrupción detectada (use Control + C para salir)");
                return;
            }

            e.Cancel = true;

            if (_hasCancelled)
                return;

            Logger.WriteLine("Control+C detectado, intentando detener el servicio.");
            Task stopTask = Task.Run(() => _consoleService.Stop());
            if (stopTask.Wait(TimeSpan.FromMilliseconds(_innerConfig.ConsoleTimeout)))
            {
                _hasCancelled = true;
                _exit.Set();
            }
            else
            {
                _hasCancelled = false;
                Logger.WriteLine("El servicio no se encuentra en un estado donde se puede detener.");
            }
        }

        private void CatchUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.WriteLine(string.Format("El servicio arrojó una excepción no manejada : {0}", e.ToString()));

            if (!e.IsTerminating)
                return;

            _exitCode = ExitCode.UnhandledServiceException;
            _exit.Set();
        }
    }
}
