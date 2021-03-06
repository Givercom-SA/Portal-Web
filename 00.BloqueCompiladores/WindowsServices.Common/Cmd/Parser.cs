using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsServices.Common.Util;

namespace WindowsServices.Common.Cmd
{
    public static class Parser
    {

        public static List<string> Parse(Action<CmdArgConfigurator> configAction)
        {
            var config = new CmdArgConfiguration();
            var configurator = new CmdArgConfigurator(config);
            try
            {
                configAction(configurator);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Excepción lanzada en la acción de configuración pasada al método Parse. Ver excepción interna para más detalles.", e);
            }
            var args = Environment.GetCommandLineArgs();
            var extraArgs = new List<string>();
            if (args.Length > 0)
            {
                var argsWithoutLocation = args.Skip(1).ToArray();
                if (argsWithoutLocation.Length > 0)
                {
                    foreach (var argument in argsWithoutLocation)
                    {
                        var argumentLC = argument.ToLower();
                        var argParsed = false;
                        foreach (var parameter in config.parameters)
                        {
                            foreach (var key in parameter.Keys)
                            {
                                if (argumentLC.StartsWith(key))
                                {
                                    var rightSide = argument.Substring(key.Length);
                                    if (string.IsNullOrEmpty(rightSide))
                                    {
                                        parameter.Value(string.Empty);
                                        argParsed = true;
                                    }
                                    else if (rightSide.StartsWith(":"))
                                    {
                                        var value = rightSide.Substring(1);
                                        parameter.Value(value);
                                        argParsed = true;
                                    }
                                }
                            }
                        }
                        if (!argParsed)
                        {
                            extraArgs.Add(argument);
                        }
                    }
                }
            }
            if (extraArgs.Count > 0 && config.ShowHelpOnExtraArguments)
            {
                Logger.WriteLine("Argumentos no reconocidos: ");
                foreach (var extraArg in extraArgs)
                {
                    Logger.WriteLine("Key: " + extraArg);
                }
            }
            return extraArgs;
        }
    }
}
