using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class TraerPerfilResult: BaseResult
    {
        public TraerPerfilResult()
        {

        }
        public Perfil perfil { get; set; }
    }
}
