using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Solicitud;

namespace ViewModel.Datos.ListarSolicitudes
{
    public class ListarSolicitudesVW : BaseResultVM
    {
        public IEnumerable<SolicitudVM> ListaSolicitudes { get; set; }
    }

}
