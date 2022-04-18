using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class ClienteVM
    {


        public string RazonSocialRepresentanteLegal() {


            if (this.TipoDocumento.Trim().Equals(Utilitario.Constante.EmbarqueConstante.TipoDocumento.RUC))
            {
                return $"{this.RazonSocial}";
            }
            else {
                return $"{this.RepresentanteLegalNombre} {this.RepresentanteLegalApellidoPaterno}";
            }


        }


        public Int64 IdEntidad { get; set; }
        public string Codigo { get; set; }
        
        public string TipoDocumento { get; set; }
        public string TipoDocumentoNombre { get; set; }
        public string NumeroDocumento { get; set; }
        public bool Activo { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentanteLegalNombre { get; set; }
        public string RepresentanteLegalApellidoPaterno { get; set; }
        public string RepresentanteLegalApellidoMaterno { get; set; }
        public string RepresentanteLegalCorreo { get; set; }
        public string CodigoSunat { get; set; }
        public Int64 IdSolicitud { get; set; }
        public bool ProcesoFacturacion { get; set; }
        public bool TerminoCondicionContrato { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioModifica { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ClienteBroker { get; set; }
        public string AgenteAduana { get; set; }
        public string ClienteForwarder { get; set; }
        public string ClienteFinal { get; set; }
    }
}
