using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Principal.Model;
using Web.Principal.Util;

namespace Web.Principal.Controllers
{
    public class HomeController : BaseLibreController
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            ErrorViewModel error = new ErrorViewModel();
            if (statusCode != null)
            {
                switch (statusCode)
                {
                    case 404:
                        error = new ErrorViewModel
                        {
                            RequestId = Convert.ToString(statusCode),
                            ErrorMessage = "La vista a la que desea acceder no esta disponible o no tiene acceso.",
                        };
                        break;

                    default:
                        error = new ErrorViewModel
                        {
                            RequestId = Convert.ToString(statusCode),
                            ErrorMessage = "Ocurrio un error inesperado por favor volver a intentar.",
                        };
                        break;
                }
            }
            return View(error);
        }
    }
}