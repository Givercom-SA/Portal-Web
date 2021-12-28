using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Servicio.Embarque.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;

namespace Servicio.Embarque.ServiceConsumer
{
    public class ServicioUsuario
    {
        private readonly string URL_BASE;
        private const string SERVICIO_ACCESOS = "Usuario/";

        static HttpClient client = new HttpClient();

        public ServicioUsuario(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Usuario"]}";
        }

        public async Task<LeerUsuarioResultVM> ObtenerUsuarioPorId(int idUsuario)
        {
            //var json = JsonConvert.SerializeObject(parameter);
            //var data = new StringContent(json, Encoding.UTF8, "application/json");

            const string SERVICIO = "obtener-usuario";
            var uri = $"{URL_BASE}{SERVICIO_ACCESOS}{SERVICIO}/{idUsuario}";
            var response = await client.GetAsync(uri);
            
            var resultado = response.ContentAsType<LeerUsuarioResultVM>();

            return resultado;
        }
    }
}
