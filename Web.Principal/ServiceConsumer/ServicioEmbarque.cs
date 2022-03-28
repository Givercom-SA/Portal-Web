using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.Utils;
using ViewModel.Datos.Embarque.AsignarAgente;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ViewModel.Datos.RegistrarNotificacionArribo;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;
using ViewModel.Datos.Embarque.SolicitudDireccionamiento;
using ViewModel.Datos.ListaNotificacionesArribo;
using ViewModel.Datos.Embarque.GestionarMemo;
using ViewModel.Datos.Embarque.CobrosPagar;
using ViewModel.Datos.ListaExpressRelease;
using ViewModel.Datos.Embarque.SolicitudFacturacion;
using Service.Common;
using ViewModel.Datos.Entidad;

namespace Web.Principal.ServiceConsumer
{
    public class ServicioEmbarque : IServiceConsumer
    {
        private readonly string URL_BASE;
        private const string SERVICIO_EMBARQUE = "Embarque/";
        private const string SERVICIO_DIRECCIONAMIENTO = "Direccionamiento/";
        private const string SERVICIO_FACTURACION = "Facturacion/";
        private const string SERVICIO_ENTIDAD = "Entidad/";

        static HttpClient client = new HttpClient();

        public ServicioEmbarque(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Embarque"]}";
        }




        #region Entidad
        public async Task<ListarEntidadResultVM> ListarEntidadTipo(ListarEntidadParameterVM parameter)
        {
            const string SERVICIO = "entidad-listar-tipo";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_ENTIDAD}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarEntidadResultVM>();

            return resultado;

        }
        #endregion

        public async Task<ListarUsuarioEntidadResultVM> ListarAgentes(int IdPerfil, int IdUsuarioExcluir)
        {
            const string SERVICIO = "listar-agentes";

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}?IdPerfil={IdPerfil}&IdUsuarioExcluir={IdUsuarioExcluir}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarUsuarioEntidadResultVM>();

            return resultado;

        }

        public async Task<ListarCobrosPagarPadreBeyBlResultVM> ObtenerCobroPagarPadreKeyBL(string keybl)
        {
            const string SERVICIO = "listar-cobros-pagar-get_padre-keybl";

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}?KeyBl={keybl}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarCobrosPagarPadreBeyBlResultVM>();

            return resultado;

        }

        public async Task<ListarSolicitudFacturacionBandejaResultVM> ListarSolicitudFacturacionBandeja(ListarSolicitudFacturacionBandejaParameterVM parameter)
        {
            const string SERVICIO = "solicitud-facturacion-bandeja";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_FACTURACION}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarSolicitudFacturacionBandejaResultVM>();

            return resultado;

        }
        public async Task<LeerSolicitudFacturacionBandejaResultVM> LeerSolicitudFacturacionBandeja(LeerSolicitudFacturacionBandejaParameterVM parameter)
        {
            const string SERVICIO = "solicitud-facturacion-bandeja-leer";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_FACTURACION}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<LeerSolicitudFacturacionBandejaResultVM>();

            return resultado;

        }
        public async Task<SolicitarFacturacionEstadoResultVM> RegistrarSolicitudFacturacionEstado(SolicitarFacturacionEstadoParameterVM parameter)
        {
            const string SERVICIO = "solicitud-facturacion-estado";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_FACTURACION}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<SolicitarFacturacionEstadoResultVM>();

            return resultado;

        }

        public async Task<AsignarAgenteResultVM> AsignarAgenteCrear(AsignarAgenteCrearParameterVM parameter)
        {
            const string SERVICIO = "asignar-agente-crear";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<AsignarAgenteResultVM>();

            return resultado;

        }

        public async Task<SolicitarFacturacionResultVM> SolicitarFacturacionRegistrar(SolicitarFacturacionParameterVM parameter)
        {
            const string SERVICIO = "solicitud-facturacion-registrar";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"{URL_BASE}{SERVICIO_FACTURACION}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<SolicitarFacturacionResultVM>();

            return resultado;

        }

        public async Task<AsignarAgenteResultVM> AsignarAgenteCambiarEstado(AsignarAgenteEstadoParameterVM parameter)
        {
            const string SERVICIO = "asignar-agente-cambiar-estado";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<AsignarAgenteResultVM>();

            return resultado;
        }

        public async Task<ListarAsignarAgenteResultVM> ListarAsignacion(AsignarAgenteListarParameterVM parameter)
        {
            const string SERVICIO = "listar-asignacion";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarAsignarAgenteResultVM>();

            return resultado;

        }
        public async Task<ListarFacturacionTerceroResultVM> ListarFacturacionTerceros(ListarFacturacionTerceroParameterVM parameter)
        {
            const string SERVICIO = "listar-facturacion-terceros";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarFacturacionTerceroResultVM>();

            return resultado;

        }

        public async Task<ListarFacturacionTerceroDetalleResultVM> ListarFacturacionTercerosDetalle(int IdFacturacionTercero)
        {
            const string SERVICIO = "listar-facturacion-terceros-detalle";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}?IdFacturacionTercero={IdFacturacionTercero}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarFacturacionTerceroDetalleResultVM>();

            return resultado;

        }

        public async Task<ListarFacturacionTerceroDetallePorKeyblResultVM> ListarFacturacionTercerosDetallePorKeybl(string keybl)
        {
            const string SERVICIO = "listar-facturacion-terceros-detalle-keybl";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}?keybl={keybl}";
            var response = await client.GetAsync(uri);
            var resultado = response.ContentAsType<ListarFacturacionTerceroDetallePorKeyblResultVM>();
            return resultado;
        }

        public async Task<ListarAsignarAgenteResultVM> ListarAsignados(AsignarAgenteListarParameterVM parameter)
        {
            const string SERVICIO = "listar-asignados";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarAsignarAgenteResultVM>();

            return resultado;

        }


        public async Task<RegistrarFacturacionTerceroResultVM> RegistrarFacturacionTercero(RegistrarFacturacionTerceroParameterVM parameter)
        {
            const string SERVICIO = "registrar-facturacion-tercero";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<RegistrarFacturacionTerceroResultVM>();

            return resultado;

        }

        public async Task<RegistrarFacturacionTerceroResultVM> ActualizarFacturacionTercero(RegistrarFacturacionTerceroParameterVM parameter)
        {
            const string SERVICIO = "actualizar-facturacion-tercero";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<RegistrarFacturacionTerceroResultVM>();

            return resultado;

        }

        public async Task<string> ProgramarNotificaciones(RegistrarNotificacionArriboVM parameter)
        {
            const string SERVICIO = "registrar-notificacion-arribo";

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            return response.ContentAsString();
        }

        public async Task<ListarNotificacionesPendientesVW> ListarNotificacionesArriboPendientes()
        {
            const string SERVICIO = "listar-notificacion-arribo-pendientes";

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarNotificacionesPendientesVW>();

            return resultado;

        }

        public async Task<SolicitudDireccionamientoResultVM> SolicitudDireccionamientoCrear(SolicitudDireccionamientoParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "solicitud-direccionamiento-crear";
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<SolicitudDireccionamientoResultVM>();

            return resultado;
        }
        public async Task<SolicitudDireccionamientoResultVM> ValidarSolicitudDireccionamiento(string KeyBL)
        {
            const string SERVICIO = "validar-solicitud-direccionamiento";
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}/{KeyBL}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<SolicitudDireccionamientoResultVM>();

            return resultado;
        }
        public async Task<ListarSolicitudesResultVM> ObtenerSolicitudes(string nroSolicitud, string RucDni, string codEstado)
        {
            const string SERVICIO = "obtener-solicitudes";
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}/{nroSolicitud}/{RucDni}/{codEstado}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarSolicitudesResultVM>();

            return resultado;
        }

        public async Task<SolicitudResultVM> ObtenerSolicitudPorCodigo(string codSol)
        {
            const string SERVICIO = "obtener-solicitud-porcodigo";
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}?codSol={codSol}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<SolicitudResultVM>();

            return resultado;
        }

        public async Task<AsignarAgenteHistorialResultVM> AsignarAgenteHistorial(string idAsginacionAgente)
        {
            const string SERVICIO = "asignar-agente-historial";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}/{idAsginacionAgente}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<AsignarAgenteHistorialResultVM>();

            return resultado;
        }



        public async Task<string> ActualizarEstadoDocumento(string codSolicitud, string codDocumento, string codEstado, string codEstadoRechazo, int userId)
        {
            const string SERVICIO = "actualizar-estado-documento";
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}/{codSolicitud}/{codDocumento}/{codEstado}/{codEstadoRechazo}/{userId}";
            var response = await client.PutAsync(uri, null);

            return string.Format("");
        }

        public async Task<EvaluarDireccionamientoVM> ProcesarSolicitud(ProcesarDireccionamientoParameterVM parameter)
        {

            const string SERVICIO = "procesar-solicitud";

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            return response.ContentAsType<EvaluarDireccionamientoVM>();




        }

        public async Task<ListarEventosResultVM> ObtenerEventosSolicitudes(string codSolicitud)
        {
            const string SERVICIO = "obtener-eventos-solicitud";
            var uri = $"{URL_BASE}{SERVICIO_DIRECCIONAMIENTO}{SERVICIO}/{codSolicitud}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarEventosResultVM>();

            return resultado;
        }

        public async Task<NotificacionesMemoResultVM> CrearNotificacionMemo(NotificacionMemoParameterVM parameter)
        {
            const string SERVICIO = "crear-notificacion-memo";

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            return response.ContentAsType<NotificacionesMemoResultVM>();
        }

        public async Task<NotificacionesMemoResultVM> VerificarNotificacionMemo(NotificacionMemoParameterVM parameter)
        {
            const string SERVICIO = "verificar-notificacion-memo";

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            return response.ContentAsType<NotificacionesMemoResultVM>();
        }

        public async Task<ProcesarSolicitudMemoResultVM> CrearSolicitudMemo(SolicitudMemoParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "crear-solicitud-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ProcesarSolicitudMemoResultVM>();

            return resultado;
        }
        public async Task<ListarSolicitudesMemoResultVM> ObtenerSolicitudesMemo(ListarSolicitudesMemoParameterVM parameter)
        {
           
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-solicitudes-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarSolicitudesMemoResultVM>();

            return resultado;

        }

        public async Task<SolicitudMemoResultVM> ObtenerSolicitudMemo(string codSol)
        {
            const string SERVICIO = "obtener-solicitud-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}?codSol={codSol}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<SolicitudMemoResultVM>();

            return resultado;
        }

        public async Task<SolicitudMemoDocumentoEstadoResultVM> ActualizarEstadoDocumentoMemo(SolicitudMemoDocumentoEstadoParameterVM parameterVM)
        {
        
       

            var json = JsonConvert.SerializeObject(parameterVM);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "actualizar-estado-documento-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<SolicitudMemoDocumentoEstadoResultVM>();

            return resultado;

        }

        public async Task<SolicitudMemoEstadoresultVM> ProcesarSolicitudMemo(SolicitudMemoEstadoParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "procesar-solicitud-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<SolicitudMemoEstadoresultVM>();

            return resultado;

        }

        public async Task<ListarEventosMemoResultVM> ObtenerEventosSolicitudeMemo(string codSolicitud)
        {
            const string SERVICIO = "obtener-eventos-solicitud-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}/{codSolicitud}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarEventosMemoResultVM>();

            return resultado;
        }

        public async Task<ListarCobrosPagarResultVM> ObtenerCobrosPagar(string KeyBLD, string BL, string BLNieto, string ConceptoCodigo)
        {
            const string SERVICIO = "obtener-cobros-pagar";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}?KeyBLD={KeyBLD}&BL={BL}&BLNieto={BLNieto}&ConceptoCodigo={ConceptoCodigo}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarCobrosPagarResultVM>();

            return resultado;
        }

        public async Task<CobrosPagarResultVM> CrearCobrosPagar(CobrosPagarParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "crear-cobros-pagar";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<CobrosPagarResultVM>();

            return resultado;
        }

        public async Task<CobrosPagarResultVM> ActualizarCobrosPagar(CobrosPagarParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "actualizar-cobros-pagar";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<CobrosPagarResultVM>();

            return resultado;
        }



        public async Task<VerificarAsignacionAgenteAduanasResultVM> VerificarAsignacionAgenteAduanas(VerificarAsignacionAgenteAduanasParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "verificar-asignacion-agente-aduanas";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<VerificarAsignacionAgenteAduanasResultVM>();

            return resultado;
        }


        public async Task<ListarProvisionFacturacionTerceroResultVM> ObtenerProvicionFacturacionTercero(ListarProvisionFacturacionTerceroParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            const string SERVICIO = "solicitud-facturacion-provision-fact-tercero";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarProvisionFacturacionTerceroResultVM>();
            return resultado;
        }

        public async Task<ListaExpressReleaseAceptadasVW> ListarExpressReleaseAceptadas()
        {
            const string SERVICIO = "listar-express-release-aceptadas";

            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListaExpressReleaseAceptadasVW>();

            return resultado;
        }

        public async Task<string> RegistrarExpressRelease(ExpressReleaseParameterVM parameter)
        {
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "registrar-express-release-aceptadas";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            return response.ContentAsString();
        }

        public async Task<string> EnviarCorreoMemo(MemoCorreoParameterVM parameter)
        {


            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "enviar-correo-memo";
            var uri = $"{URL_BASE}{SERVICIO_EMBARQUE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            return response.ContentAsString();

        }

        public async Task<LeerSolicitudFacturacionKeyBlResultVM> ListarSolicitudFacturacionPorKeyBl(string KeyBLD)
        {
            const string SERVICIO = "solicitud-facturacion-listar-keybl";
            var uri = $"{URL_BASE}{SERVICIO_FACTURACION}{SERVICIO}?keyBl={KeyBLD}";
            var response = await client.GetAsync(uri);
            var resultado = response.ContentAsType<LeerSolicitudFacturacionKeyBlResultVM>();
            return resultado;
        }

    }
}