using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Filter;
using Microsoft.Extensions.Primitives;

namespace Service.Common.Logging
{
   public  class Log4NetFilter : IFilterLoggerSettings
    {
        public Log4NetFilter(ILoggerFactory factory)
        {
           
            var filterLoggerSettings = new FilterLoggerSettings
            {
                { "Microsoft",LogLevel.Warning},
                { "System",LogLevel.Warning},
                //{ "Microsoft.AspNetCore",LogLevel.None},
                //{ "Microsoft.EntityFrameworkCore",LogLevel.None},
                //{ "Default",LogLevel.Information},
                //{ "Microsoft.AspNetCore.Hosting.Internal.WebHost",LogLevel.None},
                //{ "Microsoft.AspNetCore.Server.Kestrel",LogLevel.None},
                //{ "Microsoft.AspNetCore.Builder.RouterMiddleware",LogLevel.None},
                
            };
            var filterDeny = new log4net.Filter.DenyAllFilter();
            //factory = factory.WithFilter(filterLoggerSettings);
            //factory = factory.WithFilter(filterDeny);
            //factory = factory.
           // _factory = factory;

        //    var x = new log4net.Appender.ConsoleAppender {Layout = new log4net.Layout.SimpleLayout()};
        //x.AddFilter(filter);
        //x.AddFilter(filterDeny);
            
        }

        public IChangeToken ChangeToken => throw new NotImplementedException();

        public IFilterLoggerSettings Reload()
        {
            throw new NotImplementedException();
        }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            throw new NotImplementedException();
        }
    }
}
