using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;


namespace QueryHandlers.Common.EnvioCorreoError
{
    public class EnvioCorreoErrorQuery : IQueryHandler<EnvioCorreoErrorParameter>
    {
        private readonly string procedureName = "[dbo].[PAA_SP_ENVIAR_CORREO_ERROR_AFPNET]";
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public EnvioCorreoErrorQuery(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public EnvioCorreoErrorQuery()
        {
        }
        public QueryResult Handle(EnvioCorreoErrorParameter parameter)
        {
            var result = new EnvioCorreoErrorResult();           
            var correo = $"{_configuration["EnvioCorreoError:Para"]}";
            var connectionStringPCR = $"{_configuration["ConnectionStrings:AFPnet"]}";
            using (var ctx = new SqlConnection(connectionStringPCR))
            {
                ctx.Open();
                using (SqlCommand command = ctx.CreateCommand())
                {
                    command.CommandText = procedureName;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = ctx.ConnectionTimeout;
                    try
                    {
                        command.Parameters.Add("@VC_TIPO_ASUNTO", SqlDbType.VarChar, 100).Value = parameter.TipoAsunto;
                        command.Parameters.Add("@VC_CORREO_ELECTRONICO", SqlDbType.VarChar, 2000).Value = correo;
                        command.Parameters.Add("@VC_CORREO_MENSAJE", SqlDbType.VarChar, -1).Value = parameter.Mensaje;
                        command.ExecuteNonQuery();
                        result.Resultado = QueryResult.ResultadoTransaccion.Exito;
                    }
                    catch (Exception e)
                    {
                        result.ResultadoCorreo = false;
                        return result;
                    }
                    finally
                    {
                        ctx.Close();
                        ctx.Dispose();
                    }
                }
            }
            return result;
        }
    }
}
