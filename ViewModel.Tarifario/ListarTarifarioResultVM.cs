using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Tarifario
{
    public class ListarTarifarioResultVM : BaseResultVM
    {
        public IEnumerable<TarifarioVM> Tarifarios { get; set; }
    }

    public class TarifarioVM
    {
        public string Rubro { get; set; }
        public string Servicio { get; set; }
        public string Tarifa { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TiempoAntencion { get; set; }

     
    }
}
