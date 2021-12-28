using Microsoft.Extensions.Logging;
using Service.Common.Logging.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsServices.Common.Util
{

    public class Logger
    {
        private static ILogger _logger = ApplicationLogging.CreateLogger("Logger");

        public static void WriteLine(string mensaje)
        {
            
            Console.WriteLine(mensaje);
            _logger.LogInformation(mensaje);
        }
    }
}
