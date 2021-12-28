using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class DisponibilidadCreditoModel
    {
       public int Resultado { get; set; }
        public string Motivo { get; set; }
    }

}

