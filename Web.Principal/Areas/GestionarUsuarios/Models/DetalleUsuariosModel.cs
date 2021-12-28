using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class DetalleUsuariosModel
    {



        public UsuarioVM Usuario { get; set; }

        public string ReturnUrl { get; set; }


        public SelectList ListPerfiles { get; set; }

    }
}
