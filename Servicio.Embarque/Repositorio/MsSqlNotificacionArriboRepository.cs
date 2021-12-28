using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlNotificacionArriboRepository : INotificacionArriboRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlNotificacionArriboRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ActualizarEstadoNotificacion(string _keyBld, int _idUsuario, string tipoDoc)
        {
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_ACTUALIZAR_NOTIFICACION";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBld", _keyBld, dbType: DbType.String);
                    queryParameters.Add("@IdUsuario", _idUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@tipoDocumento", tipoDoc, dbType: DbType.String);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {            }
        }

        public string RegistrarNotificacionArribo(RegistrarNotificacionArriboParameter parameter)
        {
            string mensaje = string.Empty;

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_REGISTRAR_NOTIFICACION";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBld", parameter.keyBld, dbType: DbType.String);
                    queryParameters.Add("@NumeracionEmbarque", parameter.NumeracionEmbarque, dbType: DbType.String);
                    queryParameters.Add("@CodigoGtrmEmpresa", parameter.CodigoGtrmEmpresa, dbType: DbType.String);
                    queryParameters.Add("@IdUsuario", parameter.idUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@tipoDoc", parameter.tipoDocumento, dbType: DbType.String);
                    queryParameters.Add("@Mensaje", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure);

                    mensaje = queryParameters.Get<string>("@Mensaje");
                }
            }
            catch (Exception ex)
            {
                mensaje = "Ocurrio un error inesperado, por favor voler a intentar.";
            }

            return mensaje;
        }

        public ListarNotificacionesPendientesResult ListaNotificacionesArriboPendientes()
        {
            var result = new ListarNotificacionesPendientesResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_LEER_NOTIFICACION]";
                    
                    result.ListaNotificacionesPendientes = cnn.Query<NotificacionesPendientesResult>(spName, commandType: CommandType.StoredProcedure).ToList();
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

        public ListaExpressReleaseAceptadasResult ListaExpressReleaseAceptadas()
        {
            var result = new ListaExpressReleaseAceptadasResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TW_PDWAC_SP_LEER_ACEPTA_EXPRESS_RELEASE]";

                    result.listaExpressRelease = cnn.Query<ExpressReleaseAceptadaResult>(spName, commandType: CommandType.StoredProcedure).ToList();
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

        public string RegistrarExpressReleaseAceptadas(string keyBld, string nroBl, int idUsuario)
        {
            string mensaje = string.Empty;

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TW_PDWAC_SP_REGISTRAR_ACEPTA_EXPRESS_RELEASE";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@AER_KEYBD", keyBld, dbType: DbType.String);
                    queryParameters.Add("@AER_NROBL", nroBl, dbType: DbType.String);
                    queryParameters.Add("@AER_IDUSUARIO_CREA", idUsuario, dbType: DbType.Int32);
                    queryParameters.Add("@Mensaje", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure);

                    mensaje = queryParameters.Get<string>("@Mensaje");
                }
            }
            catch (Exception ex)
            { mensaje = ex.Message; }

            return mensaje;
        }
    }
}
