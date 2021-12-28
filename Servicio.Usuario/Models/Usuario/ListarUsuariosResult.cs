using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Usuario
{
    public class ListarUsuariosResult :BaseResult
    {
        public int TotalRegistros { get; set; }
       
        public List<Usuario> Usuarios { get; set; }
    }
}
