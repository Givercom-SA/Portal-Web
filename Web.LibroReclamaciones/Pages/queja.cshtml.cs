using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TransMares.Core;
using ViewModel.Datos.Message;
using ViewModel.Reclamo;
using Web.LibroReclamaciones.Model;
using Web.LibroReclamaciones.ServiceConsumer;
using Web.LibroReclamaciones.Utilitario;
using Web.Principal.ServiceConsumer;

namespace Web.Principal.Pages.libroreclamaciones
{
    public class quejaModel : PageModel
    {

        private readonly ServicioMaestro _serviceMaestro;
        private readonly ILogger<quejaModel> _logger;
        private readonly IMapper _mapper;
        private readonly ServicioMessage _servicioMessage;
        private IConfiguration _configuration;

        [BindProperty]
        public InputModel Input { get; set; }


        [TempData]
        public string MensajeError { get; set; }

        public ActionResponse ActionResponse { get; set; }


        public string ReturnUrl { get; set; }


        public quejaModel(ILogger<quejaModel> logger,
                            ServicioMaestro serviicoMaestro,
                            IMapper mapper,
                            ServicioMessage servicioMessage,
                            IConfiguration configuration)
        {

            _logger = logger;
            _serviceMaestro = serviicoMaestro;
            _mapper = mapper;
            _servicioMessage = servicioMessage;
            _configuration = configuration;

        }

        public async Task OnGet()
        {
            Input = new InputModel();

            var parameterEmpresas = new ViewModel.Reclamo.ListaEmpresasParameterVM();

            var listTipoDocumnentoResult = await _serviceMaestro.ListarEmpresas(parameterEmpresas);

            if (listTipoDocumnentoResult.CodigoResultado == 0)
            {

                Input.ListEmpresaAtendio = new SelectList(listTipoDocumnentoResult.Empresas, "CodigoEmpresa", "NombreEmpresa");
            }

            else
                MensajeError = MensajeError + " " + listTipoDocumnentoResult.MensajeResultado;

        }



        public async Task<IActionResult> OnPost()
        {


            ActionResponse = new ActionResponse();
            ActionResponse.Codigo = -1;
            ActionResponse.Mensaje = "Estimado cliente, ocurrio un error interno por favor volver a intentar mas tarde.";

            if (ModelState.IsValid)
            {


                RegistrarReclamoParameterVM registrarReclamoParameterVM = new RegistrarReclamoParameterVM();
                registrarReclamoParameterVM.Celular = Input.Celular;
                registrarReclamoParameterVM.CodigoEmpresa = Input.EmpresaAtiende;
                registrarReclamoParameterVM.CodigoTipoDocumento = Input.Celular;
                registrarReclamoParameterVM.CodigoTipoFormulario = "02";
                registrarReclamoParameterVM.CodigoUnidadNegocio = Input.UnidadNegocio;
                registrarReclamoParameterVM.Email = Input.Celular;
                registrarReclamoParameterVM.FechaIncidencia = Input.FechaIncidencia;
                registrarReclamoParameterVM.Nombre = Input.NombreCompleto;
                registrarReclamoParameterVM.Observacion = Input.Mensaje;
                registrarReclamoParameterVM.RazonSocial = Input.RazonSocial;
                registrarReclamoParameterVM.Ruc = Input.Ruc;

                var resultRegistrarReclamo = await _serviceMaestro.RegistrarReclamo(registrarReclamoParameterVM);
                if (resultRegistrarReclamo.CodigoResultado == 0)
                {
                    ActionResponse.Codigo = resultRegistrarReclamo.CodigoResultado;
                    ActionResponse.Mensaje = resultRegistrarReclamo.MensajeResultado;

                    enviarCorreoCliente(Input.Email,Input.NombreCompleto,$"Se ha registrado tu queja exitosamente y en 24 horas te estaremos respondiendo.");

                    

                }
                else {
                    ActionResponse.Codigo = -3;
                    ActionResponse.Mensaje = "Estimado cliente, ocurrio un error inesperado por favor volver a intentar.";
                }
               
            }
            else
            {
                ActionResponse.Codigo = -2;
                ActionResponse.Mensaje = "Estimado cliente, los datos ingresados es inv�lido.";
            }

            return new JsonResult(ActionResponse);

        }

        private async void enviarCorreoCliente(string correo, string nombreCliente, string Mensaje)
        {
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido =
                new FormatoCorreoBody().formatoBodyReclamacion(nombreCliente, Mensaje,
                _configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupoUrl.ToString()]);

            enviarMessageCorreoParameterVM.RequestMessage.Correo = correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"Transmares Group - Registro de Queja";
            var ressult = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
        }


    }
}
