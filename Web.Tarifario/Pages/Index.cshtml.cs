using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Tarifario;
using Web.Tarifario.ServiceConsumer;
using Web.Tarifario.Utilitario;

namespace Web.Tarifario.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ILogger<IndexModel> _logger;
        private readonly IMapper _mapper;

        private IConfiguration _configuration;

        [BindProperty(SupportsGet = true)] 
        public InputModel Input { get; set; }


        [TempData]
        public string MensajeError { get; set; }

        public ActionResponse ActionResponse { get; set; }


        public string ReturnUrl { get; set; }


        public IndexModel(ILogger<IndexModel> logger,
                            ServicioMaestro serviicoMaestro,
                            IMapper mapper,
                    
                            IConfiguration configuration)
        {

            _logger = logger;
            _serviceMaestro = serviicoMaestro;
            _mapper = mapper;
    
            _configuration = configuration;

        }

        public async Task<IActionResult> OnGet( )
        {

           
                ListarTarifarioParameterVM listar = new ListarTarifarioParameterVM();
                listar.FechaFin = Input.FechaFin;
                listar.FechaInicio = Input.FechaInicio;
                listar.Rubro = Input.Rubro;
                listar.Servicio = Input.Servicio;

               
                    var result = await _serviceMaestro.FiltrarTarifario(listar);
                    this.Input.TarifarioResult = result;
           





            return Page();
           
        }
    }

 
    public class InputModel
    {

        [Display(Name = "Fecha Inicio")]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        public DateTime? FechaFin { get; set; }


        public string Rubro { get; set; }
        public string Servicio { get; set; }

        public ListarTarifarioResultVM TarifarioResult { get; set; }

        

    }



}
