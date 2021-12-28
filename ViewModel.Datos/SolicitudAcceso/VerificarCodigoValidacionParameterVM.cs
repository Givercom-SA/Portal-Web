using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.SolictudAcceso
{


    public class VerificarCodigoValidacionParameterVM
    {
        public string CodigoTipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }

     
        [Display(Name = " Código de verificación (*)")]
        [Required(ErrorMessage = "Se requiere código de verificación")]

        public string CodigoVerificacion { get; set; }



        [Display(Name = "Correo")]
 
        public string Correo { get; set; }



    }



}

