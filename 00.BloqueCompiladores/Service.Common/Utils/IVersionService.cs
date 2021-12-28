using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common.Utils
{
    public interface IVersionService
    {
        string GetVersion();
        string GetIp();
    }
}
