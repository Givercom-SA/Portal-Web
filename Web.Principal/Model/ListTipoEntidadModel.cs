using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.Model
{
    public class ListTipoEntidadModel
    {
        public List<TipoEntidad> TiposEntidad { get; set; }
    }

    public class TipoEntidad {

        public int IdParametro { get; set; }
        public string CodTipoEntidad { get; set; }
        public string NombreTipoEntidad { get; set; }

        public bool Check { get; set; }
        

    }


}
