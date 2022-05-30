using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace ViewModel.Datos.Embarque.SolicitudFacturacionTercero
{
    public class RegistrarFacturacionTerceroParameterVM
    {
        public int Id { get; set; }
        public string IdEntidad { get; set; }
        public string IdUsuario { get; set; }

        public string CodigoCliente { get; set; }
        public string ClienteNombres { get; set; }
        public string ClienteNroDocumeto { get; set; }
        public string IdUsuarioCrea { get; set; }
        public string Archivo { get; set; }
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueNroBL { get; set; }
        public string Estado { get; set; }
        public string Correo { get; set; }

        public string TipoEntidad { get; set; }
        public string AgenteAduanaRazonSocial { get; set; }
        public string AgenteAduanaTipoDocumento { get; set; }
        public string AgenteAduanaNumeroDocumento { get; set; }
        public string LogoEmpresa { get; set; }
        public int IdUsuarioEvalua { get; set; }
        public string CodigoEmpresaGtrm { get; set; }
        
        public List<CobrosPendienteEmbarqueVM> CobrosPendientesEmbarque { get; set; }


    }



}
