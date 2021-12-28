using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class ListarPerfilesResult: BaseResult
    {
        public ListarPerfilesResult()
        {

        }
        public List<Perfil> Perfiles { get; set; }
    }

}
