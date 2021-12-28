using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsServices.Common.Util;

namespace WindowsServices.Common.Cmd.Utils.Help
{
    public static class Help
    {
        private static string getSpaces(int number)
        {
            return new string(' ', number);
        }

        public static void Show(HelpData data)
        {
            int longestKeyLenght = 5;
            foreach (var item in data.parameters)
            {
                if (item.Key.Length > longestKeyLenght)
                {
                    longestKeyLenght = item.Key.Length + 2;
                }
            }
            Logger.WriteLine("Help:");
            if (!string.IsNullOrEmpty(data.AppDescription))
            {
                Logger.WriteLine(data.AppDescription);
            }
            Logger.WriteLine(string.Format("Key{0}Description", getSpaces(longestKeyLenght - 3)));
            foreach (var item in data.parameters)
            {
                Logger.WriteLine(string.Format("{0}{1}{2}", item.Key, getSpaces(longestKeyLenght - item.Key.Length), item.Description));
            }
            Logger.WriteLine("Arguments are parsed as");
            Logger.WriteLine("Key:Value");

        }
    }
}
