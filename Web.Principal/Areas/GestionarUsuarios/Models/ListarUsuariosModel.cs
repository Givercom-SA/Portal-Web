using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class ListarUsuariosModel : DataRequestViewModelResponse
    {
        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Perfil")]
        public int IdPerfil { get; set; } = 0;

        [Display(Name = "Entidad")]
        public Int64 IdEntidad { get; set; }

        [Display(Name = "Estado")]
        public int isActivo { get; set; } = -1;

        public int TipoUsuario { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList Perfiles { get; set; }

        public String ReturnUrl { get; set; }

        public ListarUsuariosResultVM ListUsuarios { get; set; }

    }
}
