using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models
{
    public class ListaDocumentoTipoEntidadResult : BaseResult
    {
        public IEnumerable<DocumentoTipoEntidadResult> ListaParametros { get; set; }
    }

    public class DocumentoTipoEntidadResult
    {


        public string PRMT_NOMBRE { get; set; }
        public string PRMT_VALOR { get; set; }
    }
}
