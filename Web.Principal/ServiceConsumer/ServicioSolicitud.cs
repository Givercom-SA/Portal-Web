using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.ListarEventos;
using ViewModel.Datos.ListarSolicitudes;
using ViewModel.Datos.Solicitud;
using ViewModel.Datos.SolictudAcceso;
using Web.Principal.Utils;

namespace Web.Principal.ServiceConsumer
{
    public class ServicioSolicitud : IServiceConsumer
    {
        private readonly string URL_BASE;
        private const string SERVICIO_ACCESOS = "Solicitud/";

        static HttpClient client = new HttpClient();

        public ServicioSolicitud(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Solicitud"]}";
        }

        public async Task<ListarSolicitudesVW> ObtenerSolicitudes(ListarSolicitudesParameterVM parameter)
        {
            const string SERVICIO = "obtenerSolicitudes";
        
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarSolicitudesVW>();

            return resultado;
        }

        public async Task<SolicitudVM> obtenerSolicitudPorCodigo(string codSol)
        {
            const string SERVICIO = "obtenerSolicitudPorCodigo";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}?codSol={codSol}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<SolicitudVM>();

            return resultado;
        }

        public async Task<SolicitudVM> leerSolicitud(Int64 id)
        {
            const string SERVICIO = "leer-solicitud";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}?id={id}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<SolicitudVM>();

            return resultado;
        }
        public async Task<string> ActualizarEstadoDocumento(string codSolicitud, string codDocumento, string codEstado, string codEstadoRechazo, int userId)
        {
            const string SERVICIO = "actualizarEstadoDocumento"; 
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{codSolicitud}/{codDocumento}/{codEstado}/{codEstadoRechazo}/{userId}";
            var response = await client.PutAsync(uri, null);

            return string.Format("");
        }

        public async Task<string> ProcesarSolicitud(string codSolicitud)
        {
            const string SERVICIO = "procesarSolicitud";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{codSolicitud}";
            var response = await client.PutAsync(uri, null);

            return string.Format("");
        }

        public async Task<SolicitudAccesoAprobarResultVM> AprobarSolicitud(SolicitudAccesoAprobarParameterVM parameter)
        {
            const string SERVICIO = "aprobarSolicitud";

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<SolicitudAccesoAprobarResultVM>();

            return resultado;
        }

        public async Task<SolicitudAccesoAprobarResultVM> RechazarSolicitud(SolicitudAccesoAprobarParameterVM parameterVM)
        {
            const string SERVICIO = "rechazarSolicitud";
            var json = JsonConvert.SerializeObject(parameterVM);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<SolicitudAccesoAprobarResultVM>();

            return resultado;
        }

        public async Task<ListarEventosVW> ObtenerEventosSolicitudes(string codSolicitud)
        {
            const string SERVICIO = "obtenereventossolicitud";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{codSolicitud}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListarEventosVW>();

            return resultado;
        }
    }
}
