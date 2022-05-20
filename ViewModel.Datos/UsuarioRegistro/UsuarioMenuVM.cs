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
        public UsuarioMenuVM()
        {

        }
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }
        public int Permiso { get; set; }
        public List<VistaMenuVM> VistaMenu { get; set; }



    }
}
