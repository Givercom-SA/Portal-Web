using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Usuario.Models.Cliente
{
    public class ListarClienteResult : BaseResult
    {
        public int TotalRegistros { get; set; }
       
        public List<Cliente> Clientes { get; set; }
    }
}
