using ServiceReference1;
using Servicio.Embarque.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.ServiceExterno
{
    public class ServicioEmbarques
    {
        public async Task<int> ActualizarDocumento(string nombreArchivo, string correo)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarDocumentoRequest request = new TM_WS_ActualizarDocumentoRequest(nombreArchivo, correo);
            TM_WS_ActualizarDocumentoResponse response = client.TM_WS_ActualizarDocumento(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarDocumentoResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }

        public async Task<EmbarqueResult> ObtenerEmbarque(string pKeybld)
        {
            EmbarqueResult embarque = new EmbarqueResult();

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerInformacionEmbarqueRequest request = new TM_WS_ObtenerInformacionEmbarqueRequest(pKeybld);

            TM_WS_ObtenerInformacionEmbarqueResponse response = client.TM_WS_ObtenerInformacionEmbarque(request);

            await client.CloseAsync();

            if (!response.TM_WS_ObtenerInformacionEmbarqueResult.Nodes[1].IsEmpty)
            {
                var item = response.TM_WS_ObtenerInformacionEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();

                embarque.KEYBLD = item.Element("KEYBLD").Value;
                embarque.NROOT = item.Element("NROOT").Value;
                embarque.NROBL = item.Element("NROBL").Value;
                embarque.NRORO = item.Element("NRORO").Value;
                embarque.EMPRESA = item.Element("EMPRESA").Value;
                embarque.ORIGEN = item.Element("ORIGEN").Value;
                embarque.CONDICION = item.Element("CONDICION").Value;
                embarque.DES_CONDICION = item.Element("DES_CONDICION").Value;
                embarque.POL = item.Element("POL").Value;
                embarque.POD = item.Element("POD").Value;
                embarque.ETAPOD = item.Element("ETAPOD").Value;
                embarque.EQUIPAMIENTO = item.Element("EQUIPAMIENTO").Value;
                embarque.MANIFIESTO = item.Element("MANIFIESTO").Value;
                embarque.COD_LINEA = item.Element("COD_LINEA").Value;
                embarque.DES_LINEA = item.Element("DES_LINEA").Value;
                embarque.COD_TIPO_CONSIGNATARIO = item.Element("COD_TIPO_CONSIGNATARIO").Value;
                embarque.CONSIGNATARIO = item.Element("CONSIGNATARIO").Value;
                embarque.COD_INSTRUCCION = item.Element("COD_INSTRUCCION").Value;
                embarque.DES_INSTRUCCION = item.Element("DES_INSTRUCCION").Value;
                embarque.FLAG_LINEA_MEMO = item.Element("FLAG_LINEA_MEMO").Value;
                embarque.FLAG_MEMO_VIGENTE = item.Element("FLAG_MEMO_VIGENTE").Value;
                embarque.FLAG_COBROS_FACTURADOS = item.Element("FLAG_COBROS_FACTURADOS").Value;
                embarque.FLAG_PLAZO_ETA = item.Element("FLAG_PLAZO_ETA").Value;
                embarque.FLAG_CARGA_PELIGROSA = item.Element("FLAG_CARGA_PELIGROSA").Value;
                embarque.FLAG_LOI = item.Element("FLAG_LOI").Value;
                embarque.VENCIMIENTO_PLAZO = item.Element("VENCIMIENTO_PLAZO").Value;
                embarque.FLAG_DIRECCIONAMIENTO_PERMANENTE = item.Element("FLAG_DIRECCIONAMIENTO_PERMANENTE").Value;
                embarque.CODIGO_TAF_DEPOSITO_PERMANENTE = item.Element("CODIGO_TAF_DEPOSITO_PERMANENTE").Value;
                embarque.RAZON_SOCIAL_DEPOSITO_PERMANENTE = item.Element("RAZON_SOCIAL_DEPOSITO_PERMANENTE").Value;
                embarque.ALMACEN = item.Element("ALMACEN").Value;
                embarque.CANTIDAD_CNT = item.Element("CANTIDAD_CNT").Value;
                embarque.NAVEVIAJE = item.Element("NAVEVIAJE").Value;

                embarque.OPERADOR_MAIL = item.Element("OPERADOR_MAIL").Value;
                embarque.FLAG_DESGLOSE = item.Element("BL_DESGLOSA").Value;
                embarque.CANTIDAD_DESGLOSE = item.Element("CANT_DESGLOSES").Value;

                embarque.FLAG_CONFIRMACION_AA = item.Element("FLAG_CONFIRMACION_AA").Value;
                embarque.FLAG_PLAZO_TERMINO_DESCARGA = item.Element("FLAG_PLAZO_TERMINO_DESCARGA").Value;
                embarque.FLAG_EXONERADO_COBRO_GARANTIA = item.Element("FLAG_EXONERADO_COBRO_GARANTIA").Value;
                embarque.PLAZO_TERMINO_DESCARGA = item.Element("PLAZO_TERMINO_DESCARGA").Value;
                embarque.TIPO_PADRE = item.Element("TIPO_PADRE") == null ? "" : item.Element("TIPO_PADRE").Value;
                embarque.FLAG_DIRECCIONAMIENTO_PERMANENTE_BL = item.Element("FLAG_DIRECCIONAMIENTO_PERMANENTE_BL") == null ? "" : item.Element("FLAG_DIRECCIONAMIENTO_PERMANENTE_BL").Value;
                embarque.FINANZAS_MAIL = item.Element("FINANZAS_MAIL") == null ? "" : item.Element("FINANZAS_MAIL").Value;
                
                embarque.FLAG_ESTADO_FACTURACION_SOLICITUD = item.Element("FLAG_ESTADO_FACTURACION_SOLICITUD") == null ? "" : item.Element("FLAG_ESTADO_FACTURACION_SOLICITUD").Value;
                embarque.FLAG_COBROS_PENDIENTES = item.Element("FLAG_COBROS_PENDIENTES") == null ? "" : item.Element("FLAG_COBROS_PENDIENTES").Value;

            }

            return embarque;
        }
        public async Task<int> RegistrarSolicitudDireccionamiento(string pIdSolicitudPW, string pNroSolicitudPW, string pEmpresa, string pKeybld, string pModalidad, string pDeposito, string pEstado, string pUsuarioPW)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_RegistrarSolicitudDireccionamientoRequest request = new TM_WS_RegistrarSolicitudDireccionamientoRequest(pIdSolicitudPW, pNroSolicitudPW, pEmpresa, pKeybld, pModalidad, pDeposito, pEstado, pUsuarioPW);

            TM_WS_RegistrarSolicitudDireccionamientoResponse response = client.TM_WS_RegistrarSolicitudDireccionamiento(request);

            await client.CloseAsync();

            var res = response.TM_WS_RegistrarSolicitudDireccionamientoResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }
        public async Task<int> ActualizarEstadoSolicitudDireccionamiento(string pIdSolicitudPW, string pUsuarioPW, string pEstado, string pMotivo)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarEstadoSolicitudDireccionamientoRequest request = new TM_WS_ActualizarEstadoSolicitudDireccionamientoRequest(pIdSolicitudPW, pUsuarioPW, pEstado, pMotivo);

            TM_WS_ActualizarEstadoSolicitudDireccionamientoResponse response = client.TM_WS_ActualizarEstadoSolicitudDireccionamiento(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarEstadoSolicitudDireccionamientoResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }
        public async Task<int> ActualizarBlTrabajado  (string pKeybld, string pNomArchivo)
        {
            int result = 0;

            // TM_WS_ActualizarBLTrabajadoEnviado

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarBLTrabajadoEnviadoRequest request = new TM_WS_ActualizarBLTrabajadoEnviadoRequest(pKeybld, pNomArchivo) ;

            TM_WS_ActualizarBLTrabajadoEnviadoResponse response = client.TM_WS_ActualizarBLTrabajadoEnviado(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarBLTrabajadoEnviadoResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }

        public async Task<int> ActualizatMemoEnviadoEmbarque(string pKeybld, string pNomArchivo)
        {
            int result = 0;

            // TM_WS_ActualizarBLTrabajadoEnviado

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarMemoEnviadoEmbarqueRequest request = new TM_WS_ActualizarMemoEnviadoEmbarqueRequest(pKeybld, pNomArchivo);

            TM_WS_ActualizarMemoEnviadoEmbarqueResponse response = client.TM_WS_ActualizarMemoEnviadoEmbarque(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarMemoEnviadoEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }
        

    }
}
