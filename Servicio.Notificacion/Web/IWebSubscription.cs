using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.Subscription.Web
{
    public interface IWebSubscription
    {
        void Configure(IEnumerable<string> connectionString);
    }
}
