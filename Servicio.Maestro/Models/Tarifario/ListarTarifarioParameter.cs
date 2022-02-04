using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models.Tarifario
{
    public class ListarTarifarioParameter
    {
        public string Rubro { get; set; }
        public string Servicio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

    }




}
