using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class ObtenerPerfilResult: BaseResult
    {
        public ObtenerPerfilResult()
        {

        }
        public Perfil Perfil { get; set; }
    }
}
