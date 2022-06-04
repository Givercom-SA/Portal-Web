using Dapper;
using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.LimpiarNotificacionesPorUsuario
{
    public class LimpiarNotificacionesPorUsuarioQuery : IQueryHandler<LimpiarNotificacionesPorUsuarioParameter>
    {
        private readonly string procedureName = "[dbo].[PAA_SP_LIMPIAR_NOTIFICACIONES_POR_USUARIO_TEST]";
        private readonly string connectionStringPCR;

        public LimpiarNotificacionesPorUsuarioQuery(string connectionStringPCR)
        {
            this.connectionStringPCR = connectionStringPCR;
        }

        public QueryResult Handle(LimpiarNotificacionesPorUsuarioParameter parameters)
        {

            using (var ctx = new SqlConnection(connectionStringPCR))
            {
                var param = new DynamicParameters();
                param.Add("@IN_CODIGO_USUARIO", parameters.CodigoUsuario);

                ctx.Execute(procedureName, param, commandType: CommandType.StoredProcedure, commandTimeout: ctx.ConnectionTimeout);

                return null;
            }

        }
    }
}
