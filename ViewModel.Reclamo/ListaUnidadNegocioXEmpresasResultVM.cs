using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Reclamo
{
    public class ListaUnidadNegocioXEmpresasResultVM : BaseResultVM
    {
        public List<UnidadNegocioReclamoVM> UnidadNegociosReclamo { get; set; }
        public IEnumerable<TipoDocumentoReclamoVM> TiposDocumentos { get; set; }
    }
    public class UnidadNegocioReclamoVM
    {
        public string CodigoUnidadNegocio { get; set; }
        public string NombreUnidadNegocio { get; set; }
        public string CodigoEmpresa { get; set; }
    }
    public class TipoDocumentoReclamoVM
    {

        public string CodigoTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string CodigoEmpresa { get; set; }
    }
}
