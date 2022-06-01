using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.Util;
using ViewModel.Datos.Embarque.AsignarAgente;
using Web.Principal.Areas.GestionarEmbarques.Models;
using Web.Principal.ServiceConsumer;
using AutoMapper;
using Web.Principal.ServiceExterno;
using static Utilitario.Constante.EmbarqueConstante;
using Web.Principal.Areas.GestionarSolicitudes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Utilitario.Constante;
using Service.Common.Logging.Application;
using Microsoft.Extensions.Logging;
using ViewModel.Datos.Entidad;
using Web.Principal.Areas.GestionarEmbarques.Models.Entidad;

namespace Web.Principal.Areas.GestionarEmbarques.Controllers
{
    [Area("GestionarEmbarques")]
    public class EntidadController : BaseController
    {
        private readonly ServicioEmbarque _serviceEmbarque;
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioEmbarques _serviceEmbarques;
        private readonly ServicioUsuario _serviceUsuario;

        private readonly IMapper _mapper;
        private static ILogger _logger = ApplicationLogging.CreateLogger("EntidadController");

        public EntidadController(ServicioEmbarque serviceEmbarque,
            ServicioEmbarques serviceEmbarques,
            ServicioUsuario serviceUsuario,
            ServicioMaestro serviceMaestro,
        IMapper mapper)
        {
            _serviceEmbarque = serviceEmbarque;
            _serviceEmbarques = serviceEmbarques;
            _serviceUsuario = serviceUsuario;
            _serviceMaestro = serviceMaestro;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

      
        [HttpGet]
        public async Task<IActionResult> ListarEntidadTipo()
        {
            EntidadTipoModel model = new EntidadTipoModel();

            try
            {


                ListarEntidadParameterVM listarEntidadParameterVM = new ListarEntidadParameterVM();

                var result = await _serviceEmbarque.ListarEntidadTipo(listarEntidadParameterVM);




                model.EntidadesTipo = result;




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ListarEntidadTipo");
                model.Resultado = ViewModel.Common.Request.DataRequestViewModelResponse.ResultadoServicio.Error;
                model.Message = "Error en obtener lista de entidades tipo";
                model.StatusResponse = "-100";
            }

            return PartialView("_ResultListarEntidadTipo", model);
        }
    }
}
