using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class SolicitudMemoParameter
    {
        public string KeyBL { get; set; }
        public string Correo { get; set; }
        public string NroEmbarque { get; set; }
        public int IdUsuarioCrea { get; set; }
        public string CodigoEmpresaServicio { get; set; }
        public List<DocumentoMemo> Documentos { get; set; }

    }

    public class DocumentoMemo {

        public string CodigoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }


    }

}
