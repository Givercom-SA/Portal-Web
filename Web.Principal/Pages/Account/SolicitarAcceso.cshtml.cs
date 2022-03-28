using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Utilitario.Constante;
using ViewModel.Datos.Documento;
using ViewModel.Datos.LoginInicial;
using ViewModel.Datos.Parametros;
using ViewModel.Datos.SolictudAcceso;
using Web.Principal.Model;
using Web.Principal.ServiceConsumer;
using Web.Principal.ServiceExterno;
using Web.Principal.Utils;

namespace Web.Principal.Pages.Account
{
    public class SolicitarAccesoModel : PageModel
    {
        private readonly ServicioMaestro _serviceMaestro;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ILogger<SolicitarAccesoModel> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ServicioEmbarques _serviceEmbarqueExterno;
        private IWebHostEnvironment Environment;

   
        public SolicitarAccesoModel(ILogger<SolicitarAccesoModel> logger,
            ServicioMaestro serviicoMaestro,
            ServicioAcceso serviicoAcceso,
            IMapper mapper,
            IWebHostEnvironment environment,
            IWebHostEnvironment _environment,
            ServicioEmbarques _serviceEmbarques)
        {

            _logger = logger;
            _serviceMaestro = serviicoMaestro;
            _mapper = mapper;
            _serviceAcceso = serviicoAcceso;
            hostingEnvironment = environment;
            Environment = _environment;
            _serviceEmbarqueExterno = _serviceEmbarques;


        }


        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string MensajeError { get; set; }

        public ActionResponse ActionResponse { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGet()
        {
            Input = new InputModel();
            Input.ValidarCorreo = new VerificarCodigoValidacionParameterVM();
            Input.ValidarCorreo.CodigoVerificacion = "";

            var listEntidades = await _serviceMaestro.ObtenerParametroPorIdPadre(1);

            if (listEntidades.CodigoResultado == 0)
            {
                var resultTipoentidad = listEntidades.ListaParametros;
                Input.ListTipoEntidad2 = new ListTipoEntidadModel();
                Input.ListTipoEntidad2.TiposEntidad = new List<TipoEntidad>();

                foreach (ParametrosVM item in resultTipoentidad)
                {
                    Input.ListTipoEntidad2.TiposEntidad.Add(new TipoEntidad()
                    {
                        Check = false,
                        CodTipoEntidad = item.ValorCodigo,
                        IdParametro = item.IdParametro,
                        NombreTipoEntidad = item.NombreDescripcion
                    });
                }

            }
            else
                MensajeError = MensajeError + " " + listEntidades.MensajeResultado;

            var listTipoDocumnentoResult = await _serviceMaestro.ObtenerParametroPorIdPadre(38);

            if (listTipoDocumnentoResult.CodigoResultado == 0)
            {
                Input.ListTipoDocumento = new SelectList(listTipoDocumnentoResult.ListaParametros, "ValorCodigo", "NombreDescripcion");
            }
            else
                MensajeError = MensajeError + " " + listTipoDocumnentoResult.MensajeResultado;
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            bool blDocumentosValido = true;
            ActionResponse = new ActionResponse();
            ActionResponse.ListActionListResponse = new List<ActionErrorResponse>();

            ListDocumentoTipoEntidadParameterVM listarDocumentoTipoEntidadVM = new ListDocumentoTipoEntidadParameterVM();
            listarDocumentoTipoEntidadVM.BrindaCargaFCL = Input.seBrindaOperacionesCargaFCL;
            listarDocumentoTipoEntidadVM.AcuerdoSeguridadCadenaSuministro = Input.acuerdoSeguridadCadenaSuministra;
            listarDocumentoTipoEntidadVM.SeBrindaAgenciamientodeAduanas = Input.seBrinaAgenciaAdeuanas;
            listarDocumentoTipoEntidadVM.TiposEntidad = new List<ViewModel.Datos.Entidad.TipoEntidadVM>();

            foreach (var item in Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true))
            {
                listarDocumentoTipoEntidadVM.TiposEntidad.Add(new ViewModel.Datos.Entidad.TipoEntidadVM() { CodTipoEntidad = item.CodTipoEntidad });
            }

            var listDocumentosSeleccionados = await _serviceMaestro.ObtenerDocumentoPorTipoEntidad(listarDocumentoTipoEntidadVM);

            var listVerificar = Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true).ToList();

            if (listVerificar == null || listVerificar.Count() <= 0)
            {
                ActionResponse.ListActionListResponse.Add(new ActionErrorResponse() { Mensaje = "Debe seleccionar al menos un tipo de entidad", NombreCampo = "Input.TipoEntidad2" });
            }

            //if ((Input.ListDocumentoTipoEntidad == null) || (Input.ListDocumentoTipoEntidad.listarDocumentosTipoEntidad == null || Input.ListDocumentoTipoEntidad.listarDocumentosTipoEntidad.Count() <= 0))
            //{
            //    blDocumentosValido = false;
            //    ActionResponse.ListActionListResponse.Add(new ActionErrorResponse() { Mensaje = "Debe seleccionar todo los archivos", NombreCampo = "Input.Files" });
            //}

            //else if (HttpContext.Request.Form.Files.Count() < Input.ListDocumentoTipoEntidad.listarDocumentosTipoEntidad.Count())
            //{
            //    blDocumentosValido = false;
            //    ActionResponse.ListActionListResponse.Add(new ActionErrorResponse() { Mensaje = "Debe adjuntar todo lo archivos", NombreCampo = "Input.Files" });
            //}

            if (ModelState.IsValid  && (blDocumentosValido) && (listVerificar.Count() > 0))  {

                VerificarSolicitudAccesoParameterVM verificarSolicitudAccesoParameter = new VerificarSolicitudAccesoParameterVM();
                verificarSolicitudAccesoParameter.CodigoTipoDocumento = Input.TipoDocumento;
                verificarSolicitudAccesoParameter.NumeroDocumento = Input.NumeroDocumento;
                verificarSolicitudAccesoParameter.Correo = Input.Correo;
                var veririficarSolicitudResult = await _serviceAcceso.VerificarSolicitudAcceso(verificarSolicitudAccesoParameter);

                if (veririficarSolicitudResult.CodigoResultado == 0)
                {
                    var resultValidarentidad = await EntidadPermitoRegistrar();
                    if ( resultValidarentidad.Respuesta==false)
                    {
                        ActionResponse.Codigo = -3;
                        ActionResponse.Mensaje = "Estimado usuario, no esta registrado en nuestra base de Transmares como:";
                        ActionResponse.Mensaje += "<br/>";
                        ActionResponse.Mensaje += string.Join(" ", resultValidarentidad.EntidadRespuestas.Select(e => e.Mensaje));
                    }
                    else
                    {
                        string document = Input.TipoDocumento;
                        SolicitarAccesoParameterVM solicitarAccesoVM = new SolicitarAccesoParameterVM();
                        solicitarAccesoVM.TipoDocumento = Input.TipoDocumento;
                        solicitarAccesoVM.NumeroDocumento = Input.NumeroDocumento;
                        solicitarAccesoVM.RazonSocial = Input.RazonSocial;
                        solicitarAccesoVM.RepresentaLegalNombre = Input.Nombres;
                        solicitarAccesoVM.RepresentaLegalApellidoPaterno = Input.ApellidoPaterno;
                        solicitarAccesoVM.RepresentaLegalMaterno = Input.ApellidoMaterno;
                        solicitarAccesoVM.Correo = Input.Correo;
                        solicitarAccesoVM.AcuerdoEndoceElectronico = Input.acuerdoCorrectoUsoEndosesElectronico;
                        solicitarAccesoVM.BrindaOpeCargaFCL = Input.seBrindaOperacionesCargaFCL;
                        solicitarAccesoVM.AcuerdoSeguroCadenaSuministro = Input.acuerdoSeguridadCadenaSuministra;
                        solicitarAccesoVM.DeclaracionJuradaVeracidadInfo = Input.declaracionJUridaVeracidadInformacio;
                        solicitarAccesoVM.BrindaAgenciamientoAduanas =Input.seBrinaAgenciaAdeuanas;

                        if (Input.ListTipoEntidad2.TiposEntidad.Exists(x => x.CodTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS)))
                        {
                            solicitarAccesoVM.CodigoSunat = Input.CodigoSunat;
                            solicitarAccesoVM.ProcesoFacturacion = Input.HabiliarProcesoFacturacion;
                            solicitarAccesoVM.TerminoCondicionGeneralContraTCGC = Input.TerminoCondicionesGeneralesContraTCGC;
                        }
                        else
                        {
                            solicitarAccesoVM.CodigoSunat = null;
                            solicitarAccesoVM.ProcesoFacturacion = null;
                            solicitarAccesoVM.TerminoCondicionGeneralContraTCGC = null;
                        }



                        solicitarAccesoVM.Documento = new List<ViewModel.Datos.SolictudAcceso.DocumentoVM>();

                        foreach (DocumentoTipoEntidadVM item in listDocumentosSeleccionados.listarDocumentosTipoEntidad)
                        {
                            solicitarAccesoVM.Documento.Add(new DocumentoVM() { CodigoDocumento = item.CodigoDocumento });
                        }

                        solicitarAccesoVM.TipoEntidad = new List<ViewModel.Datos.SolictudAcceso.TipoEntidadVM>();

                        foreach (TipoEntidad item in Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true))
                        {
                            solicitarAccesoVM.TipoEntidad.Add(new TipoEntidadVM() { CodigoTipoEntidad = item.CodTipoEntidad });
                        }


                        List<DocumentoVM> listDoc = new List<DocumentoVM>();

                        int i = 0;
                        foreach (IFormFile item in HttpContext.Request.Form.Files)
                        {
                            // relacionar archivos cargados con archivos que se tiene configurado
                            string[] strNombreSeparados = item.Name.Split('_');
                            string strCodigoDocumento = strNombreSeparados[1];
                            string strCorrelativo = strNombreSeparados[2];

                            var docSelect = solicitarAccesoVM.Documento[Convert.ToInt32(strCorrelativo)];
                            docSelect.UrlArchivo = await saveArchivo(item);
                            docSelect.NombreArchivo = item.FileName;
                            i++;
                        }


                        var jsonSolicitarAcceso = JsonConvert.SerializeObject(solicitarAccesoVM);

                        HttpContext.Session.SetString("SesionSolicitarAcceso", jsonSolicitarAcceso);

                        CodigoGeneradoValidacionParameterVM codigoGeneradoValidacionParameterVM = new CodigoGeneradoValidacionParameterVM();
                        codigoGeneradoValidacionParameterVM.CodigoTipoDocumento = solicitarAccesoVM.TipoDocumento;
                        codigoGeneradoValidacionParameterVM.NumeroDocumento = solicitarAccesoVM.NumeroDocumento;
                        codigoGeneradoValidacionParameterVM.Correo = solicitarAccesoVM.Correo;
                        codigoGeneradoValidacionParameterVM.Nombres = solicitarAccesoVM.RepresentaLegalNombre;

                        var codigoGenerado = await _serviceAcceso.GenerarCodigoVerificacion(codigoGeneradoValidacionParameterVM);


                        if (codigoGenerado.CodigoResultado == 0)
                        {
                            MensajeError = "Envio de código de verificación con éxito";
                            ActionResponse.Codigo = 0;
                            ActionResponse.Mensaje = "";
                        }
                        else
                        {
                            MensajeError = "Ocurrio un error al momento de validar la solicitud de acceso";
                            ActionResponse.Codigo = -1;
                            ActionResponse.Mensaje = MensajeError;
                        }


                    }


                }
                else
                {
                    MensajeError = veririficarSolicitudResult.MensajeResultado;
                    ActionResponse.Codigo = veririficarSolicitudResult.CodigoResultado;
                    ActionResponse.Mensaje = MensajeError;

                }

            } else {

                var erroresCampos = ModelState.Where(ms => ms.Value.Errors.Any())
                                      .Select(x => new { x.Key, x.Value.Errors });

                foreach (var erroneousField in erroresCampos)
                {
                    var fieldKey = erroneousField.Key;
                    var fieldErrors = string.Join(" | ", erroneousField.Errors.Select(e => e.ErrorMessage));

                    ActionResponse.ListActionListResponse.Add(new ActionErrorResponse()
                    {
                        Mensaje = fieldErrors,
                        NombreCampo = fieldKey
                    }); ;
                }

                ActionResponse.Codigo = -1;
                ActionResponse.Mensaje = "Por favor ingresar los campos requeridos.";

            }

            return new JsonResult(ActionResponse);

        }


        private async Task<EntidadValidar> EntidadPermitoRegistrar()
        {

            EntidadValidar entidadValidar = new EntidadValidar();
            List<EntidadTransmaresResponse> listResponseService = new List<EntidadTransmaresResponse>();

            bool ExisteAgenteAduanasProcesoFacturacion = false;

            var agenteAduanasSelecciono = Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true && x.CodTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS)).FirstOrDefault();

            if (agenteAduanasSelecciono != null && Input.HabiliarProcesoFacturacion == false)
            {
                ExisteAgenteAduanasProcesoFacturacion = true;
            }

            int resultValidarRegistro = 0;

            if (ExisteAgenteAduanasProcesoFacturacion == false)
            {
                // ini validar registro en transmares

              

                foreach (var itemTipoEntidad in Input.ListTipoEntidad2.TiposEntidad.Where(x => x.Check == true))
                {

                    EntidadTransmaresResponse entidadTransmaresResponse = new EntidadTransmaresResponse();


                    string codigoTipoEntidad = itemTipoEntidad.CodTipoEntidad;

                    string strTipoEntidadTransmares = "";

                    if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.AGENTE_ADUANAS;
                        entidadTransmaresResponse.TipoEntidad = "Agente de Aduanas";
                    }
                    else if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FINAL))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FINAL;
                        entidadTransmaresResponse.TipoEntidad = "Cliente Final";
                    }
                    else if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.BROKER))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.BROKET;
                        entidadTransmaresResponse.TipoEntidad = "Broker";
                    }
                    else if (codigoTipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER))
                    {
                        strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FORWARDER;
                        entidadTransmaresResponse.TipoEntidad = "Forwarder";
                    }


                    int intRespuestaServicioTrans = await _serviceEmbarqueExterno.ValidarRegistroEntidad(strTipoEntidadTransmares, Input.NumeroDocumento, Input.Correo);
                    entidadTransmaresResponse.Respuesta = intRespuestaServicioTrans;

                    if (intRespuestaServicioTrans == 0)
                    {
                        entidadTransmaresResponse.Mensaje = @$"<li>{ entidadTransmaresResponse.TipoEntidad}</li>";

                        listResponseService.Add(entidadTransmaresResponse);
                    }
                                 
                }

                if (listResponseService.Exists(x => x.Respuesta == 0))
                {
                    resultValidarRegistro = 0;
                }
                else {
                    resultValidarRegistro = 1;
                }




                if (resultValidarRegistro == 0)
                {
                    entidadValidar.Respuesta = false;
                    
                }
                else {
                    entidadValidar.Respuesta = true;
                    
                }


            }
            else {
                entidadValidar.Respuesta = true;
               
            }

            entidadValidar.EntidadRespuestas = listResponseService;

            return entidadValidar;





        }


        class EntidadValidar {
            public List<EntidadTransmaresResponse> EntidadRespuestas { get; set; }
            public bool Respuesta { get; set; }
        }

        class EntidadTransmaresResponse
        {

            public string Mensaje { get; set; }
            public string TipoEntidad { get; set; }
            public int Respuesta { get; set; }


        }


        private async Task<string> saveArchivo(IFormFile file)
        {
            string path = "";
            string strnombreFile = "";
            bool iscopied = false;


            try
            {
                if (file.Length > 0)
                {
                    strnombreFile = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\tmpdwac\\FTP\\"));
                    using (var filestream = new FileStream(Path.Combine(path, strnombreFile), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else
                {
                    iscopied = false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return strnombreFile;
        }

        public class InputModel 
        {
            [Display(Name = "Nro. de Documento (*)")]
            [Required(ErrorMessage = "Ingrese  número de documento")]
            [StringLength(11, ErrorMessage = "11 caracteres como máximo")]
            [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
            [CustomValidation(typeof(ValidationMethod), "ValidarNumeroDocumento")]
            public string NumeroDocumento { get; set; }

            [Display(Name = "Tipo de Documento (*)")]
            [RegularExpression(@"^.{3,}$", ErrorMessage = "Debe seleccionar un tipo de documento")]
            public string TipoDocumento { get; set; }

            [Display(Name = "Razón Social")]
            [CustomValidation(typeof(ValidationMethod), "ValidarRazonSocial")]
            public string RazonSocial { get; set; }

            [Display(Name = "Nombres (*)")]
            [Required(ErrorMessage = "Debe ingresar su Nombre")]
            [StringLength(100, ErrorMessage = "El Nombre ingresado tiene longitud invalida")]
            public string Nombres { get; set; }

            [Display(Name = "Apellido Paterno (*)")]
            [Required(ErrorMessage = "Debe ingresar su apellido paterno")]
            [StringLength(100, ErrorMessage = "El Apellido Paterno ingresado tiene longitud invalida")]
            public string ApellidoPaterno { get; set; }

            [Display(Name = "Apellido Materno (*)")]
            [Required(ErrorMessage = "Debe ingresar su apellido materno")]
            [StringLength(50, ErrorMessage = "El Apellido Materno ingresado tiene longitud invalida")]
            public string ApellidoMaterno { get; set; }

            [Display(Name = "Correo (*)")]
            [Required(ErrorMessage = "Debe ingresar el correo")]
            [EmailAddress(ErrorMessage = "El correo ingresado no es válido")]
            [StringLength(50, ErrorMessage = "El correo ingresado tiene longitud invalida")]
            public string Correo { get; set; }

            [Display(Name = "Codigo de Verificación (*)")]
            public string CodigoVerificacio { get; set; }
            

            [Display(Name = "Se Brindara Operaciones con Cargas FCL")]
            public bool seBrindaOperacionesCargaFCL { get; set; }

            [Display(Name = "Se Brindara Agenciamiento de Aduanas")]
            public bool seBrinaAgenciaAdeuanas { get; set; }

            [Display(Name = "Acuerdo para el Correcto Uso de los Endoses Electrónicos")]
            [CustomValidation(typeof(ValidationMethod), "ValidarEndose")]
            public bool acuerdoCorrectoUsoEndosesElectronico { get; set; }

            [Display(Name = "Acuerdo de Seguridad de la Cadena de Suministro")]
             [CustomValidation(typeof(ValidationMethod), "ValidarCadenaSuministro")]
            public bool acuerdoSeguridadCadenaSuministra { get; set; }

            [Display(Name = "Declaración Jurada de Veracidad de la Información")]
            [CustomValidation(typeof(ValidationMethod), "ValidarVeracidadInfo")]
            public bool declaracionJUridaVeracidadInformacio { get; set; }

            public string listaTipoClienteSeleccionado { get; set; }

            [Display(Name = "Código de Sunat (*)")]
            [StringLength(10, ErrorMessage = "Código de Sunat se permite como máximo 10 caracteres.")]
            [CustomValidation(typeof(ValidationMethod), "ValidarCodigoSunat")]
            public string CodigoSunat { get; set; }

            [Display(Name = "Habilitar Servicio de Proceso de Facturación")] 
            public bool HabiliarProcesoFacturacion { get; set; }

            [Display(Name = "Términos y Condiciones Generales de Contratación - T&CGC")]
            [CustomValidation(typeof(ValidationMethod), "ValidarTerminosCondiciones")]
            public bool TerminoCondicionesGeneralesContraTCGC { get; set; }

            public ListTipoEntidadModel ListTipoEntidad2 { get; set; }

            public ListarDocumentoTipoEntidadVM ListDocumentoTipoEntidad { get; set; }

            public SelectList ListTipoDocumento { get; set; }

            public List<FormFile> Files { get; set; }

            public ViewModel.Datos.SolictudAcceso.VerificarCodigoValidacionParameterVM ValidarCorreo { get; set; }


        }

    }

    public class ValidationMethod {
        public static ValidationResult ValidarRazonSocial(string value, System.ComponentModel.DataAnnotations.ValidationContext context) { 
        
            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;
            switch (viewModel.TipoDocumento) {
                case "RUC":
                    if ( string.IsNullOrEmpty(value))
                         return new ValidationResult("Ingresar la razón social");

                    break;
                default:
                    return ValidationResult.Success;            
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarNumeroDocumento(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))

                return ValidationResult.Success;

            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;
            switch (viewModel.TipoDocumento)
            {
                case "RUC":
          
                     if (value.Length!=11)
                        return new ValidationResult("Ingrese un RUC válido");
                    break;
                case "DNI":
                    if (value.Length!=8)
                        return new ValidationResult("Ingrese un DNI válido");
                    break;
                default:
                    return new ValidationResult("Ingresar número de documento");
            }

            return ValidationResult.Success;
        }


        public static ValidationResult ValidarVeracidadInfo(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad !=null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC01" && x.Check==true).Count();
                int cantidadTC02 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC02" && x.Check == true).Count();
                int cantidadTC03 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC03" && x.Check == true).Count();
                int cantidadTC04 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC04" && x.Check == true).Count();

                if (cantidadTC01 >= 1 || cantidadTC02 >= 1 || cantidadTC03 >= 1 || cantidadTC04 >= 1)
                {

                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }


                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarCadenaSuministro(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC01" && x.Check == true).Count();
                int cantidadTC02 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC02" && x.Check == true).Count();
                int cantidadTC04 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC04" && x.Check == true).Count();

                if (cantidadTC01 >= 1 || cantidadTC02 >= 1  || cantidadTC04 >= 1)
                {
                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }
                }
            }

            return ValidationResult.Success;
        }


        public static ValidationResult ValidarTerminosCondiciones(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS && x.Check == true).Count();


                if (cantidadTC01 >= 1 )
                {
                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarOperacionCargaFCL(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            //var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;


            if (!Convert.ToBoolean(value))
            {
                return new ValidationResult("Esta opción es requerido");
            }


            return ValidationResult.Success;
        }

        public static ValidationResult ValidarEndose(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC01" && x.Check == true).Count();
                int cantidadTC04 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC04" && x.Check == true).Count();

                if (cantidadTC01 >= 1 || cantidadTC04 >= 1)
                {

                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }


                }
            }

            return ValidationResult.Success;
        }


        public static ValidationResult ValidarCodigoSunat(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
      


            var viewModel = context.ObjectInstance as SolicitarAccesoModel.InputModel;

            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int agenteAduanasCantidad = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS) && x.Check == true).Count();
                

                if (agenteAduanasCantidad >= 1 )
                {
               
                    if (string.IsNullOrEmpty(value))
                    {
                        return new ValidationResult(Utilitario.Constante.SolicitudAccesoConstante.MensajeRegistroDatosEntidad.MENSAJE_CODIGO_SUNAT);
                    }


                }
            }

            return ValidationResult.Success;
        }
    }
}
