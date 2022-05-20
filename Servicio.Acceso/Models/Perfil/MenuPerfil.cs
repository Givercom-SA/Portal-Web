using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Perfil
{
    public class MenuPerfil
    {
        public MenuPerfil()
        {

        }
        public int IdMenu { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }
        public int Permiso { get; set; }
        
    }




}
