using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Web.LibroReclamaciones.ServiceConsumer;
using Web.LibroReclamaciones.Utilitario;

namespace Web.Principal.Pages.libroreclamaciones
{
    public class quejaModel : PageModel
    {

        private readonly ServicioMaestro _serviceMaestro;
        private readonly ILogger<quejaModel> _logger;
        private readonly IMapper _mapper;



        [BindProperty]
        public InputModel Input { get; set; }


        [TempData]
        public string MensajeError { get; set; }

        public ActionResponse ActionResponse { get; set; }


        public string ReturnUrl { get; set; }


        public quejaModel(ILogger<quejaModel> logger,
                            ServicioMaestro serviicoMaestro,
                            IMapper mapper)
        {

            _logger = logger;
            _serviceMaestro = serviicoMaestro;
            _mapper = mapper;

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




            if (ModelState.IsValid)
            {


            }

            return new JsonResult(ActionResponse);

        }


    }

    public class InputModel
    {


        [Display(Name = "Ruc (*)")]
        [Required(ErrorMessage = "Ingrese  el RUC")]
        [StringLength(11, ErrorMessage = "11 caracteres como máximo")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string Ruc { get; set; }


        [Display(Name = "Tipo de Documento por la Cual Reclama (*)")]
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Seleccionar un tipo de documento")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Empresa que lo Atendió (*)")]
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Seleccionar empresa que lo antendió")]
        public string EmpresaAtiende { get; set; }

        [Display(Name = "Unidad de Negocio (*)")]
        [RegularExpression(@"^.{2,}$", ErrorMessage = "Seleccionar unidad de negocio")]
        public string UnidadNegocio { get; set; }

        [Display(Name = "Razón Social")]
        [Required(ErrorMessage = "Ingrese  el razón social")]
        [StringLength(250, ErrorMessage = "El nombre completo no debe execeder de 250 caracteres")]
        public string RazonSocial { get; set; }

        [Display(Name = "Nombre Completo (*)")]
        [Required(ErrorMessage = "Ingrese su nombre completo")]
        [StringLength(250, ErrorMessage = "El nombre completo no debe execeder de 250 caracteres")]
        public string NombreCompleto { get; set; }

        [Display(Name = "Mensaje (*)")]
        [Required(ErrorMessage = "Ingrese el mensaje")]
        [StringLength(4000, ErrorMessage = "El mensaje no debe execeder de 4000 caracteres")]
        public string Mensaje { get; set; }

        [Display(Name = "Email (*)")]
        [Required(ErrorMessage = "Ingrese Email")]
        [EmailAddress(ErrorMessage = "El Email ingresado no es válido")]
        [StringLength(150, ErrorMessage = "El Email ingresado tiene longitud invalida")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El Email ingresado no tiene el formato correcto")]
        public string Email { get; set; }


        [Display(Name = "Celular (*)")]
        [Required(ErrorMessage = "Ingrese  número de celular")]
        [StringLength(9, ErrorMessage = "11 caracteres como máximo")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        public string Celular { get; set; }

        [Display(Name = "Fecha Incidencia (*)")]
        [Required(ErrorMessage = "Ingrese  fecha de incidencia")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaIncidencia { get; set; }


        public string EmpresaAtiendeNombre { get; set; }
        public string UnidadNegocioNombre { get; set; }
        public string TipoDocumentoNombre { get; set; }

        public SelectList ListEmpresaAtendio { get; set; }

        public SelectList ListUnidadNegocio { get; set; }

        public SelectList ListTipoDocumento { get; set; }

    }

    public class ValidationMethod
    {
      
    }

}
