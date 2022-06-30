using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.Vista
{
    public class VistaVM
    {


    

        public int IdVista { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(100, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string VistaNombre { get; set; }
        [Display(Name = "Area")]
        [StringLength(100, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un area")]
        public string VistaArea { get; set; }
        [Display(Name = "Controller")]
        [StringLength(100, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un controller")]
        public string VistaController { get; set; }
        [Display(Name = "Acción")]
        [StringLength(100, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar una acción")]
        public string VistaAction { get; set; }
        [Display(Name = "Verbo")]
        [StringLength(100, ErrorMessage = "El nombre tiene una longitud inválida")]
        [Required(ErrorMessage = "Debe ingresar un verbo")]
        public string VistaVerbo { get; set; }
        [Display(Name = "Principal")]
        [RegularExpression(@"(^[0-1]+$)", ErrorMessage = "Debe indicar si es principal")]
        [Required(ErrorMessage = "Debe indicar si es principal")]
        public int VistaPrincipal { get; set; }

        [Display(Name = "Opción")]
        [RegularExpression(@"(^[0-1]+$)", ErrorMessage = "Debe ingresar una Opción")]
        [Required(ErrorMessage = "Debe ingresar una Opción")]
        public int VistaOpcion { get; set; }

        public bool Activo { get; set; }

        public int? IdPadre { get; set; }

        [Display(Name = "Orden")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(ErrorMessage = "Debe ingresar un orden")]
        public int vistaOrden { get; set; }
        public int IdSesion { get; set; }

        public int? IdUsuarioCrea { get; set; }
        public int? IdUsuarioModifica { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public DateTime? FechaModifica { get; set; }


        public string UsuarioCreaNombres { get; set; }
        public string UsuarioModificaNombres { get; set; }
        public string NombreControlHtml { get; set; }
        
    }


    public class ValidationMethod
    {
        public static ValidationResult ValidarOpcion(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {

            var viewModel = context.ObjectInstance as VistaVM;
            if (viewModel.VistaOpcion == null)
            {
                return new ValidationResult("Ingresar la razón social");
            }
           

            return ValidationResult.Success;
        }

      



      


        
    }

}
