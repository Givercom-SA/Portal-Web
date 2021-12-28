using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace Web.Principal.Areas.GestionarEmbarques.Models
{
    public class DireccionamientoSolicitud
    {
        public string KeyBL { get; set; }
        public string Modalidad { get; set; }

        [Display(Name = "RUC Almacén")]
        [RegularExpression("([0-9]+)", ErrorMessage = "El número de RUC debe contener sólo números")]
        [StringLength(11, ErrorMessage = "El número de RUC debe contener 11 digitos", MinimumLength = 11)]
        [Required(ErrorMessage = "Ingrese un RUC")]
        public string RUC { get; set; }
        [Display(Name = "Razón Social Almacén")]
        [Required(ErrorMessage = "Ingrese Razón social")]
        public string RazonSocial { get; set; }
        [Display(Name = "Código Almacén")]
        [Required(ErrorMessage = "Ingrese Código almacén")]
        public string CodigoAlmacen { get; set; }
        [Display(Name = "MSDS")]
        [Required(ErrorMessage = "Seleccione archivo")]
        public IFormFile FileMSDS { get; set; }
        [Display(Name = "Hoja de Seguridad")]
        [Required(ErrorMessage = "Seleccione archivo")]
        public IFormFile FileSeguridad { get; set; }
        [Display(Name = "LOI")]
        [Required(ErrorMessage = "Seleccione archivo")]
        public IFormFile FileLOI { get; set; }
        [Display(Name = "Letter of statement")]
        [Required(ErrorMessage = "Seleccione archivo")]
        public IFormFile FileLetter { get; set; }
        [Display(Name = "Carta garantía")]
        [Required(ErrorMessage = "Seleccione archivo")]
        public IFormFile FileCartaGarantia { get; set; }
        [Display(Name = "Carta de Responsabilidad Solidaria por Contenedores")]
        [Required(ErrorMessage = "Seleccione archivo")]
        public IFormFile FileCartaResponsabilidad { get; set; }
        public string FlagCargaPeligrosa { get; set; }
        public string FlagLOI { get; set; }
        public string FlagPlazoEta { get; set; }
        public string VenciemientoPlazo { get; set; }
        public string CodTipoConsignatario { get; set; }
        public SelectList ListModalidad { get; set; }
        public string CodigoTaf { get; set; }
        public string FlagDireccionamientoPermanente { get; set; }
        public string Almacen { get; set; }
        public string CantidadCtn { get; set; }
        public string NaveViaje { get; set; }
        public string Consignatario { get; set; }
        public string NroBL { get; set; }
    }
}
