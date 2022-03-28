using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlEntidadRepository : IEntidadRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlEntidadRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }







        public ListarEntidadResult ListarTipoEnidad(ListarEntidadParameter parameter)
        {
            var result = new ListarEntidadResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_ENTIDAD_TIPO_LISTAR]";
                    
                    result.Entidades = cnn.Query<EntidadTipo>(spName, commandType: CommandType.StoredProcedure).ToList();
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

        
    }
}
