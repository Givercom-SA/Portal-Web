using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsServices.Common.Interfaces
{
    public interface IMicroService
    {
        void Start();
        void Stop();
    }
}
