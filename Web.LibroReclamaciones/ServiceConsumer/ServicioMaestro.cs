using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Common;
using Service.Common.Utils;
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


namespace Web.LibroReclamaciones.ServiceConsumer
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

    
        public async Task<ViewModel.Reclamo.ListaEmpresasResultVM> ListarEmpresas(ViewModel.Reclamo.ListaEmpresasParameterVM parameter)
        {
            const string SERVICIO = "reclamo-empresas";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ViewModel.Reclamo.ListaEmpresasResultVM>();
            return resultado;
        }

        public async Task<ViewModel.Reclamo.ListaUnidadNegocioXEmpresasResultVM> ListarUnidadNegocioXEmpresa(ViewModel.Reclamo.ListaUnidadNegocioXEmpresaParameterVM parameter)
        {
            const string SERVICIO = "reclamo-unidadnegocio-xempresa";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ViewModel.Reclamo.ListaUnidadNegocioXEmpresasResultVM>();
            return resultado;
        }
        

    }
}
