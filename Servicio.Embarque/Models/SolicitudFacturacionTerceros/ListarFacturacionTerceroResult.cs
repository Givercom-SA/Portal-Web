using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models.SolicitudFacturacionTerceros
{
    public class ListarFacturacionTerceroResult : BaseResult
    {

        public List<SolicitudFacturacionTercero> SolicitudesFacturacionTerceros { get; set; }



    }

    public class SolicitudFacturacionTercero {
        public string EMFT_CODIGO { get; set; }
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

        public DateTime EMFT_FECHA_REGISTRO { get; set; }
        
        public string USU_NOMBRES { get; set; }
        public string USU_CORREO { get; set; }
        public string ENTIDAD_DATOS { get; set; }

        public string EMFT_ESTADO { get; set; }
        public string EMFT_ESTADO_NOMBRE { get; set; }

        public List<CobroPendietenEmbarque> CobrosPendientesEmbarque { get; set; }

    }




}
