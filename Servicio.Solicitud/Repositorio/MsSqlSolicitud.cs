using AccesoDatos.Utils;
using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Solicitud.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Repositorio
{
    public class MsSqlSolicitud : ISolicitudRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlSolicitud(IConfiguration configuration)
        {
            _configuration = configuration;
        }        

        public ObjetoSolicitudResult ObtenerSolicitudPorCodigo(string codSol)
        {
            var result = new ObjetoSolicitudResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_SOLICITUD_CODIGO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SOLI_CODIGO", codSol);

                    result = cnn.Query<ObjetoSolicitudResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public ListarSolicitudesResult ObtenerSolicitudes(ListarSolicitudesParameter parameter)
        {
            var result = new ListarSolicitudesResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUD_LEER";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SOLI_CODIGO", parameter.CampoCodSolicitud);
                    queryParameters.Add("@SOLI_RUCDNI", parameter.CampoRuc);
                    queryParameters.Add("@SOLI_ESTADO", parameter.CodEstado);
                    queryParameters.Add("@SOLI_FECHAINGRESO", parameter.FechaIngreso);
                    queryParameters.Add("@SOLI_RAZONSOCIAL", parameter.CampoRazonSocial);
                    queryParameters.Add("@SOLI_NOMBRECONTACTO", parameter.NombreContacto);

                    result.ListaSolicitudes = cnn.Query<ObjetoSolicitudResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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
                    string spName = "TM_PDWAC_SP_DOCUMENTOS_LEER_PORSOLICITUD";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CODSOL", codSol);

                    result.ListaDocumentos = cnn.Query<ObjetoDocumentoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public BaseResult ActualizarSolicitudPorCodigo(string codSolicitud, string codDocumento, string CodEstado, string CodEstadoRechazo, int userId)
        {
            var result = new BaseResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ACTUALIZAR_DOCUMENTO";

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

        public BaseResult ProcesarSolicitud(string codSolicitud)
        {
            var result = new BaseResult();
            string strContrasena = new Utilitario.Seguridad.SeguridadCodigo().GenerarCadenaLongitud(6);

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_PROCESAR_SOLICITUD";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);
                    queryParameters.Add("@CONTRASENIA", new Utilitario.Seguridad.Encrypt().GetSHA256(strContrasena));
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

        public AprobarSolicitudResult AprobarSolicitud(AprobarSolicitudParameter parameter)
        {
            var result = new AprobarSolicitudResult();

            try
            {

                string strContrasenia = new Utilitario.Seguridad.SeguridadCodigo().GenerarCadenaLongitud(6);
                strContrasenia= strContrasenia.ToUpper();

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_APROBAR_SOLICITUD";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", parameter.CodigoSolicitud);
                    queryParameters.Add("@CONTRASENIA", new Utilitario.Seguridad.Encrypt().GetSHA256(strContrasenia));
                    queryParameters.Add("@IdUsuarioEvalua", parameter.IdUsuarioEvalua);

                    result= cnn.Query<AprobarSolicitudResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    
                    
                    result.Contrasenia = strContrasenia;

                  
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -100;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public AprobarSolicitudResult rechazarSolicitud(AprobarSolicitudParameter aprobarSolicitudParameter)
        {
            var result = new AprobarSolicitudResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_RECHAZAR_SOLICITUD";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", aprobarSolicitudParameter.CodigoSolicitud);
                    queryParameters.Add("@MOTIVO_RECHAZO", aprobarSolicitudParameter.Motivorechazo);
                    queryParameters.Add("@IdUsuarioEvalua", aprobarSolicitudParameter.IdUsuarioEvalua);

                    result= cnn.Query<AprobarSolicitudResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

              
                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -100;
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
                    string spName = "TM_PDWAC_SP_OBTENER_EVENTOS_SOLICITUD";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);

                    result.ListaEventos = cnn.Query<ObjetoEventosResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListarTipoEntidadResult ObtenerTipoEntidadPorSolicitud(string codSolicitud)
        {
            var result = new ListarTipoEntidadResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_OBTENER_ENTIDADES_SOLICITUD";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@COD_SOLICITUD", codSolicitud);

                    result.ListaEntidades = cnn.Query<ObjetoEntidadesResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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
    }
}
