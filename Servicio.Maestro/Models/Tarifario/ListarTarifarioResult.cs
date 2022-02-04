using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models.Tarifario
{
    public class ListarTarifarioResult : BaseResult
    {
        public IEnumerable<Tarifario> Tarifarios { get; set; }
    }

    public class Tarifario
    {
        public string Rubro { get; set; }
        public string Servicio { get; set; }
        public string Tarifa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TiempoAntencion { get; set; }

     
    }
}
