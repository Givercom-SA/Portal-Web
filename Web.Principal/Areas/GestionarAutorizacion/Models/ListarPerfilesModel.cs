using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ListarPerfilesModel() {

            this.Activo = -1;
            this.Tipo = "0";
        }


        public int IdPerfil { get; set; }

        public string Nombre { get; set; }
        public int Activo { get; set; }

        public string Tipo { get; set; }


        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }

        public SelectList ListTipo { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<PerfilVM> Perfiles { get; set; }


    }
}
