using System;
using WindowsServices.Common.Interfaces;

namespace WindowsServices.Common
{
    public class MicroServiceController : IMicroServiceController
    {
        private Action stop;

        public MicroServiceController(Action stop)
        {
            this.stop = stop;
        }

        public void Stop()
        {
            stop();
        }
    }
}
