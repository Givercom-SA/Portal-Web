using Dapper;
using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.ObtenerNotificacionesPorUsuario
{
    public class ObtenerNotificacionesPorUsuarioQuery : IQueryHandler<ObtenerNotificacionesPorUsuarioParameter>
    {
        private readonly string procedureName = "[dbo].[PAA_SP_OBTENER_NOTIFICACIONES_POR_USUARIO_TEST]";
        private readonly string connectionStringPCR;

        public ObtenerNotificacionesPorUsuarioQuery(string connectionStringPCR)
        {
            this.connectionStringPCR = connectionStringPCR;
        }

        public QueryResult Handle(ObtenerNotificacionesPorUsuarioParameter parameters)
        {
            var result = new ObtenerNotificacionesPorUsuarioResult();

            using (var ctx = new SqlConnection(connectionStringPCR))
            {
                var param = new DynamicParameters();
                param.Add("@IN_CODIGO_USUARIO", parameters.CodigoUsuario);

                result.Elementos = ctx.Query<NotificacionResult>(procedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: ctx.ConnectionTimeout).ToList();

                return result;
            }
        }
    }
}
