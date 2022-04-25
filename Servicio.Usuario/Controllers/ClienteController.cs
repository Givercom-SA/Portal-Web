using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Servicio.Usuario.Models.Cliente;
using Servicio.Usuario.Models.Usuario;
using Servicio.Usuario.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TransMares.Core;
using ViewModel.Datos.Message;
using ViewModel.Datos.UsuarioRegistro;

namespace Servicio.Usuario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly ServiceConsumer.ServicioMessage _servicioMessage;
        private readonly ILogger<ClienteController> _logger;
        public ClienteController(IUsuarioRepository repository,
            IMapper mapper, ServiceConsumer.ServicioMessage servicioMessage,
            ILogger<ClienteController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
            _logger = logger;
        }

        [HttpPost]
        [Route("listar-clientes")]
        public ActionResult<ListarClientesResultVM> ObtenerUsuarioSecundario(ListarClienteParameterVM parameter)
        {
            ListarClienteResult result =new ListarClienteResult();
            try { 
             result = _repository.ListarClientes(_mapper.Map<ListarClienteParameter>(parameter));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<ListarClientesResultVM>(result);
        }


        [HttpGet]
        [Route("leer-cliente")]
        public ActionResult<LeerClienteResultVM> ObtenerUsuarioSecundario(Int64 id)
        {
            LeerClienteResult result =new LeerClienteResult();
            try { 
             result = _repository.LeerCliente(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, e.Message);
            }
            return _mapper.Map<LeerClienteResultVM>(result);
        }


    }
}