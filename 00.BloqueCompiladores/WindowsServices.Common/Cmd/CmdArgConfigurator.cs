using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsServices.Common.Cmd.Utils.Help;
using WindowsServices.Common.Util;

namespace WindowsServices.Common.Cmd
{
    public class CmdArgConfigurator
    {
        CmdArgConfiguration config;
        public CmdArgConfigurator(CmdArgConfiguration config)
        {
            this.config = config;
        }

        public void AddParameter(CmdArgParam param)
        {
            config.parameters.Add(param);
        }

        public void AddParameters(List<CmdArgParam> parameters)
        {
            foreach (var param in parameters)
            {
                AddParameter(param);
            }
        }

        public void AddParameters(params CmdArgParam[] parameters)
        {
            foreach (var param in parameters)
            {
                AddParameter(param);
            }
        }

        public void UseDefaultHelp()
        {
            config.parameters.Add(new CmdArgParam()
            {
                Description = "Shows application help",
                Key = "help",
                Value = (val) => DisplayHelp()
            });
        }

        public void UseAppDescription(string description)
        {
            config.AppDescription = description;
        }

        public void ShowHelpOnExtraArguments()
        {
            config.ShowHelpOnExtraArguments = true;
        }

        public void CustomHelp(Action<HelpData> helpAction)
        {
            config.CustomHelp = helpAction;
        }

        public void DisplayHelp()
        {
            
            var helpData = new HelpData()
            {
                AppDescription = config.AppDescription,
                parameters = config.parameters.Select(item => new CmdArgParam()
                {
                    Description = item.Description,
                    Key = item.Key,
                    Value = item.Value
                }).ToList()
            };
            if(config.CustomHelp!=null)
            {
                try
                {
                    config.CustomHelp(helpData);
                }
                catch (Exception e)
                {
                    Logger.WriteLine("La ayuda personalizada en esta implementación arrojó una excepción.");
                    Logger.WriteLine("Detalle de la excepcion:");
                    Logger.WriteLine(e.ToString());
                    Logger.WriteLine("Mostrando ayuda predeterminada:");
                    Help.Show(helpData);
                }
            }
            else
            {
                Help.Show(helpData);
            }
        }
    }
}
