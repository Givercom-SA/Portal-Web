using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class AsignarAgenteHistorialResult : BaseResult
    {
        public List<AgenteAduanasHistorial> Historial { get; set; }
 

    }

    public class AgenteAduanasHistorial {
        public int IdAsignacionAduanaHistorial { get; set; }
        public int IdAsignacionAduana { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaHistorial { get; set; }
        public string Descripcion { get; set; }
        public string EstadoSolicitud { get; set; }
        

    }

}
