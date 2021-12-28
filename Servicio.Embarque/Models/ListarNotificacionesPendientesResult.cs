using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class ListarNotificacionesPendientesResult : BaseResult
    {
        public List<NotificacionesPendientesResult> ListaNotificacionesPendientes { get; set; }
    }

    public class NotificacionesPendientesResult
    {
        public string NOTARR_KEYBLD { get; set; }
        public string NOTARR_ESTADO { get; set; }
        public string NOTARR_NUMERACION_EMBARQUE { get; set; }
        public int NOTARR_IDUSUARIO_CREA { get; set; }
        public DateTime NOTARR_FECHA_REGISTRO { get; set; }
        public string NOTARR_TIPO_DOCUMENTO { get; set; }
        public string NOTARR_CODGTRMEMPRESA { get; set; }
        public string GTEM_NOMBRES { get; set; }
        public string GTEM_RUC { get; set; }




    }
}
