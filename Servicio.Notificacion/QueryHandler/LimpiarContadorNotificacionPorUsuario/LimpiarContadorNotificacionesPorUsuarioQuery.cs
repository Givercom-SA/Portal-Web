using Dapper;
using QueryHandlers.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.QueryHandler.LimpiarContadorNotificacionPorUsuario
{
    public class LimpiarContadorNotificacionesPorUsuarioQuery : IQueryHandler<LimpiarContadorNotificacionesPorUsuarioParameter>
    {
        private readonly string procedureName = "[dbo].[PAA_SP_LIMPIAR_CONTADOR_NOTIFICACIONES_POR_USUARIO_TEST]";
        private readonly string connectionStringPCR;

        public LimpiarContadorNotificacionesPorUsuarioQuery(string connectionStringPCR)
        {
            this.connectionStringPCR = connectionStringPCR;
        }

        public QueryResult Handle(LimpiarContadorNotificacionesPorUsuarioParameter parameters)
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
