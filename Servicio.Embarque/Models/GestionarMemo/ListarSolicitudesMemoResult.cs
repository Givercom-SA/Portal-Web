using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.GestionarMemo
{
    public class ListarSolicitudesMemoResult : BaseResult
    {
        public IEnumerable<SolicitudMemoResult> ListaSolicitudes { get; set; }
    }

    public class SolicitudMemoResult : BaseResult
    {
        public string KeyBL { get; set; }
        public string Codigo { get; set; }
        public int IdSolicitud { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public string Correo { get; set; }

        public string CodMotivoRechazo { get; set; }

        public string Solicitante { get; set; }

        public string NroEmbarque { get; set; }

        public string Consignatario { get; set; }

        public string UsuarioEvalua { get; set; }

        public DateTime? FechaEvalua { get; set; }

        public string EstadoEvalua { get; set; }



    

        public IEnumerable<DocumentoMemoResult> ListaDocumentos { get; set; }

        public IEnumerable<EventosMemoResult> ListaEventos { get; set; }

    }

    public class ListarDocumentoMemoResult : BaseResult
    {
        public IEnumerable<DocumentoMemoResult> ListaDocumentos { get; set; }
    }

    public class DocumentoMemoResult
    {
        public string Codigo { get; set; }
        public int Padre { get; set; }
        public string Nombre { get; set; }
        public string UrlDocumento { get; set; }
        public string Estado { get; set; }
        public string CodMotivoRechazo { get; set; }
        public string NombreMotivoRechazo { get; set; }
        public string NombreDocumento { get; set; }
        
    }
}
