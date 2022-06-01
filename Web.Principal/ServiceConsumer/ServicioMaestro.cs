using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using ViewModel.Datos.Entidad;
using ViewModel.Datos.ListaCorreos;
using ViewModel.Datos.Parametros;
using Web.Principal.Util;

namespace Web.Principal.ServiceConsumer
{
    public class ServicioMaestro : IServiceConsumer
    {
        private readonly string URL_BASE;
        private const string SERVICIO_ACCESOS = "Parametros/";

        static HttpClient client = new HttpClient();

        public ServicioMaestro(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Maestros"]}";
        }

        public async Task<ListaParametrosVM> ObtenerParametroPorIdPadre(int idParam)
        {
            const string SERVICIO = "obtenerParametrosIdPadre";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}?idParam={idParam}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListaParametrosVM>();

            return resultado;
        }

        public async Task<ListarDocumentoTipoEntidadVM> ObtenerDocumentoPorTipoEntidad(ListDocumentoTipoEntidadParameterVM parameter)
        {
            const string SERVICIO = "obtenerDocumentoPorTipoentidad";
      

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

    
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);

            var resultado = response.ContentAsType<ListarDocumentoTipoEntidadVM>();

            return resultado;



        }

        public async Task<ListaCorreosVW> ObtenerCorreosPorPerfil(int _perfil)
        {
            const string SERVICIO = "ObtenerCorreosPorPerfil";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}?idParam={_perfil}";
            var response = await client.GetAsync(uri);

            var resultado = response.ContentAsType<ListaCorreosVW>();

            return resultado;
        }

        //public async Task<ListaTipoEntidadVM> ObtenerTipoEntidad()
        //{
        //    const string SERVICIO = "obtenerTipoEntidad";
        //    var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
        //    var response = await client.GetAsync(uri);

        //    var resultado = response.ContentAsType<ListaTipoEntidadVM>();

        //    return resultado;
        //}
        //public async Task<ListaTipoDocumentoVM> ObtenerTipoDocumento()
        //{
        //    const string SERVICIO = "obtenerTipoDocumento";
        //    var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
        //    var response = await client.GetAsync(uri);

        //    return response.ContentAsType<ListaTipoDocumentoVM>();
        //}
    }
}
