using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ClienteController(IUsuarioRepository repository, IMapper mapper, ServiceConsumer.ServicioMessage servicioMessage)
        {
            _repository = repository;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
        }

        [HttpPost]
        [Route("listar-clientes")]
        public ActionResult<ListarClientesResultVM> ObtenerUsuarioSecundario(ListarClienteParameterVM parameter)
        {
            var result = _repository.ListarClientes(_mapper.Map<ListarClienteParameter>(parameter));
            return _mapper.Map<ListarClientesResultVM>(result);
        }


        [HttpGet]
        [Route("leer-cliente")]
        public ActionResult<LeerClienteResultVM> ObtenerUsuarioSecundario(Int64 id)
        {
            var result = _repository.LeerCliente(id);
            return _mapper.Map<LeerClienteResultVM>(result);
        }


    }
}