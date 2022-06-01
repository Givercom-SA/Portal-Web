
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
    public class SeguridadController : Controller
    {

        private readonly ILogger<SeguridadController> _logger;
        private readonly IConfiguration _configuration;
      
        public SeguridadController(ILogger<SeguridadController> logger, IConfiguration configuration) {

            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult RequestTimeout()
        {
            return View();
        }

        [HttpGet("KeepAlive", Name = "KeepAlive")]
        public ActionResult KeepAlive()
        {
            return Json("OK");
        }


        [HttpGet("CerrarSesionAsync", Name = "CerrarSesionAsync"), HttpPost("CerrarSesionAsync", Name = "CerrarSesionAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesionAsync()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("CoreSessionDemo");
                HttpContext.Session.Clear();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                
            }
            return Redirect("~/");
        }


        [AllowAnonymous]
        public  IActionResult Login()
        {
          
            return Redirect("~/");
        }


    }
}
