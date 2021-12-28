using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Models
{
    public class ListarTipoEntidadResult : BaseResult
    {
        public IEnumerable<ObjetoEntidadesResult> ListaEntidades { get; set; }
    }

    public class ObjetoEntidadesResult
    {
        public string CODIGO_TIPOENTIDAD { get; set; }
        public string NOMBRE_TIPOENTIDAD { get; set; }
    }
}
