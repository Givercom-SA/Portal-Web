
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Message;
using ViewModel.Datos.Embarque.AsignarAgente;
using System.Net.Http;
using Service.Common.Utils;

namespace Servicio.Usuario.ServiceConsumer
{
    public class ServicioMessage
    {
        private readonly string URL_BASE;
        private const string SERVICIO_MESSAGE = "Message/";

        static HttpClient client = new HttpClient();

        public ServicioMessage(IConfiguration configuration)
        {
            this.URL_BASE = $"{configuration["ConfiguracionServicios:Message"]}";
        }

        public async Task<EnviarMessageCorreoResultVM> EnviarMensageCorreo(EnviarMessageCorreoParameterVM parameterVM)
        {

            const string SERVICIO = "enviar-message-correo";
            var json = JsonConvert.SerializeObject(parameterVM);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = $"{URL_BASE}{SERVICIO_MESSAGE}{SERVICIO}";
            var response = await client.PostAsync(uri, data);
            var resultado = response.ContentAsType<EnviarMessageCorreoResultVM>();

            return resultado;

        }
    }
}
