using AccesoDatos.Utils;
using Dapper;
using Microsoft.Extensions.Configuration;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.GestionarMemo;
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
    public class MsSqlSolicitudFacturacionRepository : ISolicitudFacturacionRepository
    {
        private IConfiguration _configuration;
        private string strConn { get { return _configuration.GetConnectionString("mssqldb"); } }

        public MsSqlSolicitudFacturacionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ListarSolicitudFacturacionBandejaResult ObtenerFacturacionListaBandeja(ListarSolicitudFacturacionBandejaParameter parameter)
        {
            var result = new ListarSolicitudFacturacionBandejaResult();

         
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUD_FACTURACION_BANDEJA";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@NrBl", parameter.NroBl, dbType: DbType.String);
                    queryParameters.Add("@Estado", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@CodigoSolicitud", parameter.CodigoFacturacion, dbType: DbType.String);
                    queryParameters.Add("@NroDocumentoConsignatario", parameter.NroDocumentoConsigntario, dbType: DbType.String);
                    queryParameters.Add("@FechaRegistro", parameter.FechaRegistro, dbType: DbType.Date);

                    result.SolicitudesFacturacion = cnn.Query<SolicitudFacturacion>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();
                    
                }
         

            return result;
        }

        public SolicitarFacturacionEstadoResult RegistrarSolicitudFacturacionEstado(SolicitarFacturacionEstadoParameter parameter)
        {
            var result = new SolicitarFacturacionEstadoResult();

         
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_SOLICITUD_FACTURACION_ESTADO_ACTUALIZAR]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SOFA_ESTADO", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@SOFA_IDSOLICITUDFACTURACION", parameter.IdSolicitudFacturacion, dbType: DbType.Int32);
                    queryParameters.Add("@SOFA_IDUSUAIO_EVALUA", parameter.IdUsuarioEvalua, dbType: DbType.Int32 );
                    queryParameters.Add("@SOFA_OBSERVACION_RECHAZO", parameter.ObservacionRechazo, dbType: DbType.String);
                    queryParameters.Add("@SOFA_IDSOLICITUD_TAF", parameter.IdSolicitudTAFF, dbType: DbType.String);
                    result = cnn.Query<SolicitarFacturacionEstadoResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
           
            return result;
        }

        public NotificacionFacturacionResult NotificarFacturacion(NotificacionFacturacionParameter parameter)
        {
            var result = new NotificacionFacturacionResult();

           
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_NOTIFICACION_FACTURACION";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBL", parameter.KEYBLD, dbType: DbType.String);
                    queryParameters.Add("@Estado", parameter.Estado, dbType: DbType.String);
                    queryParameters.Add("@UsuarioCrea", parameter.IdUsuarioRegistra, dbType: DbType.Int32);
                    result = cnn.Query<NotificacionFacturacionResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }
            

            return result;
        }

  
        public LeerSolicitudFacturacionBandejaResult ObtenerFacturacionBandeja(LeerSolicitudFacturacionBandejaParameter parameter)
        {
            var result = new LeerSolicitudFacturacionBandejaResult();

         
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_SOLICITUD_FACTURACION_DETALLE_BANDEJA]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@IdSolicitudFacturacion", parameter.IdSolicitudFacturacion, dbType: DbType.Int64);
                    
                   var readyResult = cnn.QueryMultiple(spName, queryParameters, commandType: CommandType.StoredProcedure);
                    result.SolicitudFacturacion = readyResult.ReadSingle<SolicitudFacturacion>();
                    if (result.SolicitudFacturacion != null) {
                        result.SolicitudFacturacion.DetalleFacturacion = readyResult.Read<SolicitudFacturacionDetalle>().ToList();
                        result.SolicitudFacturacion.Enventos = readyResult.Read<EventoSolicitudFacturacion>().ToList();
                    }
                    
                }
     

            return result;
        }


        public LeerSolicitudFacturacionKeyBlResult ListarSolicitudFacturacionPorKeyBl(string keyBl) {

            var result = new LeerSolicitudFacturacionKeyBlResult();

         
                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "[dbo].[TM_PDWAC_SP_SOLICITUD_FACTURACION_LISTAR_KEYBL]";
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@KeyBl", keyBl, dbType: DbType.String);

                    result.SolicitudFacturaciones = cnn.Query<SolicitudFacturacion>(spName, queryParameters, commandType: CommandType.StoredProcedure).ToList();

                }


                foreach (var item in result.SolicitudFacturaciones) {
                   var resultDetalle= this.ObtenerFacturacionBandeja(new LeerSolicitudFacturacionBandejaParameter() { IdSolicitudFacturacion=item.IdSolicitudFacturacion});
                    item.DetalleFacturacion = resultDetalle.SolicitudFacturacion.DetalleFacturacion;


                }


         

            return result;
        }

        public SolicitarFacturacionResult SolicituFacturacion(SolicitarFacturacionParameter parameter)
        {
            var result = new SolicitarFacturacionResult();

         

                DataTable dtClienteProvision = new DataTable("TM_PDWAC_TY_SOLICITUD_FACTURACION_DETALLE");
                dtClienteProvision.Columns.Add("ESFD_ID", typeof(int));
                dtClienteProvision.Columns.Add("ESFD_IDSOLICITUDFACTURACION", typeof(int));
                dtClienteProvision.Columns.Add("ESFD_CODIGO_CONCEPTO", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_MONEDA", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_IMPORTE", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_IGV", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_TOTAL", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_DESCRIPCION_CONCEPTO", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_DESCRIPCION_PROVISION", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_IDPROVISION", typeof(int));
                dtClienteProvision.Columns.Add("ESFD_RUBROCODIGO", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_KEYBL", typeof(string));
                dtClienteProvision.Columns.Add("ESFD_NROBL", typeof(string));

                CobroCliente cobroClienteProvosionSeleleccionado = new CobroCliente();



                if (parameter.ProvisionSeleccionado != null)
                {
                    cobroClienteProvosionSeleleccionado = parameter.CobrosPendientesCliente.Where(x => x.IdFacturacionTercero == parameter.ProvisionSeleccionado).FirstOrDefault();
                }
                else
                {
                    cobroClienteProvosionSeleleccionado = parameter.CobrosPendientesCliente[0];
                }

                foreach (CobroPendietenEmbarque row in cobroClienteProvosionSeleleccionado.CobrosPendientesEmbarque)
                {
                    DataRow drog = dtClienteProvision.NewRow();
                    drog["ESFD_ID"] = 0;
                    drog["ESFD_IDSOLICITUDFACTURACION"] = 0;
                    drog["ESFD_CODIGO_CONCEPTO"] = row.ConceptoCodigo;
                    drog["ESFD_MONEDA"] = row.Moneda;
                    drog["ESFD_IMPORTE"] = row.Importe;
                    drog["ESFD_IGV"] = row.Igv;
                    drog["ESFD_TOTAL"] = row.Total;
                    drog["ESFD_DESCRIPCION_CONCEPTO"] = row.ConceptoCodigoDescripcion;
                    drog["ESFD_DESCRIPCION_PROVISION"] = row.Descripcion;
                    drog["ESFD_IDPROVISION"] = row.ID;
                    drog["ESFD_RUBROCODIGO"] = row.RubroCodigo;
                    drog["ESFD_KEYBL"] = parameter.KEYBLD;
                    drog["ESFD_NROBL"] = parameter.NroBl;
                    dtClienteProvision.Rows.Add(drog);

                }

                using (var cnn = new SqlConnection(strConn))
                {
                    string spName = "TM_PDWAC_SP_SOLICITUD_FACTURACION_CREAR";

                    var queryParameters = new DynamicParameters();

                    queryParameters.Add("@SOFA_KEYBL", parameter.KEYBLD, DbType.String);
                    queryParameters.Add("@SOFA_NROBL", parameter.NroBl, DbType.String);
                    queryParameters.Add("@SOFA_IDUSUARIO_SOLICITA", parameter.IdUsuarioSolicita, DbType.Int32);
                    queryParameters.Add("@SOFA_IDENTIDAD_SOLICITA", parameter.IdEntidadSolicita, DbType.Int32);
                    queryParameters.Add("@SOFA_IDUSUARIO_CREA", parameter.IdUsuarioCrea, DbType.Int32);
                    queryParameters.Add("@SOFA_IDUSUARIO_MODIFICA", parameter.IdUsuarioModifica, DbType.Int32);
                    queryParameters.Add("@SOFA_ACEPTO_FORMULARIO", parameter.AceptoFormulario, DbType.Boolean);
                    queryParameters.Add("@SOFA_APLICA_CREDITO", parameter.AplicaCredito, DbType.Boolean);
                    queryParameters.Add("@SOFA_BANCO_TRANSFERENCIA", parameter.BancoTransferencia, DbType.String);
                    queryParameters.Add("@SOFA_CREDITO_CODIGO", parameter.CodigoCredito, DbType.String);
                    queryParameters.Add("@SOFA_OPERACION_TRANSFERENCIA_CODIGO", parameter.CodigoOperacionTransferencia, DbType.String);
                    queryParameters.Add("@SOFA_METODO_PAGO", parameter.MetodoPago, DbType.String);
                    queryParameters.Add("@SOFA_TIPO_PAGO", parameter.TipoPago, DbType.String);
                    queryParameters.Add("@SOFA_IDCLIENTE", cobroClienteProvosionSeleleccionado.IdCliente, DbType.String);
                    queryParameters.Add("@SOFA_CLIENTE_NRODOCUMENTO", cobroClienteProvosionSeleleccionado.NroDocumentoCliente, DbType.String);
                    queryParameters.Add("@SOFA_CLIENTE_RAZONSOCIAL", cobroClienteProvosionSeleleccionado.RazonSocialCliente, DbType.String);
                    queryParameters.Add("@SOFA_CLIENTE_TIPODOCUMENTO", cobroClienteProvosionSeleleccionado.TipoDocumentoCliente, DbType.String);
                    queryParameters.Add("@SOFA_IMPORTE_TRANSFERENCIA", parameter.ImporteTransferencia, DbType.String);
                    queryParameters.Add("@SOFA_FECHA_TRANSFERENCIA", parameter.FechaTransferencia, DbType.String);
                    queryParameters.Add("@SOFA_MONTO_TOTAL", cobroClienteProvosionSeleleccionado.MontoTotal, DbType.Decimal);
                    queryParameters.Add("@SOFA_CREDITO_DESCRIPCION", parameter.CreditoDescripcion, DbType.String);
                    queryParameters.Add("@SOFA_CODTIPO_ENTIDAD", parameter.CodigoTipoEntidad, DbType.String);
                    queryParameters.Add("@SOFA_EMPRESA_GTRM_CODIGO", parameter.CodigoEmpresaGtrm, DbType.String);
                    queryParameters.Add("@ListaProvosionCliente", dtClienteProvision, DbType.Object);

                    result = cnn.Query<SolicitarFacturacionResult>(spName, queryParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

          

            return result;

        }

    }
}
