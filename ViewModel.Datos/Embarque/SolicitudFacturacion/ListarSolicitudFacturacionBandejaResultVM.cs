using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{
    public class ListarSolicitudFacturacionBandejaResultVM : BaseResultVM
    {
        public List<SolicitudFacturacionVM> SolicitudesFacturacion { get; set; }
        public string TipoPerfil { get; set; }
    }

    public class SolicitudFacturacionVM
    {
        public string KeyBld { get; set; }
        public string NroBl { get; set; }
        public string CodigoTipoEntidad { get; set; }
        public string MetodoPagoDescripcion { get; set; }
        public string TipoPagoDescripcion { get; set; }
        public string EstadoDescripcion { get; set; }
        
        public decimal MontoTotal { get; set; }

        public string MontoTotalString() {
            return string.Format("{0}",this.MontoTotal);
        }

        public string CreditoDescripcion { get; set; }

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
        public string AcetoFormularioString() {

            if (this.AceptoFormulario)
            {
                return "SI";
            }
            else {
                return "NO";
            }
        }
        public string ProvisionSeleccionado { get; set; }
        public string CodigoOperacionTransferencia { get; set; }
        public string SolicitanteRUC { get; set; }
        public string SolicitanteTipoDocumento { get; set; }
        public string BancoTransferencia { get; set; }

        public string BancoTransferenciadescriocion() {



            if (this.BancoTransferencia.Equals(Utilitario.Constante.EmbarqueConstante.Banco.BCP_CODIGO)) {

                return Utilitario.Constante.EmbarqueConstante.Banco.BCP.ToString();
            }
            if (this.BancoTransferencia.Equals(Utilitario.Constante.EmbarqueConstante.Banco.IBK_CODIGO))
            {

                return Utilitario.Constante.EmbarqueConstante.Banco.IBK.ToString();
            }

            if (this.BancoTransferencia.Equals(Utilitario.Constante.EmbarqueConstante.Banco.SKB_CODIGO))
            {

                return Utilitario.Constante.EmbarqueConstante.Banco.SKB.ToString();
            }

            return "";

        }
        public DateTime FechaTransferencia { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string ImporteTransferencia { get; set; }
        public int IdSolicitudFacturacion { get; set; }
        public string SolicitanteCorreo { get; set; }
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
        public List<SolicitudFacturacionDetalleVM> DetalleFacturacion { get; set; }
        public List<EventoSolicitudFacturacionVM> Enventos { get; set; }
        
    }

    public class SolicitudFacturacionDetalleVM
    {
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

    public class EventoSolicitudFacturacionVM
    {

        public DateTime FechaRegistro { get; set; }
        public string NombreUsuario { get; set; }
        public string Estado{ get; set; }
        public string Descripcion { get; set; }

    }

}
