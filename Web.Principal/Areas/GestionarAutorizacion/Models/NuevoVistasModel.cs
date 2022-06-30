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
    public class NuevoVistasModel
    {
        public NuevoVistasModel()
        {


        }


        public MantenimientoVistaParameterVM NuevaVista { get; set; }
        public string FechaRegistro { get; set; }
        public string UsuarioCrea { get; set; }
        

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList ListEstado { get; set; }
        public string ReturnUrl { get; set; }
        public SelectList ListarVistaPadres { get; set; }


    }
}
