using Microsoft.Extensions.Logging;
using Servicio.Notificacion.Subscription.Web;
using System;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace Servicio.Notificacion.Subscription
{
    public class NotificacionSubscription : IDatabaseSubscription
    {
        private SqlTableDependency<Model.Notificacion> _subscription;
        private ILogger<NotificacionSubscription> _logger;
        private WebClient _webClient;

        public NotificacionSubscription(ILogger<NotificacionSubscription> logger, WebClient webClient)
        {
            _logger = logger;
            _webClient = webClient;
            _logger.LogInformation("Iniciando Constructor NotificacionSubscription");
        }

        public void Configure(string connection)
        {
            try
            {
                _subscription = new SqlTableDependency<Model.Notificacion>(connection, null, null, null, null, null, DmlTriggerType.Insert);
                _subscription.OnChanged += OnChanged;
                _subscription.OnError += OnError;
                _subscription.OnStatusChanged += OnStatusChanged;
                _subscription.Start();

            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Sucedio un error al configurar la Suscripcion a la Base de Datos");
                throw;
            }

        }

        private void OnChanged(object sender, RecordChangedEventArgs<Model.Notificacion> e)
        {
            if (e.ChangeType == ChangeType.Insert)
            {
                var notificacion = e.Entity;

                _logger.LogInformation("Se obtuvo nueva notificacion a usuario : " + notificacion.CodigoUsuario);
                _webClient.enviarNotificacion(e.Entity);
            }
        }

        private void OnStatusChanged(object sender, StatusChangedEventArgs eventArgs)
        {
            int i = 0;
            _logger.LogWarning("Ha cambiado el status a " + eventArgs.Status.ToString());

            while (_subscription.Status == TableDependencyStatus.StopDueToError)
            {
                Task.Delay(new Random().Next(0, 5) * 1000).Wait();
                _logger.LogWarning("Reconectando con Service Broker de Base de Datos - Intento " + i);
                _subscription.Start();
                i++;
            }

        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.LogError(e.Error, "Sucedio un error al obtener el mensaje de Base de Datos : " + e.Message);
        }

    }
}
