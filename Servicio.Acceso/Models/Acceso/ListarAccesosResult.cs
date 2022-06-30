using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Acceso
{
    public class ListarAccesosResult : BaseResult
    {
        public ListarAccesosResult()
        {

        }
        public List<ItemAcceso> Accesos { get; set; }
    }

}
