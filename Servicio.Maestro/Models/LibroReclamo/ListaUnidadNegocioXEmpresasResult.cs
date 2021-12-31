using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models.LibroReclamo
{
    public class ListaUnidadNegocioXEmpresasResult : BaseResult
    {
        public IEnumerable<UnidadNegocioReclamo> UnidadNegociosReclamo { get; set; }
        public IEnumerable<TipoDocumentoReclamo> TiposDocumentos { get; set; }
    }

    public class UnidadNegocioReclamo
    {
        public string CodigoUnidadNegocio { get; set; }
        public string NombreUnidadNegocio { get; set; }
        public string CodigoEmpresa { get; set; }
    }

    public class TipoDocumentoReclamo
    {

        public string CodigoTipoDocumento { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string CodigoEmpresa { get; set; }
    }
}
