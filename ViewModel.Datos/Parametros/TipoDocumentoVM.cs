using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Parametros
{
    public class ListaTipoDocumentoVM : BaseResultVM
    {
        public IEnumerable<TipoDocumentoVM> ListaTipoDocumento { get; set; }
    }

    public class TipoDocumentoVM
    {
        public int IdParametro { get; set; }

        public string CodTipoDocumento { get; set; }

        public string NombreTipoDocumento { get; set; }
    }
}
