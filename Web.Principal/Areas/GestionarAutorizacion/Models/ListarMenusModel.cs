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
    public class ListarMenusModel
    {

        public ListarMenusModel() {

            this.Activo = -1;
            this.IdMenuPadre = 0;

        }


   
        public string Nombre { get; set; }
        public int Activo { get; set; }
        public int IdMenuPadre { get; set; }
        public string TipoMenu { get; set; }



        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListMenusPadre { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListTipoMenu { get; set; }

        

        public string ReturnUrl { get; set; }

        public IEnumerable<MenuVM> Menus { get; set; }


    }
}
