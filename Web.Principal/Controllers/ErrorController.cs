using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Logging;
using Service.Common.Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.UsuarioRegistro;
using Web.Principal.Util;

namespace Web.Principal.Controllers
{
    [AllowAnonymous]

    public class ErrorController : BaseLibreController
    {
        private readonly ILogger<ErrorController> _logger;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;


        public ErrorController(ILogger<ErrorController> logger, Microsoft.Extensions.Configuration.IConfiguration configuration) {
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult PaginaNoEncontrada()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PaginaNoEncontrado()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PaginaError()
        {
            return View();
        }

    
        [AllowAnonymous]
        public IActionResult NavegadorNoSoportado()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("PaginaExpirada", Name = "PaginaExpirada")]
        public IActionResult PaginaExpirada()
        {
            var user = HttpContext.Session.GetSession<UsuarioRegistroVM>("UserDefault");
          
            if (user != null)
                Log4netExtensions.Usuario(user.CorreoUsuario.ToString(), user.CorreoUsuario );

            _logger.LogInformation("Session_Fin_Settings_TimeExpired: " + _configuration.GetSection("Session")["TimeExpired"]);
            var value = ViewResultExtensions.GetInstanceField(typeof(DistributedSession), HttpContext.Session, "_idleTimeout");
            _logger.LogInformation("Session_Fin_IdleTimeout: " + ((TimeSpan)value).TotalSeconds);

            HttpContext.Response.Cookies.Delete("CoreSessionDemo");
            HttpContext.Session.Clear();
            return View();
        }
  
    }
}