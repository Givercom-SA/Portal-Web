using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Perfil;

namespace Web.Principal.Areas.GestionarAutorizacion.Models
{
    public class ListarPerfilesModel
    {
        public int IdPerfil { get; set; }

        public string Nombre { get; set; }
        public int Activo { get; set; } = -1;

        public string ReturnUrl { get; set; }

        public List<PerfilVM> Perfiles { get; set; }


    }
}
