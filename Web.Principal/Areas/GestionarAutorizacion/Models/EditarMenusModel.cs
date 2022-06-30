using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Menu;
using ViewModel.Datos.Perfil;

namespace Web.Principal.Areas.GestionarAutorizacion.Models
{
    public class EditarMenusModel
    {

        public EditarMenusModel() {

          

        }

        

  
        public int IdMenu { get; set; }



        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListTipoMenu { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListMenuPadre { get; set; }



        public string ReturnUrl { get; set; }

        public MenuVM Menu { get; set; }


    }
}
