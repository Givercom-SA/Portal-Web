using System;

namespace WindowsServices.Common.Win32ServiceUtils
{
    internal delegate void ServiceControlHandler(ServiceControlCommand control, uint eventType, IntPtr eventData, IntPtr eventContext);
}