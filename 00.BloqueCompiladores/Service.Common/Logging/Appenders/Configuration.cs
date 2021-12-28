using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Configuration;
using Service.Common.Logging.Layout;
using Service.Common.Logging.Parameters;
using System;
using System.Reflection;

namespace Service.Common.Logging.Appenders
{
    public static class Configuration
    {
        private const string DefaultFileName = "log";

        public static IAppender CreateConsoleAppender()
        {
            ConsoleAppender appender = new ConsoleAppender() { Name = "ConsoleAppender" };
            PatternLayout layout = new PatternLayout() { ConversionPattern = "%date [%thread] %-5level %logger - %message%newline" };
            layout.ActivateOptions();
            appender.Layout = layout;
            appender.ActivateOptions();
            return appender;
        }

        public static IAppender CreateTraceAppender()
        {
            TraceAppender appender = new TraceAppender() { Name = "TraceAppender" };
            PatternLayout layout = new PatternLayout() { ConversionPattern = "-%-5level - %date - %property{user} - %property{idunico} - %property{action} - %message%newline" };
            layout.ActivateOptions();
            appender.Layout = layout;
            appender.ActivateOptions();


            ((Logger)LogManager.GetRepository(Assembly.GetExecutingAssembly()).GetLogger("Microsoft")).Level = log4net.Core.Level.Off;

            return appender;
        }

        public static IAppender CreateRollingFileAppender(IConfiguration configuration,string application = null)
        {
            var fileAppender = new RollingFileAppender
            {
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                DatePattern = ".yyyyMMdd_HH.'txt'",
                File = string.Format(File(configuration),DateTime.Now.ToString("yyyyMMdd")  ),
                StaticLogFileName = false,
                Layout = LayoutFactory.Create(application),
                AppendToFile = true,
                MaximumFileSize = "20MB",
                MaxSizeRollBackups = -1
            };
            
            fileAppender.ActivateOptions();
            ((Logger)LogManager.GetRepository(Assembly.GetExecutingAssembly()).GetLogger("Microsoft")).Level = log4net.Core.Level.Off;
            ((Logger)LogManager.GetRepository(Assembly.GetExecutingAssembly()).GetLogger("Microsoft.AspNetCore.Diagnostics")).Level = log4net.Core.Level.All;
            ((Logger)LogManager.GetRepository(Assembly.GetExecutingAssembly()).GetLogger("Microsoft.AspNetCore.SignalR")).Level = log4net.Core.Level.Error;
            ((Logger)LogManager.GetRepository(Assembly.GetExecutingAssembly()).GetLogger("Microsoft.AspNetCore.Http.Connections")).Level = log4net.Core.Level.Error;
            return fileAppender;
        }



        public static IAppender CreateAdoNetAppender(string connectionString)
        {
            AdoNetAppender appender = new AdoNetAppender()
            {
                Name = "AdoNetAppender",
                BufferSize = 1,
                ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version = 1.0.3300.0, Culture = neutral, PublicKeyToken = b77a5c561934e089",
                ConnectionString = connectionString,
                CommandText = "INSERT INTO Log ([Date],[Thread],[Level],[Logger],[Message],[Exception]) VALUES (@log_date, @thread, @log_level, @logger, @message, @exception)"
            };

            AddDateTimeParameterToAppender(appender, "@log_date");
            AddStringParameterToAppender(appender, "@thread", 255, "%thread");
            AddStringParameterToAppender(appender, "@log_level", 50, "%level");
            AddStringParameterToAppender(appender, "@logger", 255, "%logger");
            AddStringParameterToAppender(appender, "@message", 4000, "%message");
            AddErrorParameterToAppender(appender, "@exception", 2000);

            appender.ActivateOptions();
            return appender;
        }

        #region Helper Methods

        private static string File(IConfiguration configuration)
        {
            var file = DefaultFileName;
            var fileInConfiguration = configuration["Logging:LogFilePath"];
            if (!string.IsNullOrEmpty(fileInConfiguration))
                file = fileInConfiguration;
            return file;
        }

        private static void AddStringParameterToAppender(this AdoNetAppender appender, string paramName, int size, string conversionPattern)
        {
            var param = new Service.Common.Logging.Parameters.AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = System.Data.DbType.String,
                Size = size,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout(conversionPattern))
            };
            appender.AddParameter(param);
        }

        private static void AddDateTimeParameterToAppender(this AdoNetAppender appender, string paramName)
        {
            var param = new Service.Common.Logging.Parameters.AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = System.Data.DbType.DateTime,
                Layout = new RawUtcTimeStampLayout()
            };
            appender.AddParameter(param);
        }

        private static void AddErrorParameterToAppender(this AdoNetAppender appender, string paramName, int size)
        {
            var param = new Service.Common.Logging.Parameters.AdoNetAppenderParameter
            {
                ParameterName = paramName,
                DbType = System.Data.DbType.String,
                Size = size,
                Layout = new Layout2RawLayoutAdapter(new ExceptionLayout())
            };
            appender.AddParameter(param);
        }
        #endregion
    }
}
