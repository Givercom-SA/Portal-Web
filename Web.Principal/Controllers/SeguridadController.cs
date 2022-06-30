
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.Util;

namespace Web.Principal.Controllers
{

    [AllowAnonymous]
    public class SeguridadController : BaseLibreController
    {

        private readonly ILogger<SeguridadController> _logger;
        private readonly IConfiguration _configuration;
      
        public SeguridadController(ILogger<SeguridadController> logger, IConfiguration configuration) {

            _logger = logger;
            _configuration = configuration;
        }



        [HttpGet("RequestTimeout", Name = "RequestTimeout")]
        public IActionResult RequestTimeout()
        {
            return View();
        }

        [HttpGet("KeepAlive", Name = "KeepAlive")]
        public ActionResult KeepAlive()
        {
            return Json("OK");
        }


    

        [HttpGet ("CerrarSesionAsync", Name = "CerrarSesionAsync"), HttpPost ("CerrarSesionAsync", Name = "CerrarSesionAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesionAsync()
        {
            try
            {
               var usuario = HttpContext.Session.GetUserContent();
                HttpContext.Response.Cookies.Delete("CoreSessionDemo");
                HttpContext.Session.Clear();

                return Redirect("~/");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Redirect("~/");
            }
        }

        [HttpGet("Seguridad/Login", Name = "Login"), HttpPost("Seguridad/Login", Name = "Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            try
            {
               

                return Redirect("~/");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Redirect("~/");
            }
        }




    }
}
