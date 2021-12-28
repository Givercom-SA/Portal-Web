using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Models
{
    public class ListaCorreosResult : BaseResult
    {
        public List<string> ListaCorreos { get; set; }
    }
}
