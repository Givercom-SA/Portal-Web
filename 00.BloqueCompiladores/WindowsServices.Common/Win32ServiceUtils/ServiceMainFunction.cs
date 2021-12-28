using System;

namespace WindowsServices.Common.Win32ServiceUtils
{
    internal delegate void ServiceMainFunction(int numArs, IntPtr argPtrPtr);
}