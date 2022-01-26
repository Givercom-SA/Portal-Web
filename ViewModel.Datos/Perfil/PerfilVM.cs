using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoDatos.Utils;

namespace ViewModel.Datos.Perfil
{
    public class PerfilVM
    {
        public int IdPerfil {get;set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public int IdSesion { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }

        public string TipoPerfil { get; set; }
        public string UsuarioCrea { get; set; }
        public string UsuarioModifica { get; set; }

        public string Tipo { get; set; }
        public string Dashboard { get; set; }
     
        public List<MenuPerfilVM> Menus { get; set; }
    }

    //public class MenuPerfilVM
    //{
    //    public int IdMenu { get; set; }
    //    public string Nombre { get; set; }
    //    public string Grupo { get; set; }
    //    public int Permiso { get; set; }
    //}
}
