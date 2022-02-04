using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacion
{
    public class ListarSolicitudFacturacionBandejaResult : BaseResult
    {
        public List<SolicitudFacturacion>  SolicitudesFacturacion { get; set; }
    }

    public class SolicitudFacturacion
    {
        public string KeyBld { get; set; }
        public string NroBl { get; set; }

        public string CodigoTipoEntidad { get; set; }
        public string EstadoDescripcion { get; set; }
        
        public bool AplicaCredito
        {
            get; set;
        }
        public string CodigoCredito
        {
            get; set;
        }

        public string TipoPago
        {
            get; set;
        }
        public string MetodoPago
        {
            get; set;
        }
        public bool AceptoFormulario
        {
            get; set;
        }

        public string SolicitanteRUC { get; set; }
        public string SolicitanteTipoDocumento { get; set; }

        public string ProvisionSeleccionado { get; set; }
        public string CodigoOperacionTransferencia { get; set; }
        public string BancoTransferencia { get; set; }
        public string SolicitanteCorreo { get; set; }
        public DateTime FechaTransferencia { get; set; }
        public DateTime? FechaRegistro { get; set; }
        
        public string ImporteTransferencia { get; set; }

        public int IdSolicitudFacturacion { get; set; }
        public int IdUsuarioSolicita { get; set; }
        
        public int IdEntidadSolicitante { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }

        public string ClienteCodigo { get; set; }
        public string ClienteNroDocumento { get; set; }
        public string ClienteRazonSocial { get; set; }
        public string ClienteTipoDocumento { get; set; }

        public string SolicitanteNombres { get; set; }
        public string SolicitanteEmpresaPersona { get; set; }
        public string Estado { get; set; }
        public string CodigoSolicitud { get; set; }

 
        public string MetodoPagoDescripcion { get; set; }

        public decimal     MontoTotal { get; set; }
        public string CreditoDescripcion { get; set; }
        public string TipoPagoDescripcion { get; set; }
        public string IdSolicitudTaf { get; set; }
        
        public List<EventoSolicitudFacturacion> Enventos { get; set; }
        public List<SolicitudFacturacionDetalle> DetalleFacturacion { get; set; }


    }

    public class SolicitudFacturacionDetalle {

        public int IdSolicitudFacturacionDetalle { get; set; }
        public int IdSolicitudFacturacion { get; set; }
        public string CodigoConcepto { get; set; }
        public string Moneda { get; set; }
        public string Importe { get; set; }
        public string IGV { get; set; }
        public string Total { get; set; }
        public string DescripcionConcepto { get; set; }
        public string DescripcionProvision { get; set; }
        public string IdProvision { get; set; }
        public string CodigoRubro { get; set; }
        public string KeyBl { get; set; }
        public string NroBl { get; set; }
        public string CodigoCredito { get; set; }
        
    }

    public class EventoSolicitudFacturacion
    {

        public DateTime FechaRegistro { get; set; }
        public string NombreUsuario { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }

    }



}
