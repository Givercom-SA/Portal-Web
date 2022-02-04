using Microsoft.Extensions.Logging;
using Service.Common;
using Service.Common.Logging.Application;
using ServiceEmbarque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.Model;

namespace Web.Principal.ServiceExterno
{
    public class ServicioEmbarques : IServiceConsumer
    {
        private static ILogger logger = ApplicationLogging.CreateLogger("ServicioEmbarques");

        public async Task<ListaCobrosModel> ObtenerCobros(string val)
        {
            ListaCobrosModel listaResult = new ListaCobrosModel();

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerCobrosEmbarqueRequest request = new TM_WS_ObtenerCobrosEmbarqueRequest(pKeybld: val);

            TM_WS_ObtenerCobrosEmbarqueResponse response = client.TM_WS_ObtenerCobrosEmbarque(request);

            await client.CloseAsync();

            listaResult.listaCobros = new List<EmbarqueCobrosModel>();

            if (!response.TM_WS_ObtenerCobrosEmbarqueResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ObtenerCobrosEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {
                    listaResult.listaCobros.Add(
                        new EmbarqueCobrosModel
                        {
                            RUBRO_C_CODIGO = item.Element("RUBRO_C_CODIGO").Value,
                            CONCEP_C_CODIGO = item.Element("CONCEP_C_CODIGO").Value,
                            DESCRIPCION = item.Element("DESCRIPCION").Value,
                            CONCEP_C_DESCRIPCION_COBRO = item.Element("CONCEP_C_DESCRIPCION_COBRO").Value,
                            MONEDA = item.Element("MONEDA").Value,
                            IMPORTE = decimal.Parse(item.Element("IMPORTE").Value),
                            IGV = decimal.Parse(item.Element("IGV").Value),
                            TOTAL = decimal.Parse(item.Element("TOTAL").Value)
                        });
                }
            }

            return listaResult;
        }

        public async Task<ListaTrackingModel> ObtenerTracking(string val)
        {
            ListaTrackingModel listaResult = new ListaTrackingModel();

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerTrackingEmbarqueRequest request = new TM_WS_ObtenerTrackingEmbarqueRequest(pKeybld: val);

            TM_WS_ObtenerTrackingEmbarqueResponse response = client.TM_WS_ObtenerTrackingEmbarque(request);

            await client.CloseAsync();

            listaResult.Tracking = new EmbarqueTrackingModel();

            if (!response.TM_WS_ObtenerTrackingEmbarqueResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ObtenerTrackingEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {

                    listaResult.Tracking = new EmbarqueTrackingModel()
                    {
                        ETAPOD = item.Element("ETAPOD").Value,
                        ETDPOL = item.Element("ETDPOL").Value,
                        APROBACION_FLETE = item.Element("APROBACION_FLETE").Value,
                        DIRECCIONAMIENTOS = item.Element("DIRECCIONAMIENTOS").Value,
                        DESGLOSES = item.Element("DESGLOSES").Value,
                        ACEPTACION_DIRECCIONAMIENTO = item.Element("ACEPTACION_DIRECCIONAMIENTO").Value,
                        FECENDOSE = string.IsNullOrEmpty(item.Element("FECENDOSE").Value) ? null : DateTime.Parse(item.Element("FECENDOSE").Value),
                        FECVENSOBRESTADIA = string.IsNullOrEmpty(item.Element("FECVENSOBRESTADIA").Value) ? null : DateTime.Parse(item.Element("FECVENSOBRESTADIA").Value),
                        FECRECMEMO = string.IsNullOrEmpty(item.Element("FECRECMEMO").Value) ? null : DateTime.Parse(item.Element("FECRECMEMO").Value),
                        FECINGRESO = string.IsNullOrEmpty(item.Element("FECENDOSE").Value) ? null : DateTime.Parse(item.Element("FECENDOSE").Value),
                        DIASLIBRESALMACENAJE = int.Parse(item.Element("DIASLIBRESALMACENAJE").Value),
                        FECSALIDA = string.IsNullOrEmpty(item.Element("FECSALIDA").Value) ? null : DateTime.Parse(item.Element("FECSALIDA").Value),
                        NROINVENTARIO = item.Element("NROINVENTARIO").Value
                    };
                }
            }

            return listaResult;
        }


        public async Task<List<CreditoCobroModel>> ObtenerCreditoCobro(string pKeybld, string pTipoCliente, string pRucCliente, string pFormaCredito, short pAccion)
        {
            List<CreditoCobroModel> listaResult = new List<CreditoCobroModel>();

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerCreditoCobroEmbarqueRequest request = new TM_WS_ObtenerCreditoCobroEmbarqueRequest(pKeybld, pTipoCliente, pRucCliente, pFormaCredito, pAccion);

            TM_WS_ObtenerCreditoCobroEmbarqueResponse response = client.TM_WS_ObtenerCreditoCobroEmbarque(request);

            await client.CloseAsync();

            

            if (!response.TM_WS_ObtenerCreditoCobroEmbarqueResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ObtenerCreditoCobroEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {
                    CreditoCobroModel creditoCobro = new CreditoCobroModel();

                  
                    creditoCobro = new CreditoCobroModel()
                    {
                        COD_FORMA_CREDITO = item.Element("COD_FORMA_CREDITO").Value,
                        DES_FORMA_CREDITO = item.Element("DES_FORMA_CREDITO").Value,

                    };

                    listaResult.Add(creditoCobro);
                }
            }

            return listaResult;
        }

        public async Task<int> AplicaCredito(string pKeybld, string pTipoCliente, string pRucCliente, string pFormaCredito, short pAccion)
        {
            int lresult;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerCreditoCobroEmbarqueRequest request = new TM_WS_ObtenerCreditoCobroEmbarqueRequest(pKeybld, pTipoCliente, pRucCliente, pFormaCredito, pAccion);

            TM_WS_ObtenerCreditoCobroEmbarqueResponse response = client.TM_WS_ObtenerCreditoCobroEmbarque(request);

            await client.CloseAsync();

            var res = response.TM_WS_ObtenerCreditoCobroEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            lresult = string.IsNullOrEmpty(res.Element("FLAG_APLICA_CREDITO").Value) ? 0 : int.Parse(res.Element("FLAG_APLICA_CREDITO").Value);


            return lresult;
        }


        public async Task<DisponibilidadCreditoModel> DisponibilidadCredito(string pKeybld, string pTipoCliente, string pRucCliente, string pFormaCredito, short pAccion)
        {
            DisponibilidadCreditoModel result = new DisponibilidadCreditoModel();
            result.Resultado = -200;
            try
            {

                WebService_TM_PWSoapClient client =
           new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

                TM_WS_ObtenerCreditoCobroEmbarqueRequest request = new TM_WS_ObtenerCreditoCobroEmbarqueRequest(pKeybld, pTipoCliente, pRucCliente, pFormaCredito, pAccion);

                TM_WS_ObtenerCreditoCobroEmbarqueResponse response = client.TM_WS_ObtenerCreditoCobroEmbarque(request);

                await client.CloseAsync();

                var res = response.TM_WS_ObtenerCreditoCobroEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
                result.Resultado = string.IsNullOrEmpty(res.Element("FLAG_DISPONIBILIDAD_CREDITO").Value) ? 0 : int.Parse(res.Element("FLAG_DISPONIBILIDAD_CREDITO").Value);
                if (result.Resultado == 0)
                {
                    result.Motivo = string.IsNullOrEmpty(res.Element("MENSAJE_DISPONIBILIDAD").Value) ? "" : res.Element("MENSAJE_DISPONIBILIDAD").Value;
                }

            }
            catch (Exception err)
            {
                logger.LogError(err, "Disponibilidad de credito");
                result.Motivo = "";
                result.Resultado = -100;
            }

       
            return result;
        }

        public async Task<int> AplicaCreditoVirtal(string pKeybld, string pTipoCliente, string pRucCliente, string pFormaCredito, short pAccion)
        {
            int lresult=0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerCreditoCobroEmbarqueRequest request = new TM_WS_ObtenerCreditoCobroEmbarqueRequest(pKeybld, pTipoCliente, pRucCliente, pFormaCredito, pAccion);

            TM_WS_ObtenerCreditoCobroEmbarqueResponse response = client.TM_WS_ObtenerCreditoCobroEmbarque(request);

            await client.CloseAsync();

            if (pAccion == 0)
            {
                var res = response.TM_WS_ObtenerCreditoCobroEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
                lresult = string.IsNullOrEmpty(res.Element("FLAG_APLICA_CREDITO").Value) ? 0 : int.Parse(res.Element("FLAG_APLICA_CREDITO").Value);
            }
            else if (pAccion == 2) {
                var res = response.TM_WS_ObtenerCreditoCobroEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
                lresult = string.IsNullOrEmpty(res.Element("FLAG_ATENCION_ONLINE").Value) ? 0 : int.Parse(res.Element("FLAG_ATENCION_ONLINE").Value);
            }


            return lresult;
        }


        public async Task<int> ActualizarProvicionCobranza(string pKeybld, long pIdProvision)
        {
            int lresult;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarProvisionesCobroEmbarqueRequest request = new TM_WS_ActualizarProvisionesCobroEmbarqueRequest(pKeybld, pIdProvision );

            TM_WS_ActualizarProvisionesCobroEmbarqueResponse response = client.TM_WS_ActualizarProvisionesCobroEmbarque(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarProvisionesCobroEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            lresult = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);


            return lresult;
        }


        public async Task<ListaEmbarqueModel> ListarEmbarques(string _pEmpresa, short _pAnio, string _pServicio, string _pOrigen, short _pTipoFiltro, string _pFiltro, string pTipoEntidad, string _pRucEntidad)
        {
            ListaEmbarqueModel listaResult = new ListaEmbarqueModel();

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ListarEmbarquesRequest request = new TM_WS_ListarEmbarquesRequest(
                pEmpresa: _pEmpresa,
                pAnio: _pAnio,
                pTipoFiltro: _pTipoFiltro,
                pFiltro: _pFiltro,
                pTipoEntidad: pTipoEntidad,
                pRuc_Entidad: _pRucEntidad);

            TM_WS_ListarEmbarquesResponse response = await client.TM_WS_ListarEmbarquesAsync(request);

            await client.CloseAsync();

            listaResult.listaEmbarques = new List<EmbarqueModel>();

            if (!response.TM_WS_ListarEmbarquesResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ListarEmbarquesResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {
                    listaResult.listaEmbarques.Add(
                        new EmbarqueModel
                        {
                            KEYBLD = item.Element("KEYBLD").Value,
                            NROOT = item.Element("NROOT").Value,
                            NROBL = item.Element("NROBL").Value,
                            NRORO = item.Element("NRORO").Value,
                            EMPRESA = item.Element("EMPRESA").Value,
                            ORIGEN = item.Element("ORIGEN").Value,
                            CONDICION = item.Element("CONDICION").Value,
                            POL = item.Element("POL").Value,
                            POD = item.Element("POD").Value,
                            ETAPOD = item.Element("ETAPOD").Value,
                            EQUIPAMIENTO = item.Element("EQUIPAMIENTO").Value,
                            MANIFIESTO = item.Element("MANIFIESTO").Value,
                            COD_LINEA = item.Element("COD_LINEA").Value,
                            DES_LINEA = item.Element("DES_LINEA").Value,
                            CONSIGNATARIO = item.Element("CONSIGNATARIO").Value,
                            COD_INSTRUCCION = item.Element("COD_INSTRUCCION").Value,
                            DES_INSTRUCCION = item.Element("DES_INSTRUCCION").Value
                        });
                }
            }

            return listaResult;
        }

        public async Task<EmbarqueModel> ObtenerEmbarque(string pKeybld)
        {
            EmbarqueModel embarque = new EmbarqueModel();

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

                embarque.FEC_CREATE_RO = item.Element("FEC_CREATE_RO") == null ? "" : item.Element("FEC_CREATE_RO").Value;
                embarque.FEC_ETD = item.Element("FEC_ETD") == null ? "" : item.Element("FEC_ETD").Value;
                embarque.FEC_ETA = item.Element("FEC_ETA") == null ? "" : item.Element("FEC_ETA").Value;
                embarque.FEC_TRANSMISION_ADUANAS = item.Element("FEC_TRANSMISION_ADUANAS") == null ? "" : item.Element("FEC_TRANSMISION_ADUANAS").Value;
                embarque.FEC_ING_CARGA = item.Element("FEC_ING_CARGA") == null ? "" : item.Element("FEC_ING_CARGA").Value;
                embarque.FEC_RET_CARGA = item.Element("FEC_RET_CARGA") == null ? "" : item.Element("FEC_RET_CARGA").Value;


            }


            return embarque;
        }

        public async Task<int> RegistrarDocumentoTipo(string _codEmpresa, string pKeybld, string _nroBl, short _anio, string _correoUsu, string _tipodoc)
        {
            int result = 0;

            try
            {
                WebService_TM_PWSoapClient client =
                    new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

                TM_WS_RegistrarDocumentoRequest request = new TM_WS_RegistrarDocumentoRequest(_codEmpresa, pKeybld, _nroBl, _anio, _correoUsu, _tipodoc);

                TM_WS_RegistrarDocumentoResponse response = client.TM_WS_RegistrarDocumento(request);

                await client.CloseAsync();

                var res = response.TM_WS_RegistrarDocumentoResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
                result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);
            }
            catch(Exception ex)
            {
                logger.LogError(ex,"Registro de DocumentoTipo");
                result = -1;
            }

            return result;
        }

        public async Task<RegistroSolicitudResponseModel> RegistrarSolicitudFacturacion(RegistroSolicitudRequestModel request)
        {
            RegistroSolicitudResponseModel registroSolicitudResponseModel = new RegistroSolicitudResponseModel();

            try
            {
                WebService_TM_PWSoapClient client =
                    new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);
                        TM_WS_RegistrarSolicitudFacturacionRequest pRequest = new TM_WS_RegistrarSolicitudFacturacionRequest(
                    request.pIdSolicitudPW,
                    request.pNroSolicitudPW,
                    request.pFechaSolicitud,
                    request.pFechaEvaluacion,
                    request.pUsuarioPW,
                    request.pEmpresa, 
                    request.pKeybld, 
                    request.pTipoCliente, 
                    request.pRucCliente, 
                    request.pTipoPago, 
                    request.pFormaPago, 
                    request.pId_Transaccion,
                    request.pNumeroOperacion,
                    request.pFechaTransferencia,
                    request.pImporte,
                    request.pUsuarioFinPW);
        
                TM_WS_RegistrarSolicitudFacturacionResponse response = client.TM_WS_RegistrarSolicitudFacturacion(pRequest);
                await client.CloseAsync();
                var res = response.TM_WS_RegistrarSolicitudFacturacionResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
                registroSolicitudResponseModel.Respuesta = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);
                registroSolicitudResponseModel.IdSolicitudTAF = string.IsNullOrEmpty(res.Element("ID_SOLICITUD_TAF").Value) ? "": res.Element("ID_SOLICITUD_TAF").Value.ToString();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Registro de Solicitud de Facturacion");
                registroSolicitudResponseModel.Respuesta = -1;
            }

            return registroSolicitudResponseModel;
        }


        public async Task<RegistroSolicitudFacturacionDetalleResponseModel> RegistrarSolicitudFacturacionDetalle(RegistroSolicitudFacturacionDetalleRequestModel request)
        {
            RegistroSolicitudFacturacionDetalleResponseModel result = new RegistroSolicitudFacturacionDetalleResponseModel();

            try
            {
                WebService_TM_PWSoapClient client =
                    new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);
                TM_WS_RegistrarSolicitudFacturacionDetalleRequest pRequest = new TM_WS_RegistrarSolicitudFacturacionDetalleRequest(
                request.pIdSolicitud,
             request.pRubroCCodigo,
             request.pConcepCCodigo,
             request.pMoneda,
                request.pImporte);

                TM_WS_RegistrarSolicitudFacturacionDetalleResponse response = client.TM_WS_RegistrarSolicitudFacturacionDetalle(pRequest);
                await client.CloseAsync();
                var res = response.TM_WS_RegistrarSolicitudFacturacionDetalleResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
                result.Respuesta = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Registro de Solicitud de Facturacion Detalle");
                result.Respuesta = -1;
            }

            return result;
        }

        public async Task<List<CobrosPendienteEmbarqueVM>> ObtenerCobrosPendienteEmbarque(string keyBL, string opcion)
        {


            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerCobrosPendientesEmbarqueRequest request = new TM_WS_ObtenerCobrosPendientesEmbarqueRequest(keyBL, short.Parse(opcion));

            TM_WS_ObtenerCobrosPendientesEmbarqueResponse response = await client.TM_WS_ObtenerCobrosPendientesEmbarqueAsync(request);

            await client.CloseAsync();

            List<CobrosPendienteEmbarqueVM> cobrosPendienteEmbarques = new List<CobrosPendienteEmbarqueVM>();

            if (!response.TM_WS_ObtenerCobrosPendientesEmbarqueResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ObtenerCobrosPendientesEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {
                    if (opcion == "0")
                    {
                        cobrosPendienteEmbarques.Add(
                        new CobrosPendienteEmbarqueVM
                        {
                            ID = item.Element("ID").Value,
                            NroBl = item.Element("NROBL").Value,
                            keyBl = item.Element("KEYBLD").Value,
                            BlPagar = item.Element("BL_PAGAR").Value,
                            Desglose = item.Element("BL_PAGAR").Value,
                            IdBlPagar = item.Element("ID_BL_PAGAR").Value,
                            RubroCodigo = item.Element("RUBRO_C_CODIGO").Value,
                            ConceptoCodigo = item.Element("CONCEP_C_CODIGO").Value,
                            Descripcion = item.Element("DESCRIPCION").Value,
                            ConceptoCodigoDescripcion = item.Element("CONCEPTO").Value,
                            Moneda = item.Element("MONEDA").Value,
                            Importe = item.Element("IMPORTE").Value,
                            Igv = item.Element("IGV").Value,
                            Total = item.Element("TOTAL").Value,
                            FlagAsignacion = item.Element("FLAG_ASIGNACION").Value
                        });
                    }
                    else
                    {
                        cobrosPendienteEmbarques.Add(
                            new CobrosPendienteEmbarqueVM
                            {
                                ID = item.Element("ID").Value,
                                NroBl = "",
                                keyBl = "",
                                BlPagar = "",
                                Desglose = "",
                                IdBlPagar = "",
                                RubroCodigo = item.Element("RUBRO_C_CODIGO").Value,
                                ConceptoCodigo = item.Element("CONCEP_C_CODIGO").Value,
                                Descripcion = item.Element("DESCRIPCION").Value,
                                ConceptoCodigoDescripcion = item.Element("CONCEPTO").Value,
                                Moneda = item.Element("MONEDA").Value,
                                Importe = item.Element("IMPORTE").Value,
                                Igv = item.Element("IGV").Value,
                                Total = item.Element("TOTAL").Value,
                                FlagAsignacion = item.Element("FLAG_ASIGNACION").Value
                            });
                    }

                }
            }

            return cobrosPendienteEmbarques;
        }

        public async Task<List<ClienteFacturarTerceroVM>> ObtenerEntidades(string tipoDocumento, string numeroDocumento, string razonSocial, string opcion, string tipoEntidad)
        {
            List<ClienteFacturarTerceroVM> listaResult = new List<ClienteFacturarTerceroVM>();

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerEntidadesRequest request = new TM_WS_ObtenerEntidadesRequest(tipoDocumento, numeroDocumento, razonSocial, short.Parse(opcion), short.Parse(tipoEntidad));

            TM_WS_ObtenerEntidadesResponse response = await client.TM_WS_ObtenerEntidadesAsync(request);

            await client.CloseAsync();

            listaResult = new List<ClienteFacturarTerceroVM>();

            if (!response.TM_WS_ObtenerEntidadesResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ObtenerEntidadesResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {

                    try {

                        listaResult.Add(
                     new ClienteFacturarTerceroVM
                     {
                         CodigoClienteFacturarTercero = item.Element("CODIGO_TAF")== null ?"": item.Element("CODIGO_TAF").Value,
                         NroDocumentoClienteFacturarTercero = item.Element("RUC") == null ? "" : item.Element("RUC").Value,
                         NombresClienteFacturarTercero = item.Element("RAZON_SOCIAL") == null ? "" : item.Element("RAZON_SOCIAL").Value,
                         CodigoAlmacen = item.Element("CODIGO_ALMACEN") == null ? "" : item.Element("CODIGO_ALMACEN").Value
                     });

                    } catch (Exception err)
                    {
                        logger.LogError(err, "Obtener Entidad");

                    }





                }
            }

            return listaResult;
        }

        public async Task<ListaDesgloseModel> ObtenerDesglosesEmbarque(string _pKeybld, short _opcion, string tipoEntidad ,int proceso_negocio)
        {
            string intRespuesta = "";
            ListaDesgloseModel listaResult = new ListaDesgloseModel();
            intRespuesta = await ObtenerDesglose(_pKeybld, _opcion, 0);

            if (proceso_negocio == EmbarqueConstante.ProcesoSistema.LIBERACION_CARGA)
            {
                if (tipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER))
                {
                    if (intRespuesta == "0")
                    {

                        listaResult.listaDesglose = new List<DesgloseModel>();

                        return listaResult;
                    }
                }

            }
            else
            {


                if (intRespuesta == "0")
                {
             
                    listaResult.listaDesglose = new List<DesgloseModel>();

                    return listaResult;
                }

            }

            WebService_TM_PWSoapClient client =
                    new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

                TM_WS_ObtenerDesglosesEmbarqueRequest request = new TM_WS_ObtenerDesglosesEmbarqueRequest(_pKeybld, _opcion, 1);

                TM_WS_ObtenerDesglosesEmbarqueResponse response = client.TM_WS_ObtenerDesglosesEmbarque(request);

                await client.CloseAsync();

                listaResult.listaDesglose = new List<DesgloseModel>();

                if (!response.TM_WS_ObtenerDesglosesEmbarqueResult.Nodes[1].IsEmpty)
                {
                    foreach (var item in response.TM_WS_ObtenerDesglosesEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                    {
                        listaResult.listaDesglose.Add(
                            new DesgloseModel
                            {
                                KEYBLD = item.Element("KEYBLD").Value,
                                NROBL = item.Element("NROBL").Value,
                                CONSIGNATARIO = item.Element("CONSIGNATARIO").Value,
                                FLAG_AUTORIZADO = item.Element("FLAG_AUTORIZADO")==null?0: int.Parse(item.Element("FLAG_AUTORIZADO").Value)

                            });
                    }
                }
           

            return listaResult;
        }

        public async Task<string> ObtenerDesglose(string _pKeybld, short _opcion, short _accion)
        {
            string intRespuesta = "";

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ObtenerDesglosesEmbarqueRequest request = new TM_WS_ObtenerDesglosesEmbarqueRequest(_pKeybld, _opcion, _accion);

            TM_WS_ObtenerDesglosesEmbarqueResponse response = client.TM_WS_ObtenerDesglosesEmbarque(request);

            await client.CloseAsync();

            if (!response.TM_WS_ObtenerDesglosesEmbarqueResult.Nodes[1].IsEmpty)
            {
                foreach (var item in response.TM_WS_ObtenerDesglosesEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table"))
                {
                    intRespuesta = item.Element("RESPUESTA").Value;
                }
            }

            return intRespuesta;
        }

        public async Task<int> ActualizarAgenteAduanas(string pKeybld, string tipoOpcion, string agenteAduanaNroDoc, string agenteAduanaNombres)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarAgenteAduanasEmbarqueRequest request = new TM_WS_ActualizarAgenteAduanasEmbarqueRequest(pKeybld, short.Parse(tipoOpcion), agenteAduanaNroDoc, agenteAduanaNombres);

            TM_WS_ActualizarAgenteAduanasEmbarqueResponse response = client.TM_WS_ActualizarAgenteAduanasEmbarque(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarAgenteAduanasEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }

        public async Task<int> ActualizarPagosTercero(string pKeybld, long pId, string codigoTercero, string rucTercero,string razonSocialTercero)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarPagosTerceroRequest request = new TM_WS_ActualizarPagosTerceroRequest(pKeybld, pId, codigoTercero, rucTercero, razonSocialTercero);

            TM_WS_ActualizarPagosTerceroResponse response = client.TM_WS_ActualizarPagosTercero(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarPagosTerceroResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }

        public async Task<int> ActualizarAutorizacionEmbarque(string pKeybld)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_ActualizarAutorizacionEmbarqueRequest request = new TM_WS_ActualizarAutorizacionEmbarqueRequest(pKeybld);

            TM_WS_ActualizarAutorizacionEmbarqueResponse response = client.TM_WS_ActualizarAutorizacionEmbarque(request);

            await client.CloseAsync();

            var res = response.TM_WS_ActualizarAutorizacionEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }

        public async Task<int> RegistrarDireccionamientoPermanente(string pEmpresa, string pKeybld, string pDeposito, string pUsuarioPW)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);

            TM_WS_RegistrarDireccionamientoPermanenteRequest request = new TM_WS_RegistrarDireccionamientoPermanenteRequest(pEmpresa, pKeybld, pDeposito, pUsuarioPW);

            TM_WS_RegistrarDireccionamientoPermanenteResponse response = client.TM_WS_RegistrarDireccionamientoPermanente(request);

            await client.CloseAsync();

            var res = response.TM_WS_RegistrarDireccionamientoPermanenteResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }

        public async Task<int> ActualizarMemoEnviadoEmbarque(string pKeybld, string pNombreArchivo)
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);
            TM_WS_ActualizarMemoEnviadoEmbarqueRequest request = new TM_WS_ActualizarMemoEnviadoEmbarqueRequest(pKeybld, pNombreArchivo);
            TM_WS_ActualizarMemoEnviadoEmbarqueResponse response = client.TM_WS_ActualizarMemoEnviadoEmbarque(request);
            await client.CloseAsync();
            var res = response.TM_WS_ActualizarMemoEnviadoEmbarqueResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);
            return result;
        }

        public async Task<int> ValidarRegistroEntidad(string tipo, string _ruc , string _correo )
        {
            int result = 0;

            WebService_TM_PWSoapClient client =
                new WebService_TM_PWSoapClient(WebService_TM_PWSoapClient.EndpointConfiguration.WebService_TM_PWSoap);
            TM_WS_ValidarRegistroEntidadRequest request = new TM_WS_ValidarRegistroEntidadRequest(tipo, _ruc, _correo);
            TM_WS_ValidarRegistroEntidadResponse response = client.TM_WS_ValidarRegistroEntidad(request);

            await client.CloseAsync();
            var res = response.TM_WS_ValidarRegistroEntidadResult.Nodes[1].Element("NewDataSet").Elements("Table").FirstOrDefault();
            result = string.IsNullOrEmpty(res.Element("RESPUESTA").Value) ? 0 : int.Parse(res.Element("RESPUESTA").Value);

            return result;
        }
    }

}
