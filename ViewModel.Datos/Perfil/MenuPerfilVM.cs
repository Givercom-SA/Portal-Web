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

        private string strTipoMenu;

        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }
        public int Permiso { get; set; }
        public int IdPadre { get; set; }
        public bool Visible { get; set; }
        public string TipoMenu { get {

                if (strTipoMenu == null)
                    return "";
                else
                    return strTipoMenu;

            } set { strTipoMenu = value; } }
        public int Orden { get; set; }
        public string IdMenuChecked { get; set; }
        
        public VistaMenuVM[] VistaMenu { get; set; }

    }
}
