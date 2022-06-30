using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Vista
{
    public class ListarTodoVistaResultVM : BaseResultVM
    {
        public ListarTodoVistaResultVM()
        {

        }
        public IEnumerable<VistaTodoVM> Vistas { get; set; }

    }
    public class VistaTodoVM
    {

        public int IdVista { get; set; }
        public string Nombres { get; set; }
    }
}
