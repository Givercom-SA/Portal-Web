using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Models
{
    public class ListarEventosResult : BaseResult
    {
        public IEnumerable<ObjetoEventosResult> ListaEventos { get; set; }
    }

    public class ObjetoEventosResult
    {
        public int SAEV_ID { get; set; }
        public string SAEV_NOMUSUARIO { get; set; }
        public string SAEV_DESCRIPCIN { get; set; }
        public DateTime SAEV_FECHA_REGISTRO { get; set; }
    }
}
