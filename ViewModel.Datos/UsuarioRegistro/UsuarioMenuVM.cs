using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Perfil;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class UsuarioMenuVM
    {
        private string _grupo { get; set; }
        public UsuarioMenuVM()
        {

        }
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Grupo
        {
            get
            {
                if (this._grupo == null)
                {
                    return "";
                }
                else
                {
                    return this._grupo;
                }
            }
            set { this._grupo = value; }
        }
        public int Permiso { get; set; }
        public int IdPadre { get; set; }
        public string TipoMenu { get; set; }
        public int Orden { get; set; }
        
        public List<VistaMenuVM> VistaMenu { get; set; }



    }
}
