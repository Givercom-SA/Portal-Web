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
    public class MsSqlSolicitarAcceso : ISolicitarAccesoRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }
      
        public MsSqlSolicitarAcceso(IConfiguration configuration)
        {
            _configuration = configuration;
         
        }

        public SolicitarAccesoResult RegistrarSolicitudAcceso(SolicitarAccesoParameter parameter)
        {
            var result = new SolicitarAccesoResult();

            try
            {


                DataTable DtLis = new DataTable("TM_PDWAC_TY_SOLICITUD_ACCESO_TIPOENTIDAD");
                DtLis.Columns.Add("IDSOLICITUD_ACCESO", typeof(int));
                DtLis.Columns.Add("CODTIPO_ENTIDAD", typeof(string));
              
                foreach (TipoEntidad row in parameter.TipoEntidad)
                {
                    DataRow drog = DtLis.NewRow();

                    drog["IDSOLICITUD_ACCESO"] = 0;
                    drog["CODTIPO_ENTIDAD"] = row.CodigoTipoEntidad;
                
                    DtLis.Rows.Add(drog);

                }


                DataTable dtDocumentos = new DataTable("TM_PDWAC_TY_SOLICITUD_ACCESO_DOCUMENTO");
                dtDocumentos.Columns.Add("IDSOLICITUD_ACCESO", typeof(int));
                dtDocumentos.Columns.Add("CODDOCUMENTO", typeof(string));
                dtDocumentos.Columns.Add("URL_ARCHIVO", typeof(string));
                dtDocumentos.Columns.Add("NOMBRE_ARCHIVO", typeof(string));

                foreach (Documento row in parameter.Documento)
                {
                    DataRow drog = dtDocumentos.NewRow();

                    drog["IDSOLICITUD_ACCESO"] = 0;
                    drog["CODDOCUMENTO"] = row.CodigoDocumento;
                    drog["URL_ARCHIVO"] = row.UrlArchivo;
                    drog["NOMBRE_ARCHIVO"] = row.NombreArchivo;

                    dtDocumentos.Rows.Add(drog);

                }

                string strContrasenia = new Utilitario.Seguridad.SeguridadCodigo().GenerarCadenaLongitud(6);
                strContrasenia = strContrasenia.ToUpper();

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITAR_ACCESO";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SOLI_TIPODOCUMENTO", parameter.SOLI_TIPODOCUMENTO);
                    queryParameters.Add("@SOLI_NUMERO_DOCUMENTO", parameter.SOLI_NUMERO_DOCUMENTO);

                    queryParameters.Add("@SOLI_RAZON_SOCIAL", parameter.SOLI_RAZON_SOCIAL);
                    queryParameters.Add("@SOLI_RELEGAL_NOMBRE", parameter.SOLI_RELEGAL_NOMBRE);
                    queryParameters.Add("@SOLI_RLEGAL_APELLIDO_PATERNO", parameter.SOLI_RLEGAL_APELLIDO_PATERNO);
                    queryParameters.Add("@SOLI_RLEGAL_APELLIDO_MATERNO", parameter.SOLI_RLEGAL_APELLIDO_MATERNO);
                    queryParameters.Add("@SOLI_CORREO", parameter.SOLI_CORREO);

                    queryParameters.Add("@SOLI_ACUERDO_ENDOCE_ELECTRONICO", parameter.SOLI_ACUERDO_ENDOCE_ELECTRONICO);
                    queryParameters.Add("@SOLI_BRINDA_AGENCIAMIENTO_ADUANA", parameter.SOLI_BRINDA_AGENCIAMIENTO_ADUANA);
                    
                    queryParameters.Add("@SOLI_BRINDAOPE_CARGA_FCL", parameter.SOLI_BRINDAOPE_CARGA_FCL);
                    queryParameters.Add("@SOLI_ACUERDO_SEGUR_CADENA_SUMINI", parameter.SOLI_ACUERDO_SEGUR_CADENA_SUMINI);
                    queryParameters.Add("@SOLI_DECLARA_JURADA_VERACIDAD_INFO", parameter.SOLI_DECLARA_JURADA_VERACIDAD_INFO);

                    queryParameters.Add("@SOLI_PROCESO_FACTURACION", parameter.ProcesoFacturacion);
                    queryParameters.Add("@SOLI_TERMIN_CONDICION_GEN_CONTRA", parameter.TerminoCondicionGeneralContraTCGC);
                    queryParameters.Add("@SOLI_CODIGO_SUNAT", parameter.CodigoSunat);
                    queryParameters.Add("@CONTRASENIA", new Utilitario.Seguridad.Encrypt().GetSHA256(strContrasenia) );

                    queryParameters.Add("@SOLI_TIPO_REGISTRO", parameter.TipoRegistro);
                    queryParameters.Add("@CODIGO_USUARIO_REGISTRAR", parameter.IdUsuarioCreaModifica);
                    queryParameters.Add("@IdEntidadActualizar", parameter.IdEntidad);
                    queryParameters.Add("@ListTipoEndida", DtLis,DbType.Object);
                    queryParameters.Add("@ListdDocumentos", dtDocumentos, DbType.Object);


                 

                    result = cnn.Query<SolicitarAccesoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    result.IN_CODIGO_RESULTADO = 0;
                    result.Contrasenia = strContrasenia;
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


        public VerificarSolicitudAccesoResult VerificarSolicitudAcceso(VerificarSolicitudAccesoParameter parameter)
        {
            var result = new VerificarSolicitudAccesoResult();

            try
            {


                


                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_SOLICITARACCESO_VALIDAR_DOC_O_CORREO]";

                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@CodTipoDocumenti", parameter.CodigoTipoDocumento, size: 4);
                    queryParameters.Add("@NumeroDocumento", parameter.NumeroDocumento, size: 20);
                    queryParameters.Add("@Correo", parameter.Correo, size: 100);
                    queryParameters.Add("@CodigoRespuesta", dbType:DbType.Int32, direction:ParameterDirection.Output);
                    queryParameters.Add("@MensajeRespuesta", dbType: DbType.String, direction: ParameterDirection.Output, size:500);

                    result = cnn.Query<VerificarSolicitudAccesoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    
                    result.IN_CODIGO_RESULTADO = queryParameters.Get<System.Int32>("@CodigoRespuesta");
                    result.STR_MENSAJE_BD = queryParameters.Get<System.String>("@MensajeRespuesta");


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
