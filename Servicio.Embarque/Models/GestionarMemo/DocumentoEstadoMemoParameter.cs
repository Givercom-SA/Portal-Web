using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class DocumentoEstadoMemoParameter 
    {
        public IEnumerable<DocumentoEstado> Documentos { get; set; }
        public string CodigoSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public int IdUsuarioEvalua { get; set; }
    }

    public class DocumentoEstado
    {
        
        public string CodigoDocumento { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoEstadoRechazo { get; set; }

        


    }
}
