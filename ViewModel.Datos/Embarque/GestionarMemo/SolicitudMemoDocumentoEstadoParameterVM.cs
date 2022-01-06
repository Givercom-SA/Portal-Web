using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.GestionarMemo
{
    public class SolicitudMemoDocumentoEstadoParameterVM
    {
        public IEnumerable<DocumentoMemoEstadoVM> Documentos { get; set; }
        public string CodigoSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public int IdUsuarioEvalua { get; set; }
        public string codigoEstadoEvalua { get; set; }
        public string CodigoMotivoRechazo { get; set; }

    }

    public class DocumentoMemoEstadoVM {

        public string CodigoDocumento { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoEstadoRechazo { get; set; }



    }

}
