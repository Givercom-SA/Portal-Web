
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsServices.Common.Interfaces;
using WindowsServices.Common.Util;
using WindowsServices.Common.Win32ServiceUtils;

namespace WindowsServices.Common
{
    public class InnerService : IShutdownableWin32Service
    {
        string serviceName;
        Action onStart;
        Action onStopped;
        Action onShutdown;

        public InnerService(string serviceName, Action onStart, Action onStopped, Action onShutdown)
        {
            this.serviceName = serviceName;
            this.onStart = onStart;
            this.onStopped = onStopped;
            this.onShutdown = onShutdown;
        }

        public string ServiceName
        {
            get
            {
                return serviceName;
            }
        }

        public void Shutdown()
        {
            onShutdown();
        }

        public void Start(string[] startupArguments, ServiceStoppedCallback serviceStoppedCallback)
        {
            try
            {
                onStart();
                Logger.WriteLine(string.Format("El servicio {0} se ha iniciado correctamente.", serviceName));
            }
            catch (Exception)
            {
                onStopped();
                serviceStoppedCallback();
            }
        }

        public void Stop()
        {
            Logger.WriteLine(string.Format("El servicio {0} se ha detenido correctamente.", serviceName));
            onStopped();
        }
    }
}
