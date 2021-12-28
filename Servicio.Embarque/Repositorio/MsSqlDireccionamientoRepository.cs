using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Servicio.Embarque.Models;
using Dapper;
using Servicio.Embarque.Models.SolicitudDireccionamiento;
using AccesoDatos.Utils;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using Servicio.Embarque.Models.Usuario;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlDireccionamientoRepository : IDireccionamientoRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlDireccionamientoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SolicitudDireccionamientoResult RegistrarSolicitudDireccionamiento(SolicitudDireccionamientoParameter parameter)
        {
            var result = new SolicitudDireccionamientoResult();

            try
            {
                DataTable dtDocumentos = new DataTable("TM_PDWAC_TY_SOLICITUD_DIRECCIONAMIENTO_DOCUMENTO");
                dtDocumentos.Columns.Add("IDSOLICITUD_DIRECCIONAMIENTO", typeof(int));
                dtDocumentos.Columns.Add("CODDOCUMENTO", typeof(string));
                dtDocumentos.Columns.Add("URL_ARCHIVO", typeof(string));
                dtDocumentos.Columns.Add("NOMBRE_ARCHIVO", typeof(string));

                foreach (Documento row in parameter.Documentos)
                {
                    DataRow drog = dtDocumentos.NewRow();

                    drog["IDSOLICITUD_DIRECCIONAMIENTO"] = 0;
                    drog["CODDOCUMENTO"] = row.CodigoDocumento;
                    drog["URL_ARCHIVO"] = row.UrlArchivo;
                    drog["NOMBRE_ARCHIVO"] = row.NombreArchivo;

                    dtDocumentos.Rows.Add(drog);

                }


                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUD_DIRECCINAMIENTO_CREAR";

                    var queryParameters = new DynamicParameters();
                    
                    queryParameters.Add("@SODI_IDENTIDAD", parameter.Identidad);
                    queryParameters.Add("@SODI_IDUSUARIOREGISTRA", parameter.IdUsuario);
                    queryParameters.Add("@SODI_KEYBL", parameter.KeyBL);
                    queryParameters.Add("@SODI_NROBL", parameter.NroBL);
                    queryParameters.Add("@SODI_TIPODOCUMENTO", parameter.TipoDocumento);
                    queryParameters.Add("@SODI_NUMERO_DOCUMENTO", parameter.NumeroDocumento);
                    queryParameters.Add("@SODI_CODIGO_TAF", parameter.CodigoTaf);
                    queryParameters.Add("@SODI_RAZON_SOCIAL", parameter.RazonSocial);
                    queryParameters.Add("@SODI_CORREO", parameter.Correo);
                    queryParameters.Add("@SODI_CODALMACEN", parameter.CodAlmacen);
                    queryParameters.Add("@SODI_CODMODALIDAD", parameter.CodModalidad);
                    queryParameters.Add("@SODI_FLAG_DIRECCIONAMIENTO_PERMANENTE", parameter.FlagDireccionamientoPermanente);
                    queryParameters.Add("@SODI_ALMACEN", parameter.Almacen);
                    queryParameters.Add("@SODI_CANTIDAD_CNT", parameter.CantidadCtn);
                    queryParameters.Add("@SODI_NAVEVIAJE", parameter.NaveViaje);
                    queryParameters.Add("@SODI_CONSIGNATARIO", parameter.Consignatario);
                    queryParameters.Add("@ListDocumentos", dtDocumentos, DbType.Object);

                    result = cnn.Query<SolicitudDireccionamientoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public SolicitudResult ObtenerSolicitudPorCodigo(string codSol)
        {
            var result = new SolicitudResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_SOLICITUDDIRECCIONAMIENTO_CODIGO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SODI_CODIGO", codSol);

                    result = cnn.Query<SolicitudResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public ListarSolicitudesResult ObtenerSolicitudes(string nroSolicitud, string RucDni, string codEstado)
        {
            var result = new ListarSolicitudesResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUDDIRECCIONAMIENTO_LISTAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SODI_CODIGO", nroSolicitud.Trim());
                    queryParameters.Add("@SODI_RUC", RucDni.Trim());
                    queryParameters.Add("@SODI_ESTADO", codEstado.Trim());

                    result.ListaSolicitudes = cnn.Query<SolicitudResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListarDocumentoResult ObtenerDocumentosPorSolicitud(string codSol)
        {
            var result = new ListarDocumentoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_DOCUMENTOS_SOLICITUDDIRECCIONAMIENTO_LISTAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CODSOL", codSol);

                    result.ListaDocumentos = cnn.Query<DocumentoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListarEventosResult ObtenerEventosPorSolicitud(string codSolicitud)
        {
            var result = new ListarEventosResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_EVENTOS_SOLICITUDDIRECCIONAMIENTO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);

                    result.ListaEventos = cnn.Query<EventosResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public SolicitudDireccionamientoResult ActualizarSolicitudPorCodigo(string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo, int userId)
        {
            var result = new SolicitudDireccionamientoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ACTUALIZAR_DOCUMENTO_DIRECCIONAMIENTO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);
                    queryParameters.Add("@COD_DOCUMENTO", codDocumento);
                    queryParameters.Add("@COD_ESTADO", CodEstado);
                    queryParameters.Add("@COD_ESTADORECHAZO", CodEstadoRechazo);
                    queryParameters.Add("@USERID", userId);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public SolicitudDireccionamientoResult ProcesarSolicitud(string codSolicitud,string CodigoEstado,string CodigoMotivoRechazo, int idUsiarioEvalua)
        {
            var result = new SolicitudDireccionamientoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_PROCESAR_SOLICITUDDIRECCIONAMIENTO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);
                    queryParameters.Add("@COD_ESTADO", CodigoEstado);
                    queryParameters.Add("@COD_MOTIVORECHAZO", CodigoMotivoRechazo);
                    queryParameters.Add("@ID_USUARIO_EVALUA", idUsiarioEvalua);
                    
                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public SolicitudDireccionamientoResult ValidarSolicitudDireccionamiento(string KeyBL)
        {
            var result = new SolicitudDireccionamientoResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUD_DIRECCINAMIENTO_VALIDAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SODI_KEYBL", KeyBL);
                    result = cnn.Query<SolicitudDireccionamientoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

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
