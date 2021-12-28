using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.AsignarAgente
{
    public class AsignarAgenteHistorialResultVM : BaseResultVM
    {
        public List<AgenteAduanasHistorialVM> Historial { get; set; }
    }
    public class AgenteAduanasHistorialVM
    {
        public int IdAsignacionAduanaHistorial { get; set; }
        public int IdAsignacionAduana { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaHistorial { get; set; }
        public string Descripcion { get; set; }
        public string EstadoSolicitud { get; set; }

    }
}
