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
    public class EditarVistasModel
    {
        public EditarVistasModel()
        {


        }


        public MantenimientoVistaParameterVM NuevaVista { get; set; }
      

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }
        public string ReturnUrl { get; set; }
        public SelectList ListarVistaPadres { get; set; }


    }
}
