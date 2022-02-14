using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Cliente
{
    public class LeerClienteResult : BaseResult
    {
      
       
        public Cliente Cliente { get; set; }
    }
}
