using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Service.Common.Utils
{
    public class VersionService : IVersionService
    {
        private readonly string _version;
        private readonly string _ip;
        public VersionService()
        {
            _version = FileVersion.Version();
            _ip = Server.GetAliasIPAddress();
        }
      
        public string GetVersion()
        {
            return _version;
        }

        public string GetIp()
        {
            return _ip;
        }
    }
}
