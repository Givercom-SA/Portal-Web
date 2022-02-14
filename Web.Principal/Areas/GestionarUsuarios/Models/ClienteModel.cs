using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class ClienteModel
    {
        [Display(Name = "Tipo de Documento")]
        public string IdTipoDocumento { get; set; } 

        [Display(Name = "Número de Documento")]
        public string NumeroDocumento { get; set; }

        [Display(Name = "Razon Social / Representante Legal")]
        public string RazonSocuialRepresentanteLegal { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Perfil")]
        public int IdPerfil { get; set; } = -1;


        [Display(Name = "Estado")]
        public int isActivo { get; set; } = -1;

        public String ReturnUrl { get; set; }

        public ListarClientesResultVM ListarClientes { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListarTipoDocumento { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListarEstado { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListarPerfiles { get; set; }

    }
}
