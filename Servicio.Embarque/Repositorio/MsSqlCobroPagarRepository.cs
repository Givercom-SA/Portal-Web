using AccesoDatos.Utils;
using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.CobrosPagar;
using Servicio.Embarque.Models.SolicitudFacturacion;
using Servicio.Embarque.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlCobroPagarRepository : ICobroPagarRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlCobroPagarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CobrosPagarResult ActualizarCobrosPagar(CobrosPagarParameter parameter)
        {
            var result = new CobrosPagarResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_EMBARQUE_COBRO_PAGAR_ACTUALIZAR";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EMCP_ID", parameter.Id, dbType: DbType.Int32);
                    queryParameters.Add("@EMCP_ESTADO", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@EMCP_IDUSUARIO", parameter.IdUsuario, dbType: DbType.Int32);

                    result = cnn.Query<CobrosPagarResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }
            return result;
        }

        public CobrosPagarResult CrearCobrosPagar(CobrosPagarParameter parameter)
        {
            var result = new CobrosPagarResult();

            try
            {
                DataTable dtClienteProvision = new DataTable("TM_PDWAC_TY_EMBARQUE_COBRO_PAGAR");
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_RUBRO_CCODIGO", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_CONCEP_CCODIGO", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_DESCRIPCION", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_CONCEPTO", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_MONEDA", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_IMPORTE", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_IGV", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_TOTAL", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_FLAG_ASIGNACION", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_NROBL", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_KEYBL", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_BLPAGAR", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_EMBARQUE_IDBLPAGAR", typeof(string));
                dtClienteProvision.Columns.Add("EMCP_IDPROVISION", typeof(int));
                

                foreach (CobrosPagarDetalle row in parameter.ListaDetalle)
                {
                    DataRow drog = dtClienteProvision.NewRow();

                    drog["EMCP_EMBARQUE_RUBRO_CCODIGO"] = row.RubroCodigo;
                    drog["EMCP_EMBARQUE_CONCEP_CCODIGO"] = row.ConceptoCodigo;
                    drog["EMCP_EMBARQUE_DESCRIPCION"] = row.Descripcion;
                    drog["EMCP_EMBARQUE_CONCEPTO"] = row.Concepto;
                    drog["EMCP_EMBARQUE_MONEDA"] = row.Moneda;
                    drog["EMCP_EMBARQUE_IMPORTE"] = row.Importe;
                    drog["EMCP_EMBARQUE_IGV"] = row.IGV;
                    drog["EMCP_EMBARQUE_TOTAL"] = row.Total;
                    drog["EMCP_EMBARQUE_FLAG_ASIGNACION"] = row.FlagAsignacion;
                    drog["EMCP_EMBARQUE_NROBL"] = row.NroBL;
                    drog["EMCP_EMBARQUE_KEYBL"] = row.KeyBl;
                    drog["EMCP_EMBARQUE_BLPAGAR"] = row.BlPagar;
                    drog["EMCP_EMBARQUE_IDBLPAGAR"] = row.IdBlPagar;
                    drog["EMCP_IDPROVISION"] = row.IdProvision;

                    dtClienteProvision.Rows.Add(drog);

                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_EMBARQUE_COBRO_PAGAR_CREAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EMCP_EMBARQUE_KEYBLD", parameter.KeyBLD);
                    queryParameters.Add("@EMCP_EMBARQUE_BL", parameter.BL);
                    queryParameters.Add("@EMCP_EMBARQUE_BLNIETO", parameter.BLNieto);
                    queryParameters.Add("@EMCP_IDUSUARIO_CREA", parameter.IdUsuario);
                    queryParameters.Add("@ListCobroPagar", dtClienteProvision, DbType.Object);

                    result = cnn.Query<CobrosPagarResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }

        public ListarCobrosPagarResult ObtenerCobrosPagar(string KeyBLD, string BL, string BLNieto, string ConceptoCodigo)
        {
            var result = new ListarCobrosPagarResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_EMBARQUE_COBRO_PAGAR_LISTAR";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EMCP_EMBARQUE_KEYBLD", KeyBLD);
                    queryParameters.Add("@EMCP_EMBARQUE_BL", BL);
                    queryParameters.Add("@EMCP_EMBARQUE_BLNIETO", BLNieto);
                    queryParameters.Add("@EMCP_EMBARQUE_CONCEP_CCODIGO", ConceptoCodigo);

                    result.ListaCobrosPagar = cnn.Query<CobrosPagar>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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



        public CobrosPagarPadreKeyBLResult ObtenerEmbarquePadrePorKeyBl(string KeyBLD)
        {
            var result = new CobrosPagarPadreKeyBLResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_TB_EMBARQUE_COBRO_PAGAR_GETPADRE_KEYBL";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EMBARQUE_KEYBL", KeyBLD);
   

                    result.EmbarquePadreKeyBl = cnn.Query<EmbarqueCobroPadre>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.STR_MENSAJE_BD = "";


                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
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


        public ListarProvisionFacturacionTerceroResult ObtenerProvisionEmbarqueFacturacionTercerto(ListarProvisionFacturacionTerceroParameter parameter)
        {
            var result = new ListarProvisionFacturacionTerceroResult();

            try
            {

                DataTable dtClienteProvision = new DataTable("TM_PDWAC_TY_PROVISION");
                dtClienteProvision.Columns.Add("PROV_EMBARQUE_NROBL", typeof(string));
                dtClienteProvision.Columns.Add("PROV_EMBARQUE_KEYBL", typeof(string));
                dtClienteProvision.Columns.Add("PROV_IDPROVISION", typeof(int));
                


                foreach (Provision row in parameter.Provision)
                {
                    DataRow drog = dtClienteProvision.NewRow();

                    drog["PROV_EMBARQUE_NROBL"] = row.NroBl;
                    drog["PROV_EMBARQUE_KEYBL"] = row.keyBl;
                    drog["PROV_IDPROVISION"] = row.IdProvision;
                

                    dtClienteProvision.Rows.Add(drog);

                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_FACTURACION_TERCERO_LISTAR_POR_PROVISION";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBl", parameter.KeyBl);
                   
                    queryParameters.Add("@ListaProvisiones", dtClienteProvision, DbType.Object);


                    result.PrivisionFacturacionTercero = cnn.Query<ProvisionFacturacionTercero>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

    


    }
}
