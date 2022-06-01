using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicio.Notificacion.Subscription.Web
{
    public class WebClient : IWebSubscription
    {
        private List<HubConnection> _hubConnections = new List<HubConnection>();
        private ILogger<WebClient> _logger;

        public WebClient(ILogger<WebClient> logger)
        {
            _logger = logger;
        }

        public void Configure(IEnumerable<string> connectionsString)
        {
            _logger.LogInformation("Configurando conexion con los Hub de AFPnet");

            var i = 1;

            foreach (var connectionString in connectionsString)
            {
                _logger.LogInformation(string.Concat("Conexion ", i, " - ", connectionString));

                conectarAHub(connectionString);

                i++;
            }
        }

        private void conectarAHub(string connectionString)
        {

            var hubConnection = new HubConnectionBuilder().WithUrl(connectionString).Build();

            //Reconexion
            hubConnection.Closed += async (error) =>
            {
                _logger.LogError(error, string.Concat("Se cerro la conexion por el siguiente motivo : ", error.Message));
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            _hubConnections.Add(hubConnection);
        }

        public void enviarNotificacion(Model.Notificacion notificacion)
        {
            try
            {
                var i = 1;

                foreach (var hub in _hubConnections)
                {
                    _logger.LogInformation(string.Concat("Enviando Notificacion a Hub ", i));
                    enviarNotificacionAHub(hub, notificacion);

                    i++;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al enviar la notificacion a Hubs");
            }
        }

        private void enviarNotificacionAHub(HubConnection hubConnection, Model.Notificacion notificacion)
        {

            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                _logger.LogWarning("Conexion al HUB de AFPnet desconectada. Se vuelve a conectar");
                hubConnection.StartAsync().Wait();
            }

            hubConnection.SendAsync("RecibirNotificacionDB", notificacion).Wait();
        }
    }
}
