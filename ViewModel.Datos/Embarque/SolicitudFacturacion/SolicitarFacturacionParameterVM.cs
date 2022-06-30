using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;

namespace ViewModel.Datos.Embarque.SolicitudFacturacion
{
    public class SolicitarFacturacionParameterVM
    {
        public string KEYBLD { get; set; }
        public string NroBl { get; set; }
        public bool AplicaCredito { get; set; }
        public string CodigoCredito { get; set; }
        public string TipoPago { get; set; }
        public string MetodoPago { get; set; }

        [Display(Name = "He leído y estoy de acuerdo con los términos y condiciones de la web.")]
        [CustomValidation(typeof(ValidationMethod), "ValidarTerminosCondiciones")]
        public bool AceptoFormulario { get; set; }
        public Int32 IdEntidadSolicita { get; set; }
        public Int32 IdUsuarioSolicita { get; set; }
        public Int32 IdUsuarioCrea { get; set; }
        
        public string CreditoDescripcion { get; set; }

        public string CodigoTipoEntidad { get; set; }
        public string CodigoEmpresaGtrm { get; set; }
        
        public string TipoPagoString()
        {


            if (TipoPago.Equals(EmbarqueConstante.TipoPago.CONTADO))
            {
                return "Contado";
            }
            else
            {
                return "Credito";
            }


        }

        public string ProvisionSeleccionado { get; set; }

        [CustomValidation(typeof(ValidationMethod), "ValidarCodigoOperacionTransferencia")]
        public string CodigoOperacionTransferencia { get; set; }
        public string BancoTransferencia { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [CustomValidation(typeof(ValidationMethod), "ValidarFechaTransferencia")]
        public string FechaTransferencia { get; set; }

        public string FechaTope { get; set; }

        
        [CustomValidation(typeof(ValidationMethod), "ValidarImporteTransferencia",ErrorMessage = "Se requiere el importe de transferencia")]
        public double? ImporteTransferencia { get; set; }


        public string Servicio { get; set; }
        public string Origen { get; set; }

        public string ParKey { get; set; }

        public List<CobroClienteVM> CobrosPendientesCliente { get; set; }

    }

    public class CobroClienteVM
    {
        public string IdFacturacionTercero { get; set; }
        public string IdCliente { get; set; }
        public string TipoDocumentoCliente { get; set; }
        public string NroDocumentoCliente { get; set; }
        public string RazonSocialCliente { get; set; }
        public double MontoTotal { get; set; }
        public List<CobrosPendienteEmbarqueVM> CobrosPendientesEmbarque { get; set; }
    }


    public class ValidationMethod
    {

        public static ValidationResult ValidarTerminosCondiciones(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
           

            var viewModel = context.ObjectInstance as SolicitarFacturacionParameterVM;

            if (viewModel.TipoPago.Equals(Utilitario.Constante.EmbarqueConstante.TipoPago.CREDITO) )
            {
                if (!Convert.ToBoolean(value))
                {
                    return new ValidationResult("Esta opción es requerido");
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarCodigoOperacionTransferencia(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
        

            var viewModel = context.ObjectInstance as SolicitarFacturacionParameterVM;

            if (viewModel.TipoPago.Equals(Utilitario.Constante.EmbarqueConstante.TipoPago.CONTADO) && viewModel.MetodoPago.Equals(Utilitario.Constante.EmbarqueConstante.MetodoPago.TRANSFERENCIA))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new ValidationResult("Se requiere el código de operación de transferencia");
                }
            }

            return ValidationResult.Success;
        }


        public static ValidationResult ValidarImporteTransferencia(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
       

            var viewModel = context.ObjectInstance as SolicitarFacturacionParameterVM;

            if (viewModel.TipoPago.Equals(Utilitario.Constante.EmbarqueConstante.TipoPago.CONTADO) && viewModel.MetodoPago.Equals(Utilitario.Constante.EmbarqueConstante.MetodoPago.TRANSFERENCIA))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new ValidationResult("Se requiere el importe de transferencia");
                }
            }

            return ValidationResult.Success;

        }
        public static ValidationResult ValidarFechaTransferencia(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
        

            var viewModel = context.ObjectInstance as SolicitarFacturacionParameterVM;

            if (viewModel.TipoPago.Equals(Utilitario.Constante.EmbarqueConstante.TipoPago.CONTADO) && viewModel.MetodoPago.Equals(Utilitario.Constante.EmbarqueConstante.MetodoPago.TRANSFERENCIA))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return new ValidationResult("Se requiere la fecha de transferencia");
                }
            }

            return ValidationResult.Success;

        }


    }

}