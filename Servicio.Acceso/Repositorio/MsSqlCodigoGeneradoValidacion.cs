using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Acceso.Models;
using Servicio.Acceso.Models.LoginUsuario;
using Servicio.Acceso.Models.SolicitarAcceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Repositorio
{
    public class MsSqlCodigoGeneradoValidacion : ICodigoGeneradoValidacionRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlCodigoGeneradoValidacion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CodigoGeneradoValidacionResult GenerarCodigoValidacion(CodigoGeneradoValidacionParameter parameter)
        {
            var result = new CodigoGeneradoValidacionResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_GENERAR_CODIGO_VALIDACION";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("CODTIPO_DOCUMENTO", parameter.CODTIPO_DOCUMENTO);
                    queryParameters.Add("NUMERO_DOCUMENTO", parameter.NUMERO_DOCUMENTO);
                    queryParameters.Add("CODIGO_VERIFICACION", parameter.CODIGO_VERIFICACION);

                    result = cnn.Query<CodigoGeneradoValidacionResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public CodigoGeneradoValidacionResult GenerarCodigoValidacionCorreo(CodigoGeneradoValidacionParameter parameter)
        {
            var result = new CodigoGeneradoValidacionResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_GENERAR_CODIGO_VALIDACION_CORREO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("CORREO", parameter.CORREO);
                    queryParameters.Add("CODIGO_VERIFICACION", parameter.CODIGO_VERIFICACION);

                    result = cnn.Query<CodigoGeneradoValidacionResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public VerificarCodigoValidacionResult VerificarCodigoValidacion(VerificarCodigoValidacionParameter parameter)
        {
            var result = new VerificarCodigoValidacionResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_VALIDAR_CODIGO_VALIDACION";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("CODTIPO_DOCUMENTO", parameter.CODTIPO_DOCUMENTO);
                    queryParameters.Add("NUMERO_DOCUMENTO", parameter.NUMERO_DOCUMENTO);
                    queryParameters.Add("CODIGO_VERIFICACION", parameter.CODIGO_VERIFICACION);

                    result = cnn.Query<VerificarCodigoValidacionResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public VerificarCodigoValidacionResult VerificarCodigoValidacionCorreo(VerificarCodigoValidacionParameter parameter)
        {
            var result = new VerificarCodigoValidacionResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_VALIDAR_CODIGO_VALIDACION_CORREO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("CORREO", parameter.CORREO);
                    queryParameters.Add("CODIGO_VERIFICACION", parameter.CODIGO_VERIFICACION);

                    result = cnn.Query<VerificarCodigoValidacionResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
