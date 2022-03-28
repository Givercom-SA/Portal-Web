using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Documento;
using Web.Principal.Model;

namespace Web.Principal.Areas.GestionarUsuarios.Models
{
    public class SolicitarAccesoModel
    {

        public string ReturnUrl { set; get; }
        public string MensajeError { set; get; }
        
    

        [Display(Name = "Se Brindara Operaciones con Cargas FCL")]
        public bool seBrindaOperacionesCargaFCL { get; set; }

        [Display(Name = "Se Brindara Agenciamiento de Aduanas")]
        public bool seBrinaAgenciaAdeuanas { get; set; }

        [Display(Name = "Acuerdo para el Correcto Uso de los Endoses Electrónicos")]
        [CustomValidation(typeof(ValidationMethod), "ValidarEndose")]
        public bool acuerdoCorrectoUsoEndosesElectronico { get; set; }

        [Display(Name = "Acuerdo de Seguridad de la Cadena de Suministro")]
        [CustomValidation(typeof(ValidationMethod), "ValidarCadenaSuministro")]
        public bool acuerdoSeguridadCadenaSuministra { get; set; }

        [Display(Name = "Declaración Jurada de Veracidad de la Información")]
        [CustomValidation(typeof(ValidationMethod), "ValidarVeracidadInfo")]
        public bool declaracionJUridaVeracidadInformacio { get; set; }

        public string listaTipoClienteSeleccionado { get; set; }

        [Display(Name = "Código de Sunat (*)")]
        [StringLength(10, ErrorMessage = "Código de Sunat se permite como máximo 10 caracteres.")]
        [CustomValidation(typeof(ValidationMethod), "ValidarCodigoSunat")]
        public string CodigoSunat { get; set; }

        [Display(Name = "Habilitar Servicio de Proceso de Facturación")]
        public bool HabiliarProcesoFacturacion { get; set; }

        [Display(Name = "Términos y Condiciones Generales de Contratación - T&CGC")]
        [CustomValidation(typeof(ValidationMethod), "ValidarTerminosCondiciones")]
        public bool TerminoCondicionesGeneralesContraTCGC { get; set; }


        public ListTipoEntidadModel ListTipoEntidad2 { get; set; }

        public ListarDocumentoTipoEntidadVM ListDocumentoTipoEntidad { get; set; }

        public SelectList ListTipoDocumento { get; set; }

        public List<FormFile> Files { get; set; }

        public ViewModel.Datos.SolictudAcceso.VerificarCodigoValidacionParameterVM ValidarCorreo { get; set; }

    }
    public class ValidationMethod
    {
       

       


        public static ValidationResult ValidarVeracidadInfo(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC01" && x.Check == true).Count();
                int cantidadTC02 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC02" && x.Check == true).Count();
                int cantidadTC03 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC03" && x.Check == true).Count();
                int cantidadTC04 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC04" && x.Check == true).Count();

                if (cantidadTC01 >= 1 || cantidadTC02 >= 1 || cantidadTC03 >= 1 || cantidadTC04 >= 1)
                {

                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }


                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarCadenaSuministro(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC01" && x.Check == true).Count();
                int cantidadTC02 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC02" && x.Check == true).Count();
                int cantidadTC04 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC04" && x.Check == true).Count();

                if (cantidadTC01 >= 1 || cantidadTC02 >= 1 || cantidadTC04 >= 1)
                {
                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }
                }
            }

            return ValidationResult.Success;
        }


        public static ValidationResult ValidarTerminosCondiciones(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS && x.Check == true).Count();


                if (cantidadTC01 >= 1)
                {
                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }
                }
            }

            return ValidationResult.Success;
        }

        public static ValidationResult ValidarOperacionCargaFCL(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;

            if (!Convert.ToBoolean(value))
            {
                return new ValidationResult("Esta opción es requerido");
            }


            return ValidationResult.Success;
        }

        public static ValidationResult ValidarEndose(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {
            if (string.IsNullOrEmpty(value))
                return ValidationResult.Success;


            var viewModel = context.ObjectInstance as SolicitarAccesoModel;
            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int cantidadTC01 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC01" && x.Check == true).Count();
                int cantidadTC04 = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad == "TC04" && x.Check == true).Count();

                if (cantidadTC01 >= 1 || cantidadTC04 >= 1)
                {

                    if (!Convert.ToBoolean(value))
                    {
                        return new ValidationResult("Esta opción es requerido");
                    }


                }
            }

            return ValidationResult.Success;
        }


        public static ValidationResult ValidarCodigoSunat(string value, System.ComponentModel.DataAnnotations.ValidationContext context)
        {



            var viewModel = context.ObjectInstance as SolicitarAccesoModel;

            if (viewModel.ListTipoEntidad2 != null || viewModel.ListTipoEntidad2.TiposEntidad != null)
            {
                int agenteAduanasCantidad = viewModel.ListTipoEntidad2.TiposEntidad.Where(x => x.CodTipoEntidad.Equals(Utilitario.Constante.EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS) && x.Check == true).Count();


                if (agenteAduanasCantidad >= 1)
                {

                    if (string.IsNullOrEmpty(value))
                    {
                        return new ValidationResult(Utilitario.Constante.SolicitudAccesoConstante.MensajeRegistroDatosEntidad.MENSAJE_CODIGO_SUNAT);
                    }


                }
            }

            return ValidationResult.Success;
        }
    }
}

