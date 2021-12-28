
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsServices.Common.Win32ServiceUtils;

namespace WindowsServices.Common.Interfaces
{
    public interface IShutdownableWin32Service : IWin32Service
    {
        void Shutdown();
    }
}
