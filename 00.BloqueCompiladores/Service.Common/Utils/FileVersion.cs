using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service.Common.Utils
{
    public class FileVersion
    {
        public static string Version()
        {
            try
            {
                FileStream fileStream = new FileStream("build.version", FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return reader.ReadLine();
                }
            }
            catch (Exception)
            {

                return string.Empty;
            }

        }


    }

    public class Server
    {
        public static string GetAliasIPAddress()
        {


            var ipLocal = string.Empty;
            var ips = new List<string>();
            try
            {
                var servers = new Dictionary<string, string> { { "4",""},{ "5","" }, { "6","" }, { "20", "DES" } };
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ips.Add(ip.ToString().Trim());
                    }
                }
                foreach (var item in ips)
                {
                    var i = IPAddress.Parse(item).GetAddressBytes()[3].ToString();
                    if (servers.ContainsKey(i)) ipLocal = string.Format("{0}-{1}", servers[i], i);
                }
            }
            catch { return string.Empty; }
            return string.IsNullOrEmpty(ipLocal) ? "" : ipLocal;
        }

        public static string GetLocalIPAddress()
        {


            var ipLocal = string.Empty;
            var ips = new List<string>();
            try
            {
                var servers = new Dictionary<string, string> { { "4", "PROD" }, { "5", "PROD" }, { "6", "PROD" }, { "20", "DES" } };
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ips.Add(ip.ToString().Trim());
                    }
                }
                foreach (var item in ips)
                {
                    var i = IPAddress.Parse(item).GetAddressBytes()[3].ToString();
                    if (servers.ContainsKey(i)) ipLocal = string.Format("{0}", item);
                }
            }
            catch { return string.Empty; }
            ipLocal = string.IsNullOrEmpty(ipLocal) ? "LOCAL" : ipLocal;
            return ipLocal;
        }
    }
}
