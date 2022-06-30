using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Menu
{
    public class ListarMenusResultVM : BaseResultVM
    {
        public ListarMenusResultVM()
        {

        }
        public List<MenuVM> Menus { get; set; }
        
    }

}
