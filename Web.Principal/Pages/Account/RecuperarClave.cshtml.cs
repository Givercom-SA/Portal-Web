using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ViewModel.Datos.LoginInicial;
using Web.Principal.ServiceConsumer;
using Web.Principal.Util;

namespace Web.Principal.Pages.Account
{
    public class RecuperarClaveModel : PageModel
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ILogger<RecuperarClaveModel> _logger;

        public RecuperarClaveModel(
            ILogger<RecuperarClaveModel> logger,
            ServicioAcceso serviceAcceso)
        {
            _logger = logger;
            _serviceAcceso = serviceAcceso;
        }


        [BindProperty]
        public InputCorreoModel InputCorreo { get; set; }

        [BindProperty]
        public InputClaveModel InputClave { get; set; }

        [BindProperty]
        public InputCodigoModel InputCodigo { get; set; }

        [TempData]
        public string MensajeError { get; set; }

        [TempData]
        public bool EsCodigoValido { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet()
        {
            EsCodigoValido = false;
        }

        public IActionResult OnGetGenerarCodigo(string Correo)
        {
            return new JsonResult(new { codigo= "D5FGHH56" });
        }

        public async Task<IActionResult> OnPost()
        {

            EsCodigoValido = true;
            InputClave.Correo = InputCodigo.Correo;
            return Page();
        }

        public class InputCorreoModel
        {
            [Display(Name = "Correo")]
            [Required(ErrorMessage = "Debe ingresar el correo")]
            [EmailAddress(ErrorMessage = "El correo ingresado no es v�lido")]
            [StringLength(50, ErrorMessage = "El correo ingresado tiene longitud invalida")]

            public string Correo { get; set; }
        }

        public class InputCodigoModel
        {
            [Display(Name = "C�digo")]
            [Required(ErrorMessage = "Debe ingresar el c�digo")]
            [StringLength(10, ErrorMessage = "El correo ingresado tiene longitud invalida")]
            public string Codigo { get; set; }
            public string Correo { get; set; }
        }

        public class InputClaveModel
        {
            [Display(Name = "Nueva Contrase�a")]
            [Required(ErrorMessage = "Debe ingresar una nueva contrase�a")]
            [StringLength(50, ErrorMessage = "La contrase�a ingresada tiene longitud invalida")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "La contrase�a debe contener al menos una may�scula, una min�scula, al menos un n�mero, un caracter especial (ej. !@#$%^&*) y m�nimo debe ser de 8 caracteres.")]
            [DataType(DataType.Password)]
            public string Contrasenia { get; set; }

            [Display(Name = "Confirmar Contrase�a")]
            [Required(ErrorMessage = "Debe Confirmar su contrase�a")]
            [Compare("Contrasenia", ErrorMessage = "Nueva Contrase�a y Confirmar Contrase�a no coinciden.")]
            [DataType(DataType.Password)]
            public string ConfirmarContrasenia { get; set; }
            public string Correo { get; set; }
        }
    }
}
