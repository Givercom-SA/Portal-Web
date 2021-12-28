using System;
using WindowsServices.Common.Configurators.Service;

namespace WindowsServices.Common
{
    public class HostConfigurator<SERVICE>
    {
        HostConfiguration<SERVICE> innerConfig;
        public HostConfigurator(HostConfiguration<SERVICE> innerConfig)
        {
            this.innerConfig = innerConfig;
        }

        public void SetName(string serviceName, bool force = false)
        {
            if (!string.IsNullOrEmpty(innerConfig.Name) || force)
            {
                innerConfig.Name = serviceName;
            }
        }

        public void SetDisplayName(string displayName, bool force = false)
        {
            if (!string.IsNullOrEmpty(innerConfig.DisplayName) || force)
            {
                innerConfig.DisplayName = displayName;
            }
        }

        public void SetDescription(string description, bool force = false)
        {
            if (!string.IsNullOrEmpty(innerConfig.Description) || force)
            {
                innerConfig.Description = description;
            }
        }

        public void SetConsoleTimeout(int milliseconds)
        {
            innerConfig.ConsoleTimeout = milliseconds;
        }

        public void SetServiceTimeout(int milliseconds)
        {
            innerConfig.ServiceTimeout = milliseconds;
        }

        public string GetDefaultName()
        {
            return innerConfig.Name;
        }

        public bool IsNameNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(innerConfig.Name);
            }
        }

        public bool IsDescriptionNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(innerConfig.Description);
            }
        }

        public bool IsDisplayNameNullOrEmpty
        {
            get
            {
                return string.IsNullOrEmpty(innerConfig.DisplayName);
            }
        }

        public void Service(Action<ServiceConfigurator<SERVICE>> serviceConfigAction)
        {
            try
            {
                var serviceConfig = new ServiceConfigurator<SERVICE>(innerConfig);
                serviceConfigAction(serviceConfig);
                if (innerConfig.ServiceFactory == null)
                {
                    throw new ArgumentException("Es necesario configurar la acción que crea el servicio (ServiceFactory)");
                }

                if (innerConfig.OnServiceStart == null)
                {
                    throw new ArgumentException("Es necesario configurar la acción que se llama cuando se inicia el servicio.");
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("La configuración del servicio arrojó una excepción.", e);
            }
        }
    }
}
