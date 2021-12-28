using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.ListaNotificacionesArribo
{
    public class ListarNotificacionesPendientesVW : BaseResultVM
    {
        public List<NotificacionesPendientesVM> ListaNotificacionesPendientes { get; set; }
    }

    public class NotificacionesPendientesVM
    {
        public string KeyBld { get; set; }
        public string tipoDocumento { get; set; }

        public string CodigoGtrmEmpresa { get; set; }
        public string GtrmEmpresaRazonSocial { get; set; }
        public string GtrmEmpresaRuc { get; set; }

    }
}
