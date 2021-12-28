using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class RegistrarFacturacionTerceroParameter
    {
        public int EMFT_ID { get; set; }
        public string EMFT_IDENTIDAD { get; set; }
        public string EMFT_IDUSUARIO { get; set; }

        public string EMFT_CODIGO_CLIENTE { get; set; }
        public string EMFT_CLIENTE_NOMBRE { get; set; }
        public string EMFT_CLIENTE_NRODOC { get; set; }
        public string EMFT_IDUSUARIO_CREA { get; set; }
        public string EMFT_ARCHIVO { get; set; }
        public string EMFT_EMBARQUE_KEYBL { get; set; }
        public string EMFT_EMBARQUE_NROBL { get; set; }
        public string EMFT_ESTADO { get; set; }
        public string USU_CORREO { get; set; }

        public string TipoEntidad { get; set; }
        public string AgenteAduanaRazonSocial { get; set; }
        public string AgenteAduanaTipoDocumento { get; set; }
        public string AgenteAduanaNumeroDocumento { get; set; }
        public List<CobroPendietenEmbarque> CobrosPendientesEmbarque { get; set; }
        public int IdUsuarioEvalua { get; set; }

    }
    public class CobroPendietenEmbarque
    {
        public string RubroCodigo { get; set; }
        public string ConceptoCodigo { get; set; }
        public string Descripcion { get; set; }
        public string ConceptoCodigoDescripcion { get; set; }
        public string Moneda { get; set; }
        public string Importe { get; set; }
        public string Igv { get; set; }
        public string Total { get; set; }
        public string FlagAsignacion { get; set; }
        public string ID { get; set; }



    }


}
