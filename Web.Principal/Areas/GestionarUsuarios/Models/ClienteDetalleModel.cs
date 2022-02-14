using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.AsignarAgente;
using ViewModel.Datos.UsuarioRegistro;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class ClienteDetalleModel
    {
        public ViewModel.Datos.Solicitud.SolicitudVM Solicitud { get; set; }
        public ClienteVM Entidad { get; set; }
        public string CodigoSolicitud { get; set; }

    }
}
