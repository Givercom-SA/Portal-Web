using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.SolictudAcceso
{
    [Serializable]
    public class SolicitarAccesoParameterVM 
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string RepresentaLegalNombre { get; set; }
        public string RepresentaLegalApellidoPaterno { get; set; }
        public string RepresentaLegalMaterno { get; set; }
        public string Correo { get; set; }
        public bool AcuerdoEndoceElectronico { get; set; }
        public bool BrindaOpeCargaFCL { get; set; }
        public bool AcuerdoSeguroCadenaSuministro { get; set; }
        public bool DeclaracionJuradaVeracidadInfo { get; set; }

        public bool? ProcesoFacturacion { get; set; }
        public bool? TerminoCondicionGeneralContraTCGC { get; set; }
        public string CodigoSunat { get; set; }
        public int IdEntidad { get; set; }
        public bool BrindaAgenciamientoAduanas { get; set; }
        public string ImagenGrupoTrans { get; set; }
        public string TipoRegistro { get; set; }
        public int IdUsuarioCreaModifica { get; set; }
        public List<TipoEntidadVM> TipoEntidad { get; set; }
        public List<DocumentoVM> Documento { get; set; }
    }

    [Serializable]
    public class TipoEntidadVM
    {
        public string CodigoTipoEntidad { get; set; }
    }

    [Serializable]
    public class DocumentoVM
    {
        public string CodigoDocumento { get; set; }
        public string NombreArchivo { get; set; }

        public string UrlArchivo { get; set; }
    }
}
