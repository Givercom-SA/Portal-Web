using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class MenuPerfilModel
    {
        public List<PerfilLoginVM> Perfiles { get; set; }
        public List<GruposAutorizacion> Grupos { get; set; }
    }
}
