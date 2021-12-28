using EasyNetQ;
using Microsoft.Extensions.Logging;
using System;

namespace EventBus.EasyNetQ
{
    public class NetCoreLogger : IEasyNetQLogger
    {
        private readonly ILogger<NetCoreLogger> logger;

        public NetCoreLogger(ILogger<NetCoreLogger> logger)
        {
            this.logger = logger;
        }

        public void DebugWrite(string format, params object[] args)
        {
            logger.LogDebug(format, args);
        }

        public void InfoWrite(string format, params object[] args)
        {
            logger.LogInformation(format, args);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            logger.LogError(format, args);
        }

        public void ErrorWrite(Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }
    }
}
