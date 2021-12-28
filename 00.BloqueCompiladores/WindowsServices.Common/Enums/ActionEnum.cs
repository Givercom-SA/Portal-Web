using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsServices.Common.Enums
{
    public enum ActionEnum
    {
        Install,
        Uninstall,
        Run,
        RunInteractive,
        Stop,
        Start,
        Pause,
        Continue,
        CustomCommand
    }
}
