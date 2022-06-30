using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Menu;
using ViewModel.Datos.Perfil;
using ViewModel.Datos.Vista;

namespace Web.Principal.Areas.GestionarAutorizacion.Models
{
    public class ListarVistasModel
    {
        public ListarVistasModel() {

            this.Activo = -1;
            this.IdVistaPadre = 0;
        }

        public string Nombre { get; set; }
        public int Activo { get; set; }
        public int IdVistaPadre { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListArea { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListController { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListAction { get; set; }
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListarVistaPadres { get; set; }
        public string ReturnUrl { get; set; }
        
        public IEnumerable<VistaVM> Vistas { get; set; }

    }
}
