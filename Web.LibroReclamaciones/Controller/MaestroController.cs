using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ViewModel.Reclamo;
using Web.LibroReclamaciones.ServiceConsumer;
using AutoMapper;

namespace Web.LibroReclamaciones.Controller
{


    [AllowAnonymous]
    public class MaestroController : Microsoft.AspNetCore.Mvc.Controller
    {


        
        private readonly ServicioMaestro _serviceMaestro;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public MaestroController(
            ServicioMaestro servicioMaestro,
            IConfiguration configuration,

            IMapper mapper)
        {
            _serviceMaestro = servicioMaestro;
            _mapper = mapper;
            _configuration = configuration;
        }


        [HttpPost("UnidadNegocioXEmpresa", Name = "maestro_unidadnegocio_x_empresa")]
        [AllowAnonymous]
        public async Task<JsonResult> UnidadNegocioXEmpresa(ListaUnidadNegocioXEmpresaParameterVM request)
        {
            ListaUnidadNegocioXEmpresasResultVM resultVM = new ListaUnidadNegocioXEmpresasResultVM();

            resultVM = await _serviceMaestro.ListarUnidadNegocioXEmpresa(request);



            return new JsonResult(resultVM);



        }


    }
}
