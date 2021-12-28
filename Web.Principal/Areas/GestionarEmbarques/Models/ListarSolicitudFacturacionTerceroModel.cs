using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class ListarSolicitudFacturacionTerceroModel
    {

        public string Entidad { get; set; }
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueNroBL { get; set; }
        public string Cliente { get; set; }
        public string NroDocumento { get; set; }
        public string Estado { get; set; }

        public string TipoPerfil { get; set; }

        public ListarFacturacionTerceroResultVM model { get; set; }
        
    }
}
