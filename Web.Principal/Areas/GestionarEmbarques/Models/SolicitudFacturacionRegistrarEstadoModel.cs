using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class SolicitudFacturacionRegistrarEstadoModel
    {

        public SolicitudFacturacionRegistrarEstadoModel() {

        }

        public Int32 IdSolicitudFacturacion { get; set; }
        public string Correo { get; set; }

   
        public string Mensaje { get; set; }
        public string Estado { get; set; }



    }

}
