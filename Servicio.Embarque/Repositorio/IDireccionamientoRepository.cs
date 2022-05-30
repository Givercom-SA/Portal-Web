using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccesoDatos.Utils;
using Microsoft.AspNetCore.Mvc;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using Servicio.Embarque.Models.Usuario;

namespace Servicio.Embarque.Repositorio
{
    public interface IDireccionamientoRepository
    {
        public SolicitudDireccionamientoResult RegistrarSolicitudDireccionamiento(SolicitudDireccionamientoParameter parameter);
        public SolicitudResult ObtenerSolicitudPorCodigo(string codSol);
        public ListarSolicitudesResult ObtenerSolicitudes(string nroSolicitud, string RucDni, string codEstado);
        public ListarDocumentoResult ObtenerDocumentosPorSolicitud(string codSol);
        public ListarEventosResult ObtenerEventosPorSolicitud(string codSolicitud);
        public SolicitudDireccionamientoResult ActualizarSolicitudPorCodigo(string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo, int userId);
        public SolicitudDireccionamientoResult ProcesarSolicitud(string codSolicitud, string CodigoEstado, string CodigoMotivoRechazo,int idUsiarioEvalua);
        public ListarUsuarioResult ObtenerUsuariosPorPerfil(int IdPerfil);
        public SolicitudDireccionamientoResult ValidarSolicitudDireccionamiento(string KeyBL);
        public CrearDireccionamientoPermanenteResult CrearDireccionamientoPermanente(CrearDireccionamientoPermanenteParameter parameter);
    }
}
