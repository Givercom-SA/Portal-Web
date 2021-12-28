using AccesoDatos.Utils;
using Servicio.Embarque.Models.GestionarMemo;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using Servicio.Embarque.Models.SolicitudFacturacion;
using Servicio.Embarque.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public interface ISolicitudFacturacionRepository
    {
        
        public ListarSolicitudFacturacionBandejaResult ObtenerFacturacionListaBandeja(ListarSolicitudFacturacionBandejaParameter parameter);
        public LeerSolicitudFacturacionBandejaResult ObtenerFacturacionBandeja(LeerSolicitudFacturacionBandejaParameter parameter);
        public SolicitarFacturacionEstadoResult RegistrarSolicitudFacturacionEstado(SolicitarFacturacionEstadoParameter parameter);

        public LeerSolicitudFacturacionKeyBlResult ListarSolicitudFacturacionPorKeyBl(string keyBl);
        public SolicitarFacturacionResult SolicituFacturacion(SolicitarFacturacionParameter parameter);
        public NotificacionFacturacionResult NotificarFacturacion(NotificacionFacturacionParameter parameter);

    }
}
