using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class DashboardClienteResultVM : BaseResultVM
    {
        public List<DashboardEstadoVM> DashboardsXEstado { get; set; }
        public List<DashboardFechaVM> DashboardsXFecha { get; set; }
    }





}
