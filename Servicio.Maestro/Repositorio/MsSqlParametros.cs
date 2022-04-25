﻿using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Maestro.Models;
using Servicio.Maestro.Models.LibroReclamo;
using Servicio.Maestro.Models.Tarifario;
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
         

            return result;
        }

        public ListaDocumentoTipoEntidadResult ObtenerDocumentoPorTipoEntidad(ListarDocumentoTipoEntidadParameter listDocumentoTipoEntidadParameter)
        {
            var result = new ListaDocumentoTipoEntidadResult();

         
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
        

            return result;
        }

        public ListaCorreosResult ObtenerCorreosPorPerfil(int _perfil)
        {
            var result = new ListaCorreosResult();

         
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
                
         

            return result;
        }


        public Servicio.Maestro.Models.LibroReclamo.ListaEmpresasResult ListEmpresasReclamo(ListaEmpresasParameter parametro) {

            var result = new Servicio.Maestro.Models.LibroReclamo.ListaEmpresasResult();

          
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[RECLAMO_SP_EMPRESAS_LISTAR]";
                    
                    result.Empresas = cnn.Query<EmpresaReclamo>(spName, null, commandType: CommandType.StoredProcedure).ToList();



                    result.IN_CODIGO_RESULTADO = 0;

                }

         

            return result;
        }
        public Servicio.Maestro.Models.LibroReclamo.ListaUnidadNegocioXEmpresasResult ListarUnidadNegocioXEmpresa(ListaUnidadNegocioXEmpresaParameter parametro) {
            var result = new Servicio.Maestro.Models.LibroReclamo.ListaUnidadNegocioXEmpresasResult();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[RECLAMO_SP_UNIDAD_NEGOCIO_POR_EMPRESA]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CodEmpresa", parametro.CodigoEmpresa, DbType.String);

                    var readyResult = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure);
                    result.UnidadNegociosReclamo = readyResult.Read<UnidadNegocioReclamo>().ToList();
                   result.TiposDocumentos = readyResult.Read<TipoDocumentoReclamo>().ToList();
                     
                    

                    result.IN_CODIGO_RESULTADO = 0;

                }

        

            return result;
        }

        public Servicio.Maestro.Models.LibroReclamo.RegistraReclamoResult RegistrarReclamo(RegistrarReclamoParameter parametro) {
            var result = new Servicio.Maestro.Models.LibroReclamo.RegistraReclamoResult();

          
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[RECLAMO_SP_REGISTRAR]";
                    var queryParameters = new DynamicParameters();
                    /*
        
                     */
                    queryParameters.Add("@COD_TIPO_FORMULARIO", parametro.CodigoTipoFormulario, DbType.String);
                    queryParameters.Add("@RUC", parametro.Ruc, DbType.String);
                    queryParameters.Add("@RAZON_SOCIAL", parametro.RazonSocial, DbType.String);
                    queryParameters.Add("@NOMBRE", parametro.Nombre, DbType.String);
                    queryParameters.Add("@EMAIL", parametro.Email, DbType.String);
                    queryParameters.Add("@NRO_CELULAR", parametro.Celular, DbType.String);
                    queryParameters.Add("@FEC_INCIDENCIA", parametro.FechaIncidencia, DbType.Date);
                    queryParameters.Add("@COD_EMPRESA", parametro.CodigoEmpresa, DbType.String);
                    queryParameters.Add("@COD_UNIDAD_NEGOCIO", parametro.CodigoUnidadNegocio, DbType.String);
                    queryParameters.Add("@COD_TIPO_DOCUMENTO", parametro.CodigoTipoFormulario, DbType.String);
                    queryParameters.Add("@OBSERVACION", parametro.Observacion, DbType.String);

                    result = cnn.Query<RegistraReclamoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                
                 

                }

          

            return result;
        }

        public ListarTarifarioResult ListarTarifario(ListarTarifarioParameter parametro) {
            var result = new ListarTarifarioResult();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TARIFARIO_SP_LISTAR_TARIFARIO_WEB";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@DES_RUBRO", parametro.Rubro, DbType.String);
                    queryParameters.Add("@DES_SERVICIO", parametro.Servicio, DbType.String);
                    queryParameters.Add("@FEC_INICIO", parametro.FechaInicio, DbType.DateTime);
                    queryParameters.Add("@FEC_FIN", parametro.FechaFin, DbType.DateTime);
                    result.Tarifarios = cnn.Query<Tarifario>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
           
                    result.IN_CODIGO_RESULTADO = 0;

                }

          

            return result;
        }
    
    }
}

