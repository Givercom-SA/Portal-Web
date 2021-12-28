using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ViewModel.Datos.SolictudAcceso;

namespace Web.Principal.Pages.Account
{
    public class SolicitudAccesoRegistradoModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string MensajeError { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet()
        {
            SolicitarAccesoParameterVM solicitarAccesoVM = new SolicitarAccesoParameterVM();
            Input = new InputModel();



            string strSesion = HttpContext.Session.GetString("SesionSolicitarAcceso");

            solicitarAccesoVM = JsonConvert.DeserializeObject<SolicitarAccesoParameterVM>(strSesion);

            Input.Correo = solicitarAccesoVM.Correo;

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {


                return RedirectToPage("Login");


            }
            else
                MensajeError = string.Empty;

            return Page();
        }

        public class InputModel
        {
           
            public string NumeroDocumento { get; set; }


            public string RazonSocial { get; set; }

       
            public string Nombres { get; set; }

         
            public string ApellidoPaterno { get; set; }

       
            public string ApellidoMaterno { get; set; }

        
            public string Correo { get; set; }

          
        }
    }
}
