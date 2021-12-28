using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models
{
    public class ListarDocumentoTipoEntidadParameter
    {

        public bool BrindaCargaFCL { get; set; }

        public bool AcuerdoSeguridadCadenaSuministro { get; set; }
        public bool SeBrindaAgenciamientodeAduanas { get; set; }
        public List<TipoEntidad> TiposEntidad { get; set; }
    }

    public class TipoEntidad {
    
    
        public string CodigoEntidad { get; set; }


    }




}
