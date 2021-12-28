using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacion
{
    public class SolicitarFacturacionParameter
    {
        public string KEYBLD { get; set; }
        public string NroBl { get; set; }

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

        public string ProvisionSeleccionado { get; set; }
        public string CodigoOperacionTransferencia { get; set; }
        public string BancoTransferencia { get; set; }

        public string FechaTransferencia { get; set; }
        public double ImporteTransferencia { get; set; }
        
        
        public int IdUsuarioSolicita { get; set; }
        public int IdEntidadSolicita { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }

        public string ClienteCodigo { get; set; }
        public string ClienteNroDocumento { get; set; }
        public string ClienteRazonSocial { get; set; }
        public string ClienteTipoDocumento { get; set; }


        public string CreditoDescripcion { get; set; }
        public string CodigoTipoEntidad { get; set; }

        
        public List<CobroCliente> CobrosPendientesCliente { get; set; }

    }

    public class CobroCliente
    {
        public string IdFacturacionTercero { get; set; }
        public string IdCliente { get; set; }
        public string TipoDocumentoCliente { get; set; }
        public string NroDocumentoCliente { get; set; }
        public string RazonSocialCliente { get; set; }
        public double MontoTotal { get; set; }
        public List<CobroPendietenEmbarque> CobrosPendientesEmbarque { get; set; }
    }


}
