using BusinessLogic.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MSMQ.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Message;

namespace Servicio.Message.BussinesLogic
{

  
    public class MessageCorreoManager : IBusinessLogic
    {
        private readonly ILogger<MessageCorreoManager> _logger;
        private IConfiguration _configuration;
        private string RutaCola { get; set; }
        MessageQueue messageQueue = null;

        public MessageCorreoManager(ILogger<MessageCorreoManager> logger, IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;

            RutaCola = _configuration["Cola:Correo"];
        }

        private void setMensaje(String _etiquetaCola )
        {
            messageQueue = new MessageQueue(this.RutaCola);
            messageQueue.Label = _etiquetaCola;
        }

        public void EnviarMensajeCorreo(RequestMessage mensaje, String _etiquetaCola)
        {
            _logger.LogInformation(" INICIO ENVIO COLA");

            setMensaje(_etiquetaCola);

            if (!MessageQueue.Exists(this.RutaCola))
            {
                MessageQueue.Create(this.RutaCola);
            }

            messageQueue.Send(mensaje);

            _logger.LogInformation(" Correo => " + mensaje.Correo);
            _logger.LogInformation(" Asunto => " + mensaje.Asunto);

            _logger.LogInformation(" FIN ENVIO COLA");
        }
    }
}
