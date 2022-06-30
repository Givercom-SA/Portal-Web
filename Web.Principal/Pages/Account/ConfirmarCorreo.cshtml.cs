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
    public class ConfirmarCorreoModel : PageModel
    {
        private readonly ServicioAcceso _serviceAcceso;
        private readonly ServicioUsuario _serviceUsuario;
        private readonly ILogger<LoginModel> _logger;

        public ConfirmarCorreoModel(
            ILogger<LoginModel> logger,
            ServicioAcceso serviceAcceso,
            ServicioUsuario serviceUsuario)
        {
            _logger = logger;
            _serviceAcceso = serviceAcceso;
            _serviceUsuario = serviceUsuario;
        }

        public async Task OnGet(string token)
        {
            if(string.IsNullOrWhiteSpace(token))
                Response.Redirect("/");
            else
            {
                var result =await _serviceUsuario.ConfirmarCorreoUsuario(Int32.Parse(Security.Common.Encriptador.Instance.DesencriptarTexto(token)));
                if(result.CodigoResultado == -1)
                    Response.Redirect("/");
            }
        }

        public IActionResult OnPost()
        {
            return Page();
        }

    }
}
