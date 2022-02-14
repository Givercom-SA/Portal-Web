using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.ListarEventos;
using ViewModel.Datos.ListarTipoEntidadSolicitud;

namespace ViewModel.Datos.Solicitud
{
    public class SolicitudVM
    {
        public Int64 IdSolicitud { get; set; }
        public string CodigoSolicitud { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RazonSocial { get; set; }
        public string UsuarioEvalua { get; set; }
        
        public string CodigoEstado { get; set; }
        public string NombreEstado { get; set; }
        public string Correo { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreRepresentanteL { get; set; }
        public string ApellidoPatRepresentanteL { get; set; }
        public string ApellidoMatRepresentanteL { get; set; }

        public bool? AcuerdoCadenaSuministro { get; set; }
        public bool? BrindaAgenciamientoAduanas { get; set; }
        
        public bool? TerminoCondicionGeneralContracion { get; set; }
        public bool? ProcesoFacturacion { get; set; }
        public bool? DeclaracionJuaradVerecidad { get; set; }
        public bool? BrindaCargaFCL { get; set; }
        public bool? AcuerdoEndoceElectronico { get; set; }

        public DateTime? FechaEvaluacion { get; set; }
        public int? IdUsuarioEvaluacion { get; set; }
        public string MotivoRechazo { get; set; }
        public string CodigoSunat { get; set; }

        

        public IEnumerable<DocumentosVW> ListaDocumentos { get; set; }
        public IEnumerable<EventoVW> ListaEventos { get; set; }
        public IEnumerable<TipoEntidadVW> ListaTipoEntidad { get; set; }


       

    }

    public class DocumentosVW
    {
        public string CodigoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string UrlDocumento { get; set; }
        public string CodigoEstado { get; set; }
        public string CodigoRechazo { get; set; }
        public string NombreRechazo { get; set; }
    }
}
