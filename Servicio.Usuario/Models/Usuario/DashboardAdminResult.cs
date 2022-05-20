using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Usuario
{
    public class DashboardAdminResult : BaseResult
    {
        public List<DashboardEstado> DashboardsXEstado { get; set; }
        public List<DashboardFecha> DashboardsXFecha { get; set; }

    }
}
