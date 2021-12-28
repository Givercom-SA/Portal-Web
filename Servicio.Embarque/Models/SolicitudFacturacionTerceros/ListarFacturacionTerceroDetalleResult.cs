using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacionTerceros
{
    public class ListarFacturacionTerceroDetalleResult : BaseResult
    {
        public string Codigo { get; set; }
        public string CodigoCliente { get; set; }
        public string ClienteNombre { get; set; }
        public string NroDocumento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModifica { get; set; }
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueNroBL { get; set; }
        public string UsuarioNombre { get; set; }
        public string EntidadDatos { get; set; }
        public string Estado { get; set; }
        public string EstadoNombre { get; set; }
        public string Archivo { get; set; }

        public string TipoEntidad { get; set; }
        public string AgenteNumeroDocumento { get; set; }
        public string AgenteRazonSocial { get; set; }
        public string AgenteTipoDocumento { get; set; }
  


        public List<SolicitudFacturacionTerceroDetalle> ListFacturacionTerceroDetalle { get; set; }
        public List<FacturacionTerceroHistorial> Historial { get; set; }

    }
    public class FacturacionTerceroHistorial
    {

        public string NombreUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Descripcion { get; set; }
        public string EstadoSolicitud { get; set; }
        

    }

    public class SolicitudFacturacionTerceroDetalle {
        public int Id { get; set; }
        public string IdProvision { get; set; }
        
        public string IdEmbarqueFactTercero { get; set; }
        public string CodigoConcepto { get; set; }
        public string TipoProvicion { get; set; }
        public string Concepto { get; set; }
        public string Moneda { get; set; }
        public string Importe { get; set; }
        public string IGV { get; set; }
        public string Total { get; set; }

    }




}
