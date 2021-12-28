using System;
using System.Collections.Generic;
using System.Text;
using log4net.Layout;
using Service.Common.Utils;

namespace Service.Common.Logging.Layout
{

    public class LayoutFactory
    {
        private const string DEFAULT_PATTERN = "%level|%property{server}|%date{yyyyMMdd}|%date{HHmmss:fff}|%property{user}|%property{idunico}|%message%newline";

        private const string USER_PATTERN = "%level|%property{server}|%date{yyyyMMdd}|%date{HHmmss:fff}|%property{user}|%property{idunico}|%property{action}|%message%newline";

        public static ILayout Create(string application)
        {
            PatternLayout layout;
            if (string.IsNullOrEmpty(application))
            {
                layout = new PatternLayout { ConversionPattern = DEFAULT_PATTERN };
            }
            else
            {
                layout = new PatternLayout { ConversionPattern = USER_PATTERN };
            }
            
            layout.ActivateOptions();
            log4net.LogicalThreadContext.Properties["server"] = Server.GetLocalIPAddress();
            return layout;
        }

        private static string Pattern()
        {
            string pattern = DEFAULT_PATTERN;
            if (log4net.LogicalThreadContext.Properties["user"] != null)
            {
                pattern = USER_PATTERN;
            }
            return pattern;
        }
    }
}
