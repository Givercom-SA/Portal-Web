using System;
using System.Collections.Generic;
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

namespace Web.Principal.Pages.ReclamoQueja
{
    public class ReclamoModel : PageModel
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ILogger<ReclamoModel> _logger;
        private readonly IMapper _mapper;
        private readonly ServicioMessage _servicioMessage;
        private IConfiguration _configuration;

        [BindProperty]
        public InputReclamoModel Input { get; set; }


        [TempData]
        public string MensajeError { get; set; }

        public ActionResponse ActionResponse { get; set; }


        public string ReturnUrl { get; set; }


        public ReclamoModel(ILogger<ReclamoModel> logger,
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
            Input = new InputReclamoModel();

            var parameterEmpresas = new ViewModel.Reclamo.ListaEmpresasParameterVM();
            Input.FechaTope = DateTime.Now.ToString("yyyy-MM-dd");

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
                registrarReclamoParameterVM.CodigoTipoDocumento = Input.TipoDocumento;
                registrarReclamoParameterVM.CodigoTipoFormulario = "01";
                registrarReclamoParameterVM.CodigoUnidadNegocio = Input.UnidadNegocio;
                registrarReclamoParameterVM.Email = Input.Email;
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

                    // enviar cliente
                    string contenidoCliente = "";
                    contenidoCliente = $"Se ha registrado tu reclamo exitosamente y en 24 horas te estaremos respondiendo, a continuación el detalle. <br/><br/>";
                    contenidoCliente = contenidoCliente + $" Ruc: {Input.Ruc}<br/>";
                    contenidoCliente = contenidoCliente + $" Razón Social: {Input.RazonSocial}<br/>";
                    contenidoCliente = contenidoCliente + $" Nombres: {Input.NombreCompleto}<br/>";
                    contenidoCliente = contenidoCliente + $" Email: {Input.Email}<br/>";
                    contenidoCliente = contenidoCliente + $" Fecha Incidencia: {Input.FechaIncidencia}<br/>";
                    contenidoCliente = contenidoCliente + $" Empresa Atendió: {Input.EmpresaAtiendeNombre}<br/>";
                    contenidoCliente = contenidoCliente + $" Unidad de Negocio: {Input.UnidadNegocioNombre}<br/>";

                   await enviarCorreoCliente(Input.Email, Input.NombreCompleto, contenidoCliente);





                    // enviar usuario atiende

                    var listaEstado = await _serviceMaestro.ObtenerParametroPorIdPadre(74);
                    string correo = listaEstado.ListaParametros.ElementAt(0).ValorCodigo;

                    string contenido = "";
                    contenido = $"Se ha registrado un reclamo, a continuación el detalle. <br/><br/>";
                    contenido = contenido + $" Ruc: {Input.Ruc}<br/>";
                    contenido = contenido + $" Razón Social: {Input.RazonSocial}<br/>";
                    contenido = contenido + $" Nombres: {Input.NombreCompleto}<br/>";
                    contenido = contenido + $" Email: {Input.Email}<br/>";
                    contenido = contenido + $" Fecha Incidencia: {Input.FechaIncidencia}<br/>";
                    contenido = contenido + $" Empresa Atendió: {Input.EmpresaAtiendeNombre}<br/>";
                    contenido = contenido + $" Unidad de Negocio: {Input.UnidadNegocioNombre}<br/>";
                    contenido = contenido + $" Tipo Documento: {Input.TipoDocumentoNombre}<br/>";
                    contenido = contenido + $" Mensaje: {Input.Mensaje}<br/>";
                    await enviarCorreoCliente(correo,"usuario", contenido);

                }
                else
                {
                    ActionResponse.Codigo = -3;
                    ActionResponse.Mensaje = "Estimado cliente, ocurrio un error inesperado por favor volver a intentar.";
                }
            }
            else
            {
                ActionResponse.Codigo = -2;
                ActionResponse.Mensaje = "Estimado cliente, los datos ingresados es inválido.";
            }

            return new JsonResult(ActionResponse);

        }
        private async Task<string> enviarCorreoCliente(string correo, string nombreCliente, string Mensaje)
        {
            EnviarMessageCorreoParameterVM enviarMessageCorreoParameterVM = new EnviarMessageCorreoParameterVM();
            enviarMessageCorreoParameterVM.RequestMessage = new RequestMessage();
            enviarMessageCorreoParameterVM.RequestMessage.Contenido =
                new FormatoCorreoBody().formatoBodyReclamacion(nombreCliente, Mensaje,
                _configuration[Utilitario.Constante.ConfiguracionConstante.Imagen.ImagenGrupoUrl.ToString()]);

            enviarMessageCorreoParameterVM.RequestMessage.Correo = correo;
            enviarMessageCorreoParameterVM.RequestMessage.Asunto = $"Transmares Group - Registro de Reclamo";
            var ressult = await _servicioMessage.EnviarMensageCorreo(enviarMessageCorreoParameterVM);
            return "";
        }
    }
}
