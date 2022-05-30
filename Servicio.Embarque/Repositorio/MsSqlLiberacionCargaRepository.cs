using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.Entidad;
using Servicio.Embarque.Models.LiberacionCarga;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlLiberacionCargaRepository : ILiberacionCargaRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlLiberacionCargaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CrearLiberacionCargaResult CrearLiberacionCarga(CrearLiberacionCargaParameter parameter)
        {
            CrearLiberacionCargaResult result = new CrearLiberacionCargaResult();

            DataTable dtLiberaconCargaDetalle = new DataTable("TM_PDWAC_TY_LIBEERACION_CARGA_DETALLE");
            dtLiberaconCargaDetalle.Columns.Add("LCDE_EMBARQUE_KEYBL", typeof(string));
            dtLiberaconCargaDetalle.Columns.Add("LCDE_NROBL", typeof(string));
            dtLiberaconCargaDetalle.Columns.Add("LCDE_CONSIGNATARIO", typeof(string));


            foreach (LiberacionCargaDetalle row in parameter.Detalles)
            {
                DataRow drog = dtLiberaconCargaDetalle.NewRow();
                drog["LCDE_EMBARQUE_KEYBL"] = row.KeyBl;
                drog["LCDE_NROBL"] = row.NroBl;
                drog["LCDE_CONSIGNATARIO"] = row.Consignatario;


                dtLiberaconCargaDetalle.Rows.Add(drog);

            }

            using (var cnn = new SqlConnection(strConn))
            {
                string spName = "TM_PDWAC_SP_LIBERACION_CARGA_CREAR";

                var queryParameters = new DynamicParameters();

                queryParameters.Add("@LICA_EMBARQUE_KEYBL", parameter.KeyBLD, DbType.String);
                queryParameters.Add("@LICA_NROBL", parameter.NroBL, DbType.String);
                queryParameters.Add("@LICA_ORIGEN", parameter.Origen, DbType.String);
                queryParameters.Add("@LICA_SERVICIO", parameter.Servicio, DbType.String);
                queryParameters.Add("@LICA_EMPRESA_GTRM_CODIGO", parameter.IdEmpresaGtrm, DbType.String);
                queryParameters.Add("@LICA_IDUSUARIO_CREA", parameter.IdUsuarioCrea, DbType.Int32);
                queryParameters.Add("@LICA_IDSESION", parameter.IdSesion, DbType.Int32);
                queryParameters.Add("@LISTDETALLE", dtLiberaconCargaDetalle, DbType.Object);



                result = cnn.Query<CrearLiberacionCargaResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }

            return result;

        }


    }
}
