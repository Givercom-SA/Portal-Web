using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.Subscription
{
    public interface IDatabaseSubscription
    {
        void Configure(string connectionString);
    }
}
