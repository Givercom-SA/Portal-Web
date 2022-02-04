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
using ViewModel.Tarifario;

namespace Web.Tarifario.ServiceConsumer
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

    
      
        public async Task<ListarTarifarioResultVM> FiltrarTarifario(ListarTarifarioParameterVM parameter)
        {
            const string SERVICIO = "tarifario-listar";
            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<ListarTarifarioResultVM>();
            return resultado;
        }
    }
}
