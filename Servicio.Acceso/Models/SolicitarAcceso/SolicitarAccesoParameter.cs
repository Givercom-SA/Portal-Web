using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.SolicitarAcceso
{
    public class SolicitarAccesoParameter
    {
        public string SOLI_TIPODOCUMENTO { get; set; }
        public string SOLI_NUMERO_DOCUMENTO { get; set; }
        public string SOLI_RAZON_SOCIAL { get; set; }
        public string SOLI_RELEGAL_NOMBRE { get; set; }
        public string SOLI_RLEGAL_APELLIDO_PATERNO { get; set; }
        public string SOLI_RLEGAL_APELLIDO_MATERNO { get; set; }
        public string SOLI_CORREO { get; set; }
        public bool SOLI_ACUERDO_ENDOCE_ELECTRONICO { get; set; }
        public bool SOLI_BRINDAOPE_CARGA_FCL { get; set; }
        public bool SOLI_BRINDA_AGENCIAMIENTO_ADUANA { get; set; }
        public bool SOLI_ACUERDO_SEGUR_CADENA_SUMINI { get; set; }
        public bool SOLI_DECLARA_JURADA_VERACIDAD_INFO { get; set; }
        public bool? ProcesoFacturacion { get; set; }
        public bool? TerminoCondicionGeneralContraTCGC { get; set; }
        public string CodigoSunat { get; set; }
        public List<TipoEntidad> TipoEntidad { get; set; }
        public List<Documento> Documento { get; set; }
        public string TipoRegistro { get; set; }
        public int IdUsuarioCreaModifica { get; set; }
        public int IdEntidad { get; set; }
        

    }


    public class TipoEntidad {
        public string CodigoTipoEntidad { get; set; }
    }

    public class Documento { 
    
        public string CodigoDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; }
        

    }

}
