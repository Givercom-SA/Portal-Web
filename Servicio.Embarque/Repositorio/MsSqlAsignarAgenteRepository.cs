using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Servicio.Embarque.Models;
using Dapper;
using Servicio.Embarque.Models.SolicitudFacturacionTerceros;

namespace Servicio.Embarque.Repositorio
{
    public class MsSqlAsignarAgenteRepository: IAsignarAgenteRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlAsignarAgenteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ListarUsuarioEntidadResult ObtenerUsuariosEntidad(int IdPerfil, int IdUsuarioExcluir)
        {
            var result = new ListarUsuarioEntidadResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_USUARIO_ENTIDAD_LISTAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdPerfil", IdPerfil, dbType: DbType.Int32);
                    queryParameters.Add("@IdUsuarioExcluir", IdPerfil, dbType: DbType.Int32);

                    result.Usuarios = cnn.Query<Models.UsuarioEntidad>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListarAsignarAgenteResult ObtenerListaAsignacion(AsignarAgenteListarParameter parameter)
        {
            var result = new ListarAsignarAgenteResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_EMBARQUE_ASIGNACION_ADUANAS_FILTRAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", 0, dbType: DbType.Int32);
                    queryParameters.Add("@IdUsuarioAsigna", parameter.IdUsuarioAsigna, dbType: DbType.Int32);
                    queryParameters.Add("@IdUsuarioAsignado", parameter.IdUsuarioAsignado, dbType: DbType.Int32);
                    queryParameters.Add("@IdEntidadAsigna", parameter.IdEntidadAsigna, dbType: DbType.Int32);
                    queryParameters.Add("@IdEntidadAsignado", parameter.IdEntidadAsignado, dbType: DbType.Int32);
                    queryParameters.Add("@Estado", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@KEYBLD", parameter.KEYBLD, dbType: DbType.String);
                    queryParameters.Add("@NROOT", parameter.NROOT, dbType: DbType.String);
                    queryParameters.Add("@NROBL", parameter.NROBL, dbType: DbType.String);

                    result.AsignacionAduanas = cnn.Query<Models.AsignacionAduanas>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public ListarAsignarAgenteResult ObtenerListaAsignados(AsignarAgenteListarParameter parameter)
        {
            var result = new ListarAsignarAgenteResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_EMBARQUE_ASIGNACION_ADUANAS_FILTRAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Modo", 1, dbType: DbType.Int32);
                    queryParameters.Add("@IdUsuarioAsigna", parameter.IdUsuarioAsigna, dbType: DbType.Int32);
                    queryParameters.Add("@IdUsuarioAsignado", parameter.IdUsuarioAsignado, dbType: DbType.Int32);
                    queryParameters.Add("@IdEntidadAsigna", parameter.IdEntidadAsigna, dbType: DbType.Int32);
                    queryParameters.Add("@IdEntidadAsignado", parameter.IdEntidadAsignado, dbType: DbType.Int32);
                    queryParameters.Add("@Estado", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@KEYBLD", parameter.KEYBLD, dbType: DbType.String);
                    queryParameters.Add("@NROOT", parameter.NROOT, dbType: DbType.String);
                    queryParameters.Add("@NROBL", parameter.NROBL, dbType: DbType.String);

                    result.AsignacionAduanas = cnn.Query<Models.AsignacionAduanas>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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

        public AsignarAgenteResult AsignarAgenteCrear(AsignarAgenteCrearParameter parameter)
        {
            var result = new AsignarAgenteResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_EMBARQUE_ASIGNACION_ADUANAS_CREAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KEYBLD", parameter.KEYBLD, dbType: DbType.String);
                    queryParameters.Add("@NROOT", parameter.NROOT, dbType: DbType.String);
                    queryParameters.Add("@NROBL", parameter.NROBL, dbType: DbType.String);
                    queryParameters.Add("@NRORO", parameter.NRORO, dbType: DbType.String);
                    queryParameters.Add("@EMPRESA", parameter.EMPRESA, dbType: DbType.String);
                    queryParameters.Add("@ORIGEN", parameter.ORIGEN, dbType: DbType.String);
                    queryParameters.Add("@CONDICION", parameter.CONDICION, dbType: DbType.String);
                    queryParameters.Add("@POL", parameter.POL, dbType: DbType.String);
                    queryParameters.Add("@POD", parameter.POD, dbType: DbType.String);
                    queryParameters.Add("@ETAPOD", parameter.ETAPOD, dbType: DbType.String);
                    queryParameters.Add("@EQUIPAMIENTO", parameter.EQUIPAMIENTO, dbType: DbType.String);
                    queryParameters.Add("@MANIFIESTO", parameter.MANIFIESTO, dbType: DbType.String);
                    queryParameters.Add("@COD_LINEA", parameter.COD_LINEA, dbType: DbType.String);
                    queryParameters.Add("@DES_LINEA", parameter.DES_LINEA, dbType: DbType.String);
                    queryParameters.Add("@CONSIGNATARIO", parameter.CONSIGNATARIO, dbType: DbType.String);
                    queryParameters.Add("@COD_INSTRUCCION", parameter.COD_INSTRUCCION, dbType: DbType.String);
                    queryParameters.Add("@DES_INSTRUCCION", parameter.DES_INSTRUCCION, dbType: DbType.String);
                    queryParameters.Add("@IDUSUARIO_ASIGNA", parameter.IdUsuarioAsigna, dbType: DbType.Int32);
                    queryParameters.Add("@IDUSUARIO_ASIGNADO", parameter.IdUsuarioAsignado, dbType: DbType.Int32);
                    queryParameters.Add("@OBSERVACION", parameter.Observacion, dbType: DbType.String);
                    queryParameters.Add("@ESTADO", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@IDUSUARIO_CREA", parameter.IdUsuarioCrea, dbType: DbType.Int32);
                    queryParameters.Add("@IDENTIDAD_ASIGNA", parameter.IdEntidadAsigna, dbType: DbType.Int32);
                    queryParameters.Add("@IDENTIDAD_ASIGNADO", parameter.IdEntidadAsignado, dbType: DbType.Int32);
                    queryParameters.Add("@IdNuevo", direction: ParameterDirection.Output, dbType: DbType.Int32);

                    result = cnn.Query<AsignarAgenteResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    result.IN_CODIGO_RESULTADO = queryParameters.Get<System.Int32>("@IdNuevo");

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }
            return result;
        }

        public AsignarAgenteResult AsignarAgenteCambiarEstado(AsignarAgenteEstadoParameter parameter)
        {
            var result = new AsignarAgenteResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_EMBARQUE_ASIGNACION_ADUANAS_EDITAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ID", parameter.Id, dbType: DbType.Int32);
                    queryParameters.Add("@IDUSUARIO_MODIFICA", parameter.IdUsuarioModifica, dbType: DbType.Int32);
                    queryParameters.Add("@OBSERVACION", parameter.Observacion, dbType: DbType.String);
                    queryParameters.Add("@ESTADO", parameter.Estado, dbType: DbType.String);

                    cnn.Query(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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


        public VerificarAsignacionAgenteAduanasResult VerificarAsignarAgenteAduanas(VerificarAsignacionAgenteAduanasParameter parameter)
        {
            var result = new VerificarAsignacionAgenteAduanasResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_EMBARQUE_ASIGNACION_ADUANAS_VERIFICAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBl", parameter.KEYBLD, dbType: DbType.String);

                    result = cnn.Query<VerificarAsignacionAgenteAduanasResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
 

                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }

            return result;
        }


        public AsignarAgenteDetalle AsignarAgenteDetalle(int Id)
        {
            var result = new AsignarAgenteDetalle();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_EMBARQUE_ASIGNACION_ADUANAS_DETALLE]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Id", Id, dbType: DbType.Int32);
                    result = cnn.Query<AsignarAgenteDetalle>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }




        public RegistrarFacturacionTerceroResult RegistrarFacturacionTercero(RegistrarFacturacionTerceroParameter parameter)
        {
            var result = new RegistrarFacturacionTerceroResult();
            try
            {


                DataTable DtListaCobros = new DataTable("TM_PDWAC_TY_COBRO_PENDIENTE_EMBARQUE");
                DtListaCobros.Columns.Add("EFTD_CODIGO_CONCEPTO", typeof(string));
                DtListaCobros.Columns.Add("EFTD_TIPO_PROVICION", typeof(string));
                DtListaCobros.Columns.Add("EFTD_CONCEPTO", typeof(string));
                DtListaCobros.Columns.Add("EFTD_MONEDA", typeof(string));
                DtListaCobros.Columns.Add("EFTD_IMPORTE", typeof(string));
                DtListaCobros.Columns.Add("EFTD_IGV", typeof(string));
                DtListaCobros.Columns.Add("EFTD_TOTAL", typeof(string));
                DtListaCobros.Columns.Add("EFTD_ID_PROVISION", typeof(string));

                foreach (var item in parameter.CobrosPendientesEmbarque)
                {
                    DataRow drog = DtListaCobros.NewRow();
                    drog["EFTD_CODIGO_CONCEPTO"] = item.ConceptoCodigo;
                    drog["EFTD_TIPO_PROVICION"] = item.Descripcion;
                    drog["EFTD_CONCEPTO"] = item.ConceptoCodigoDescripcion;
                    drog["EFTD_MONEDA"] = item.Moneda;
                    drog["EFTD_IMPORTE"] = item.Importe;
                    drog["EFTD_IGV"] = item.Igv ;
                    drog["EFTD_TOTAL"] = item.Total;
                    drog["EFTD_ID_PROVISION"] = item.ID;
                    DtListaCobros.Rows.Add(drog);
                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_EMBARQUE_FACTURACION_TERCERO]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EMFT_IDENTIDAD", parameter.EMFT_IDENTIDAD, dbType: DbType.Int32);
                    queryParameters.Add("@EMFT_IDUSUARIO", parameter.EMFT_IDUSUARIO, dbType: DbType.Int32);
                    queryParameters.Add("@EMFT_CODIGO_CLIENTE", parameter.EMFT_CODIGO_CLIENTE, dbType: DbType.String);
                    queryParameters.Add("@EMFT_CLIENTE_NOMBRE", parameter.EMFT_CLIENTE_NOMBRE, dbType: DbType.String);
                    queryParameters.Add("@EMFT_CLIENTE_NRODOC", parameter.EMFT_CLIENTE_NRODOC, dbType: DbType.String);
                    queryParameters.Add("@EMFT_IDUSUARIO_CREA", parameter.EMFT_IDUSUARIO_CREA, dbType: DbType.Int32);
                    queryParameters.Add("@EMFT_ARCHIVO", parameter.EMFT_ARCHIVO, dbType: DbType.String);
                    queryParameters.Add("@EMFT_EMBARQUE_KEYBL", parameter.EMFT_EMBARQUE_KEYBL, dbType: DbType.String);
                    queryParameters.Add("@EMFT_EMBARQUE_NROBL", parameter.EMFT_EMBARQUE_NROBL, dbType: DbType.String);
                    queryParameters.Add("@EMFT_TIPOENTIDAD", parameter.TipoEntidad, dbType: DbType.String);
                    queryParameters.Add("@EMFT_AGENTEA_RAZONSOCIAL", parameter.AgenteAduanaRazonSocial, dbType: DbType.String);
                    queryParameters.Add("@EMFT_AGENTEA_TIPODOCUMENTO", parameter.AgenteAduanaTipoDocumento, dbType: DbType.String);
                    queryParameters.Add("@EMFT_AGENTEA_NUMERODOCUMENTO", parameter.AgenteAduanaNumeroDocumento, dbType: DbType.String);
                    queryParameters.Add("@LISTDETALLE", DtListaCobros, dbType: DbType.Object);


                    result = cnn.Query<RegistrarFacturacionTerceroResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();


                }
            }
            catch (Exception ex)
            {
                result.IN_CODIGO_RESULTADO = -1;
                result.STR_MENSAJE_BD = ex.Message;
            }
            return result;
        }

        public ListarFacturacionTerceroResult ObtenerFacutracionTerceros(ListarFacturacionTerceroParameter parameter)
        {
            var result = new ListarFacturacionTerceroResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_EMBARQUE_FACTURACION_TERCERO_LISTAR]";
                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@NroBL", parameter.EMFT_EMBARQUE_NROBL, dbType: DbType.String);
                    queryParameters.Add("@Cliente", parameter.EMFT_CLIENTE, dbType: DbType.String);
                    queryParameters.Add("@Entidad", parameter.EMFT_ENTIDAD, dbType: DbType.String);
                    queryParameters.Add("@Estado", parameter.EMFT_ESTADO, dbType: DbType.String);
                    queryParameters.Add("@IdEntidad", parameter.IdEntidad, dbType: DbType.Int32);
                    
                    result.SolicitudesFacturacionTerceros = cnn.Query<SolicitudFacturacionTercero>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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


        public AsignarAgenteHistorialResult AsignarAgenteHistorial(int IdAsignacionAduana){

            var result = new AsignarAgenteHistorialResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_ASIGNACION_ADUANA_HISTORIAL]";
                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@IdAignacionAduana", IdAsignacionAduana, dbType: DbType.Int32);
           

                    result.Historial = cnn.Query<AgenteAduanasHistorial>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
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
   


        public LeerFacturacionTerceroPorKeyblResult ObtenerFacutracionTercerosPorKeyBl(string keybl)
        {
            var result = new LeerFacturacionTerceroPorKeyblResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_EMBARQUE_FACTURACION_TERCERO_LEER_KEYBL]";
                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@KEYBL", keybl, dbType: DbType.String);
              


                    result.SolicitudesFacturacionTerceros = cnn.Query<SolicitudFacturacionTercero>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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

        public ListarFacturacionTerceroDetalleResult ObtenerFacutracionTerceroDetalle(int IdFacturacionTercero)
        {
            var result = new ListarFacturacionTerceroDetalleResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_EMBARQUE_FACTURACION_TERCERO_DETALLE]";
                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@IdFacturacionTercero", IdFacturacionTercero, dbType: DbType.String);

                    var results = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure);

                    result = results.ReadSingleOrDefault<ListarFacturacionTerceroDetalleResult>();
                    result.ListFacturacionTerceroDetalle = results.Read<SolicitudFacturacionTerceroDetalle>().ToList();
                    result.Historial = results.Read<FacturacionTerceroHistorial>().ToList();


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


        public ListarFacturacionTerceroDetallePorKeyblResult ObtenerFacutracionTerceroDetallePorKeybl(string Keybl)
        {
            var result = new ListarFacturacionTerceroDetallePorKeyblResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[DBO].[TM_PDWAC_SP_EMBARQUE_FACTURACION_TERCERO_DETALLE_POR_KEYBL]";
                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@Keybl", Keybl, dbType: DbType.String);

                    result.ListFacturacionTerceroDetalle = cnn.Query<SolicitudFacturacionTerceroDetalle>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

   
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

        public RegistrarFacturacionTerceroResult ActualizarFacturacionTercero(RegistrarFacturacionTerceroParameter parameter)
        {
            var result = new RegistrarFacturacionTerceroResult();
            try
            {
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_EMBARQUE_FACTURACION_TERCERO_ACTUALIZAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@EMFT_ID", parameter.EMFT_ID, dbType: DbType.Int32);
                    queryParameters.Add("@EMFT_ESTADO", parameter.EMFT_ESTADO, dbType: DbType.String);
                    queryParameters.Add("@EMFT_IDUSUARIO", parameter.EMFT_IDUSUARIO, dbType: DbType.Int32);

                    result = cnn.Query<RegistrarFacturacionTerceroResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

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
