using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacion;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class VerSolicitudFacturacionModel
    {

        public VerSolicitudFacturacionModel() {

            this.LeerSolicitudFacturacionBandejaParameter = new LeerSolicitudFacturacionBandejaParameterVM();
            this.LeerSolicitudFacturacionBandejaResult = new LeerSolicitudFacturacionBandejaResultVM();

        }

        public LeerSolicitudFacturacionBandejaParameterVM  LeerSolicitudFacturacionBandejaParameter { get; set; }

        public LeerSolicitudFacturacionBandejaResultVM LeerSolicitudFacturacionBandejaResult { get; set; }

    }

}
