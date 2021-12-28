using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsServices.Common
{
    enum ExitCode
    {
        Ok = 0,
        AbnormalExit = 1,
        SudoRequired = 2,
        ServiceAlreadyInstalled = 3,
        ServiceNotInstalled = 4,
        StartServiceFailed = 5,
        StopServiceFailed = 6,
        ServiceAlreadyRunning = 7,
        UnhandledServiceException = 8,
        ServiceNotRunning = 9,
        SendCommandFailed = 10,
    }
}
