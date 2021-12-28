using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace ViewModel.Datos.Embarque.SolicitudFacturacionTercero
{
    public class SolicitarFacturacionTerceroParameterVM
    {
        public string KEYBLD { get; set; }

        [Display(Name = "Tipo de documento")]
        public string TipoDocumento { get; set; }

        [Display(Name = "Nro. de documento (*)")]
        [StringLength(11, ErrorMessage = "11 caracteres como máximo")]

        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        [CustomValidation(typeof(ValidationMethod), "ValidarNumeroDocumento")]
        public string  NumeroDocumento { get; set; }


        [Display(Name = "Razón Social / Nombres")]
        [RegularExpression(@"^.{5,}$", ErrorMessage = "Debe ingresar como mínimo 5 caracteres")]
        public string RazonSocialNombres { get; set; }
        
        public string Archivo { get; set; }

        public string TipoEntidad { get; set; }

        [Display(Name = "Tipo de documento")]
        public string AgenteAduanasTipoDocumento { get; set; }
        [Display(Name = "Numero de documento")]
        public string AgenteAduanasNumeroDocumento { get; set; }
        [Display(Name = "Razón social")]
        public string AgenteAduanasRazonSocial { get; set; }

        public bool AceptarCondicion { get; set; }

        
        public string ClienteSeleccionado { get; set; }
        public string ClienteNroDocumento { get; set; }
        public string ClienteRazonNombre { get; set; }

        public List<ClienteFacturarTerceroVM> ClientesFacturarTerceros { get; set; }

    }

    public class ClienteFacturarTerceroVM {

        public string CodigoClienteFacturarTercero { get; set; }
        public string NombresClienteFacturarTercero { get; set; }
        public string NroDocumentoClienteFacturarTercero { get; set; }
        public string CodigoAlmacen { get; set; }
        public bool Check { get; set; }

    }

    public class ValidationMethod
    {

        public static ValidationResult ValidarNumeroDocumento(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))

                return ValidationResult.Success;

            var viewModel = context.ObjectInstance as SolicitarFacturacionTerceroParameterVM;
            switch (viewModel.TipoDocumento)
            {
                case "RUC":

                    if (value.Length != 11)
                        return new ValidationResult("Ingrese un RUC válido");
                    break;
                case "DNI":
                    if (value.Length != 8)
                        return new ValidationResult("Ingrese un DNI válido");
                    break;
                default:
                    return new ValidationResult("Ingresar número de documento");
            }

            return ValidationResult.Success;
        }


    }

}
