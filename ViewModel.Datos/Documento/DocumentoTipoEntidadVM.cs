using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Documento
{
    public class ListarDocumentoTipoEntidadVM : BaseResultVM
    {
        public List<DocumentoTipoEntidadVM> listarDocumentosTipoEntidad { get; set; }
    }

    public class DocumentoTipoEntidadVM
    {
        

        public string CodigoDocumento { get; set; }

        public string NombreDocumento { get; set; }
    }
}
