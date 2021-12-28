using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class ListarPerfilesActivosResult : BaseResult
    {
        public ListarPerfilesActivosResult()
        {

        }

        public List<Perfil> Perfiles { get; set; }
    }

}
