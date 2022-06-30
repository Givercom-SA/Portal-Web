using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using Web.Principal.Model;
using ViewModel.Common.Request;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class LiberacionCargaListarModel 
    {

        public string ParKey { get; set; }
        public string NroBl { get; set; }
    
   
    }

}
