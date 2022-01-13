﻿using AccesoDatos.Utils;
using Servicio.Embarque.Models.GestionarMemo;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using Servicio.Embarque.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public interface IMemoRepository
    {
        public void ActualizarNotificacionMemo(string _keyBld, int _idUsuario);
        public NotificacionesMemoResult CrearNotificacionMemo(NotificacionMemoParameter parameter);
        public NotificacionesMemoResult ProcesarNotificacionesMemo();
        public ProcesarSolicitudMemoResult CrearSolicitudMemo(SolicitudMemoParameter parameter);
        public SolicitudMemoResult ObtenerSolicitudMemoPorCodigo(string codSol);
        public ListarSolicitudesMemoResult ObtenerSolicitudesMemo(ListarSolicitudesMemoParameter parameter);
        public ListarDocumentoMemoResult ObtenerDocumentosSolicitudMemo(string codSol);
        public ListarEventosMemoResult ObtenerEventosSolicitudMemo(string codSolicitud);
        public DocumentoEstadoMemoResult ActualizarSolicitudMemo(DocumentoEstadoMemoParameter parameter);

        public SolicitudMemoEstadoresult ProcesarSolicitudMemo(string codSolicitud, int IdUsuarioEvalua, string codigoEstadoEvalua, string codigoMotivoRechazo);
        public ListarUsuarioResult ObtenerUsuariosPorPerfil(int IdPerfil);
        public NotificacionesMemoResult VerificarNotificacionMemo(NotificacionMemoParameter parameter);
    }
}
