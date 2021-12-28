using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common.Logging.Application
{
    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory { get; set; }// = new LoggerFactory();
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        public static ILogger CreateLogger(string categoryName) => LoggerFactory.CreateLogger(categoryName);

    }
}
