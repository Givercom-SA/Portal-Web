using AccesoDatos.Utils;
using Servicio.Solicitud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Repositorio
{
    public interface ISolicitudRepository
    {
        public ListarSolicitudesResult ObtenerSolicitudes(ListarSolicitudesParameter parameter);

        public ObjetoSolicitudResult ObtenerSolicitudPorCodigo(string codSol);
        public ObjetoSolicitudResult LeerSolicitud(Int64 id);
        public ListarDocumentoResult ObtenerDocumentosPorSolicitud(string codSol);

        public BaseResult ActualizarSolicitudPorCodigo(string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo, int userId);

        public BaseResult ProcesarSolicitud(string codSolicitud);

        public AprobarSolicitudResult AprobarSolicitud(AprobarSolicitudParameter parameter);

        public AprobarSolicitudResult rechazarSolicitud(AprobarSolicitudParameter parameter);

        public ListarEventosResult ObtenerEventosPorSolicitud(string codSolicitud);

        public ListarTipoEntidadResult ObtenerTipoEntidadPorSolicitud(string codSolicitud);
    }
}
