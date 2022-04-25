using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Servicio.Message.BussinesLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Message;

namespace Servicio.Message.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMapper _mapper;
        private readonly MessageCorreoManager _messageCorreoManager;
    
        public MessageController(ILogger<MessageController> logger, IMapper mapper, MessageCorreoManager messageCorreoManager)
        {
            _logger = logger;
                _mapper = mapper;
            _messageCorreoManager = messageCorreoManager;
        }

        [HttpPost]
        [Route("enviar-message-correo")]
        public ActionResult<EnviarMessageCorreoResultVM> ObtenerDocumentoPorTipoEntidad(EnviarMessageCorreoParameterVM parameter)
        {
            EnviarMessageCorreoResultVM result = new EnviarMessageCorreoResultVM();
            try { 
         
            _messageCorreoManager.EnviarMensajeCorreo(parameter.RequestMessage,"Correo");

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }

            return result;
        }
    }
}
