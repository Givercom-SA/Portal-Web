using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudDireccionamiento
{
    public class ListarSolicitudesResultVM : BaseResultVM
    {
        public IEnumerable<SolicitudResultVM> ListaSolicitudes { get; set; }
    }

    public class SolicitudResultVM : BaseResultVM
    {
        public int Id { get; set; }
        public string KeyBL { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RazonSocial { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }

        public string Correo { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string CodAlmacen { get; set; }
        public string CodModalidad { get; set; }
        public string NombreModalidad { get; set; }
        public string Consignatario { get; set; }
        public string CantidadCnt { get; set; }
        public string AlmacenDestino { get; set; }
        public string Solicitante { get; set; }
        public string ReceptorCarga { get; set; }
        public string NroEmbarque { get; set; }
        public string CodMotivoRechazo { get; set; }
        public string MotivoRechazo { get; set; }

        public DateTime? FechaEvaluacion { get; set; }
        public string UsuarioEvalua { get; set; }

        public string TipoPerfil { get; set; }

        public IEnumerable<DocumentoResultVM> ListaDocumentos { get; set; }

        public IEnumerable<EventosResultVM> ListaEventos { get; set; }

    }

    public class ListarDocumentoResultVM : BaseResultVM
    {
        public IEnumerable<DocumentoResultVM> ListaDocumentos { get; set; }
    }

    public class DocumentoResultVM
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string UrlDocumento { get; set; }
        public string Estado { get; set; }
        public string CodMotivoRechazo { get; set; }
        public string NombreMotivoRechazo { get; set; }
    }
}
