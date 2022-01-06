using AccesoDatos.Utils;
using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models.GestionarMemo;
using Servicio.Embarque.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlMemoRepository : IMemoRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlMemoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ActualizarNotificacionMemo(string _keyBld, int _idUsuario)
        {
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_NOTIFICACION_MEMO_ACTUALIZAR";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBld", _keyBld, dbType: DbType.String);
                    queryParameters.Add("@IdUsuario", _idUsuario, dbType: DbType.Int32);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {            }
        }

        public NotificacionesMemoResult CrearNotificacionMemo(NotificacionMemoParameter parameter)
        {
            var result = new NotificacionesMemoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_NOTIFICACION_MEMO_CREAR";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBld", parameter.KeyBLD, dbType: DbType.String);
                    queryParameters.Add("@IdUsuario", parameter.IdUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@FlagVigente", parameter.FlagVigente, dbType: DbType.String);
                    queryParameters.Add("@FlagRuteado", parameter.FlagRuteado, dbType: DbType.String);
                    queryParameters.Add("@NombreArchivo", parameter.NombreArchivo, dbType: DbType.String);

                    result = cnn.Query<NotificacionesMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public NotificacionesMemoResult ProcesarNotificacionesMemo()
        {
            var result = new NotificacionesMemoResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_NOTIFICACION_MEMO_LISTAR]";

                    result.ListaNotificaciones = cnn.Query<NotificacionesMemo>(spName, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ProcesarSolicitudMemoResult CrearSolicitudMemo(SolicitudMemoParameter parameter)
        {
            var result = new ProcesarSolicitudMemoResult();

            try
            {
                DataTable dtDocumentos = new DataTable("TM_PDWAC_TY_SOLICITUD_MEMO_DOCUMENTO");
                dtDocumentos.Columns.Add("IDSOLICITUD_MEMO", typeof(int));
                dtDocumentos.Columns.Add("CODDOCUMENTO", typeof(string));
                dtDocumentos.Columns.Add("URL_ARCHIVO", typeof(string));
                dtDocumentos.Columns.Add("NOMBRE_ARCHIVO", typeof(string));

                foreach (DocumentoMemo row in parameter.Documentos)
                {
                    DataRow drog = dtDocumentos.NewRow();

                    drog["IDSOLICITUD_MEMO"] = 0;
                    drog["CODDOCUMENTO"] = row.CodigoDocumento;
                    drog["URL_ARCHIVO"] = row.UrlArchivo;
                    drog["NOMBRE_ARCHIVO"] = row.NombreArchivo;

                    dtDocumentos.Rows.Add(drog);

                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUD_MEMO_CREAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SOME_KEYBL", parameter.KeyBL);
                    queryParameters.Add("@SOME_CORREO", parameter.Correo);
                    queryParameters.Add("@SOME_IDUSUARIO_CREA", parameter.IdUsuarioCrea);
                    queryParameters.Add("@SOME_NROBL", parameter.NroEmbarque);
                    queryParameters.Add("@ListDocumentos", dtDocumentos, DbType.Object);

                    result = cnn.Query<ProcesarSolicitudMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public SolicitudMemoResult ObtenerSolicitudMemoPorCodigo(string codSol)
        {
            var result = new SolicitudMemoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_SOLICITUDMEMO_CODIGO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SOME_CODIGO", codSol);

                    result = cnn.Query<SolicitudMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.STR_MENSAJE_BD != null)
                        result.IN_CODIGO_RESULTADO = 1;

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarSolicitudesMemoResult ObtenerSolicitudesMemo(string nroSolicitud, string codEstado, string strRuc)
        {
            var result = new ListarSolicitudesMemoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUDMEMO_LISTAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("SOME_CODIGO", nroSolicitud);
                    queryParameters.Add("SODI_RUC", strRuc);
                    queryParameters.Add("SODI_ESTADO", codEstado);
                 

                    result.ListaSolicitudes = cnn.Query<SolicitudMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.STR_MENSAJE_BD != null)
                        result.IN_CODIGO_RESULTADO = 1;

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarDocumentoMemoResult ObtenerDocumentosSolicitudMemo(string codSol)
        {
            var result = new ListarDocumentoMemoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_DOCUMENTOS_SOLICITUDMEMO_LISTAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CODSOL", codSol);

                    result.ListaDocumentos = cnn.Query<DocumentoMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.STR_MENSAJE_BD != null)
                        result.IN_CODIGO_RESULTADO = 1;

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarEventosMemoResult ObtenerEventosSolicitudMemo(string codSolicitud)
        {
            var result = new ListarEventosMemoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_EVENTOS_SOLICITUDMEMO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);

                    result.ListaEventos = cnn.Query<EventosMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.STR_MENSAJE_BD != null)
                        result.IN_CODIGO_RESULTADO = 1;

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public DocumentoEstadoMemoResult ActualizarSolicitudMemo(DocumentoEstadoMemoParameter parameter)
        {
            var result = new DocumentoEstadoMemoResult();

            try
            {

                DataTable dtDocumentos = new DataTable("TM_PDWAC_TY_SOLICITUD_MEMO_DOCUMENTO_ESTADO");
                dtDocumentos.Columns.Add("ID_SOLICITUD_DEMO", typeof(int));
                dtDocumentos.Columns.Add("CODIGO_DOCUMENTO", typeof(string));
                dtDocumentos.Columns.Add("CODIGO_ESTADO", typeof(string));
                dtDocumentos.Columns.Add("CODIGO_ESTADO_RECHAZO", typeof(string));

             
                foreach (DocumentoEstado row in parameter.Documentos)
                {
                    DataRow drog = dtDocumentos.NewRow();

                    drog["ID_SOLICITUD_DEMO"] = parameter.IdSolicitud;
                    drog["CODIGO_DOCUMENTO"] = row.CodigoDocumento;
                    drog["CODIGO_ESTADO"] = row.CodigoEstado;
                    drog["CODIGO_ESTADO_RECHAZO"] = row.CodigoEstadoRechazo;

                    dtDocumentos.Rows.Add(drog);

                }


                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ACTUALIZAR_DOCUMENTO_MEMO";

                    var queryParameters = new DynamicParameters();
                    
                    queryParameters.Add("@USERID", parameter.IdUsuarioEvalua);
                    queryParameters.Add("@COD_SOLICITUD", parameter.CodigoSolicitud);
                    queryParameters.Add("@ID_SOLICITUD", parameter.IdSolicitud);
                    queryParameters.Add("@ListDocumentos", dtDocumentos, DbType.Object);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.STR_MENSAJE_BD ="";


                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -100;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public SolicitudMemoEstadoresult ProcesarSolicitudMemo(string codSolicitud, int IdUsuarioEvalua, String codigoEstadoEvalua, string codigoMotivoRechazo)
        {
            var result = new SolicitudMemoEstadoresult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_PROCESAR_SOLICITUDMEMO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud, DbType.String);
                    queryParameters.Add("@IdUsuarioEvalua", IdUsuarioEvalua,DbType.Int32);
                    queryParameters.Add("@CodigoEstadoEvalua", codigoEstadoEvalua, DbType.String);
                    queryParameters.Add("@CodigoRechazo", codigoMotivoRechazo, DbType.String);
                    result = cnn.Query<SolicitudMemoEstadoresult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
              
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarUsuarioResult ObtenerUsuariosPorPerfil(int IdPerfil)
        {
            var result = new ListarUsuarioResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_USUARIOS_PORPERFIL";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@TipoPerfil", "TP01");
                    queryParameters.Add("@IdPerfil", IdPerfil);

                    result.Usuarios = cnn.Query<Usuario>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.STR_MENSAJE_BD != null)
                        result.IN_CODIGO_RESULTADO = 1;

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public NotificacionesMemoResult VerificarNotificacionMemo(NotificacionMemoParameter parameter)
        {
            var result = new NotificacionesMemoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_NOTIFICACION_MEMO_VERIFICAR";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBld", parameter.KeyBLD, dbType: DbType.String);
                    result = cnn.Query<NotificacionesMemoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }
    }
}
