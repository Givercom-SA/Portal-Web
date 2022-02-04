using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.GestionarMemo
{
    public class SolicitudMemoParameterVM
    {
        public string KeyBL { get; set; }
        public string Correo { get; set; }
        public string NroEmbarque { get; set; }

        public string ImagenEmpresaLogo { get; set; }
        public int IdUsuarioCrea { get; set; }
        public string CorreoOperador { get; set; }
        public string CodigoEmpresaServicio { get; set; }

        
        public List<DocumentoMemoVM> Documentos { get; set; }

    }

    public class DocumentoMemoVM {

        public string CodigoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }


    }

}
