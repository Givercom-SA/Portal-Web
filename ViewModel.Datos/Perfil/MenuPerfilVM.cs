using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Perfil
{
    public class MenuPerfilVM
    {
        public MenuPerfilVM()
        {

        }
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }
        public int Permiso { get; set; }
    }
}
