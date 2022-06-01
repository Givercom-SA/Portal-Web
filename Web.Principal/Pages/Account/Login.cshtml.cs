using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CapchaDLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ViewModel.Datos.Acceso;
using ViewModel.Datos.LoginInicial;
using Web.Principal.ServiceConsumer;
using Web.Principal.Util;

namespace Web.Principal.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            IConfiguration configuration,
            ILogger<LoginModel> logger,
            ServicioAcceso serviceAcceso)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceAcceso = serviceAcceso;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string MensajeError { get; set; }

        public string ReturnUrl { get; set; }

        public string ImgCaptcha { get; set; }

        public string ImgCode { get; set; }

        public async Task OnGet()
        {
            MensajeError = string.Empty;
            HttpContext.Response.Cookies.Delete("CoreSessionDemo");
            HttpContext.Session.Clear();
            
            Input = new InputModel();

            var sEncryptionKey = $"{_configuration["Captcha:EncryptionKey"]}";
            var captchaEntity = CreateCaptchaFromEntity();
            ImgCaptcha = captchaEntity.Value;
            ImgCode = new Utilitario.Seguridad.Encrypt().EncryptAdvanced(captchaEntity.Code, sEncryptionKey.ToString());

            var listEmpresas = await CargarEmpresas();

            if (listEmpresas != null)
            {
                if (listEmpresas.CodigoResultado == 0)
                {
                    Input.ListarEmpresas = new SelectList(listEmpresas.Empresa, "Codigo", "Nombres");
                }
                else
                    MensajeError = MensajeError + " " + listEmpresas.MensajeResultado;
            }
        }

        private async Task<ListarTransGroupEmpresaVM> CargarEmpresas() 
        {
            var listEmpresas = await _serviceAcceso.ObetenerTransGroupEmpresas();
            return listEmpresas;
        }



        public async Task<IActionResult> OnPostAsync()
        {
            ValidarCaptcha();

            var listEmpresas = await CargarEmpresas();

            if (listEmpresas != null)
            {
                if (listEmpresas.CodigoResultado == 0)
                {
                    Input.ListarEmpresas = new SelectList(listEmpresas.Empresa, "Codigo", "Nombres");
                }
                else
                    MensajeError = MensajeError + " " + listEmpresas.MensajeResultado;
            }

            var sEncryptionKey = $"{_configuration["Captcha:EncryptionKey"]}";
            var captchaEntity = CreateCaptchaFromEntity();
            ImgCaptcha = captchaEntity.Value;
            ImgCode = new Utilitario.Seguridad.Encrypt().EncryptAdvanced(captchaEntity.Code, sEncryptionKey.ToString());

            if (ModelState.IsValid)
            {
                var loginInicial = new LoginInicialVW();
                loginInicial.CorreoElectronico = Input.Correo;
                loginInicial.Contrasenia = new Utilitario.Seguridad.Encrypt().GetSHA256(Input.Contrasenia);
                loginInicial.EntidadRuc = Input.Ruc;

                var empresaSelect = listEmpresas.Empresa.Where(x => x.Codigo == Input.Empresa).FirstOrDefault();


                var user = await _serviceAcceso.LoginInicial(loginInicial);
                user.Sesion = new ViewModel.Datos.UsuarioRegistro.SesionUsuarioVM();
                user.Sesion.FechaInicioSesion = DateTime.Now;
                user.Sesion.CodigoTransGroupEmpresaSeleccionado = Input.Empresa;
                user.Sesion.RucTransGroupEmpresaSeleccionado = "";
                user.Sesion.RucIngresadoUsuario = Input.Ruc;
                user.Sesion.NombreTransGroupEmpresaSeleccionado = "";
                user.Sesion.ImagenTransGroupEmpresaSeleccionado = "";
                user.Empresas = listEmpresas;
                user.IdUsuarioInicioSesion = user.idUsuario;

                if (user.AdminSistema == 1)
                {

                    user.ModoAdminSistema = Utilitario.Constante.SeguridadConstante.ModoVisualizacion.ADMINISTRADOR.ToString();
                }
                else {
                    user.ModoAdminSistema = Utilitario.Constante.SeguridadConstante.ModoVisualizacion.USUARIO.ToString();
                }

                
                if (user.CodigoResultado == 0)
                {
                    HttpContext.Session.SetUserContent(user);
                    return Redirect("/GestionarDashboards/Inicio/Home");
                }
                else
                    MensajeError = user.MensajeResultado;
            }

            Input.CodigoCaptcha = "";
            Input.CodigoCaptchaValidate = "";
            

            return Page();
        }

        public class InputModel
        {
            [Display(Name = "Correo (*)")]
            [Required(ErrorMessage = "Debe ingresar su correo")]
            [EmailAddress(ErrorMessage = "El correo ingresado no es válido")]
            [StringLength(50, ErrorMessage = "El correo ingresado tiene longitud inválida")]
            public string Correo { get; set; }

  
            [Display(Name = "Empresa Transmares Group(*)")]
            [Required(ErrorMessage = "Debe seleccionar una empresa")]
            //[RegularExpression(@"^.{2,}$", ErrorMessage = "Seleccione una empresa")]
            public string Empresa { get; set; }


            [Display(Name = "Ruc")]
            
            [StringLength(11, ErrorMessage = "Solo se permite ingresar 11 caracteres como máximo")]
            public string Ruc { get; set; }

            public SelectList ListarEmpresas { get; set; }

            [Display(Name = "Contraseña (*)")]
            [Required(ErrorMessage = "Debe ingresar su contraseña")]
            [DataType(DataType.Password)]
            public string Contrasenia { get; set; }

            [Required(ErrorMessage = "Debe ingresar el texto de la imagen")]
            public string CodigoCaptcha { get; set; }

            public string CodigoCaptchaValidate { get; set; }

        }

        private class Captcha
        {
            public string Code { get; set; }
            public string Value { get; set; }
        }

        private Captcha CreateCaptchaFromEntity()
        {
            string sCode = CaptchaImage.GenerateRandomCode(CaptchaType.AlphaNumeric, 4);
            MemoryStream stream = new MemoryStream();
            CaptchaImage ci = new CaptchaImage(sCode, 106, 33, "Arial", System.Drawing.Color.FromArgb(52, 62, 106), System.Drawing.Color.FromArgb(238, 245, 253));
            ci.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            ci.Dispose();
            byte[] buf = new byte[stream.Position];
            stream.Position = 0;
            stream.Read(buf, 0, buf.Length);
            var base64 = Convert.ToBase64String(buf);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
            return new Captcha() { Code = sCode, Value = imgSrc };
        }

        private void ValidarCaptcha()
        {
            var sEncryptionKey = $"{_configuration["Captcha:EncryptionKey"]}";
            var sCaptchaValidate = new Utilitario.Seguridad.Encrypt().DecryptAdvanced(Input.CodigoCaptchaValidate, sEncryptionKey.ToString());
            var sCaptcha = Input.CodigoCaptcha;

            if (sCaptchaValidate != null && sCaptcha != null)
            {
                if (sCaptcha.ToUpper() != sCaptchaValidate.ToUpper())
                {
                    ModelState.AddModelError("Captcha", "El texto ingresado es incorrecto.");
                    MensajeError = "El texto del Captcha ingresado es incorrecto.";
                }
            }
        }
    }
}
