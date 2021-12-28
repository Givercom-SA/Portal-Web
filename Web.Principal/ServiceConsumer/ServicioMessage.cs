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
using ViewModel.Datos.Message;
using ViewModel.Datos.Parametros;
using Web.Principal.Utils;

namespace Web.Principal.ServiceConsumer
{
    public class ServicioMessage : IServiceConsumer
    {
        private readonly string URL_BASE;
        private const string SERVICIO_MESSAGE = "Message/";

        static HttpClient client = new HttpClient();

        public ServicioMessage(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Message"]}";
        }

        public async Task<EnviarMessageCorreoResultVM> EnviarMensageCorreo(EnviarMessageCorreoParameterVM parameter)
        {
            const string SERVICIO = "enviar-message-correo";
      

            var json = JsonConvert.SerializeObject(parameter);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
       
            var uri = $"{URL_BASE}{SERVICIO_MESSAGE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<EnviarMessageCorreoResultVM>();

            return resultado;

 
        }

    }
}
