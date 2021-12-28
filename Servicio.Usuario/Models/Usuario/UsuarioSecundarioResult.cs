using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Usuario
{
    public class UsuarioSecundarioResult: BaseResult
    {
        public Usuario usuario { get; set; }
    }
}
