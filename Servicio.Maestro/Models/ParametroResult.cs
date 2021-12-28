using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models
{
    public class ListaParametroResult : BaseResult
    {
        public IEnumerable<ParametroResult> ListaParametros { get; set; }
    }

    public class ParametroResult
    {
        public int PRMT_ID { get; set; }

        public string PRMT_NOMBRE { get; set; }
        public string PRMT_VALOR { get; set; }
    }
}
