using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class ListarClientesResultVM : BaseResultVM
    {


        public int TotalRegistros { get; set; }
        public List<ClienteVM> Clientes { get; set; }

    }





}
