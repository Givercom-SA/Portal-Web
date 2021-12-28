﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Maestro.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Maestro.Repositorio
{
    public class MsSqlParametros : IParametrosRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlParametros(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ListaParametroResult ObtenerParametroPorIdPadre(int idParam)
        {
            var result = new ListaParametroResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_PARAMETROS_LEER_ID";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID", idParam);

                    result.ListaParametros = cnn.Query<ParametroResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListaDocumentoTipoEntidadResult ObtenerDocumentoPorTipoEntidad(ListarDocumentoTipoEntidadParameter listDocumentoTipoEntidadParameter)
        {
            var result = new ListaDocumentoTipoEntidadResult();

            try
            {
                DataTable DtLis = new DataTable("TM_PDWAC_TY_TIPO_ENTIDAD");
                DtLis.Columns.Add("CODTIPO_ENTIDAD", typeof(string));

                foreach (TipoEntidad row in listDocumentoTipoEntidadParameter.TiposEntidad)
                {
                    DataRow drog = DtLis.NewRow();
                    drog["CODTIPO_ENTIDAD"] = row.CodigoEntidad;
                    DtLis.Rows.Add(drog);
                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_DOCUMENTO_TIPOS_ENTIDAD";
                    var queryParameters = new DynamicParameters();


                     queryParameters.Add("@BrindaCargaFCL", listDocumentoTipoEntidadParameter.BrindaCargaFCL, DbType.Boolean);
                    queryParameters.Add("@AcuerdoCadenaSuministro", listDocumentoTipoEntidadParameter.AcuerdoSeguridadCadenaSuministro, DbType.Boolean);
                    queryParameters.Add("@AgenciamientoAduana", listDocumentoTipoEntidadParameter.SeBrindaAgenciamientodeAduanas, DbType.Boolean);
                    queryParameters.Add("@LIST_TIPOENTIDAD", DtLis,DbType.Object);
                    result.ListaParametros = cnn.Query<DocumentoTipoEntidadResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListaCorreosResult ObtenerCorreosPorPerfil(int _perfil)
        {
            var result = new ListaCorreosResult();

            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_LEER_CORREOS_PERFIL";
                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@USU_PEFL_ID", _perfil, DbType.Int32);
                    result.ListaCorreos = cnn.Query<string>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    result.IN_CODIGO_RESULTADO = 0;

                    if (result.STR_MENSAJE_BD != null)
                        result.IN_CODIGO_RESULTADO = 1;
                }                   
                
            }
            catch(Exception x)
            {
                result.IN_CODIGO_RESULTADO = 2;
                result.STR_MENSAJE_BD = x.Message;
            }

            return result;
        }
    }
}