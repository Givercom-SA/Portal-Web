using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Solicitud.Models
{
    public class ListarSolicitudesResult : BaseResult
    {
        public IEnumerable<ObjetoSolicitudResult> ListaSolicitudes { get; set; }
    }

    public class ObjetoSolicitudResult : BaseResult
    {
        public string SOLI_CODIGO { get; set; }
        public DateTime SOLI_FECHA_REGISTRO { get; set; }
        public string SOLI_RAZON_SOCIAL { get; set; }
        public string SOLI_ESTADO_CODIGO { get; set; }
        public string SOLI_ESTADO_NOMBRE { get; set; }

        public string SOLI_CORREO { get; set; }

        public string SOLI_TIPODOCUMENTO { get; set; }
        public string SOLI_NUMERO_DOCUMENTO { get; set; }
        public string SOLI_RELEGAL_NOMBRE { get; set; }
        public string SOLI_RLEGAL_APELLIDO_PATERNO { get; set; }
        public string SOLI_RLEGAL_APELLIDO_MATERNO { get; set; }

        public int SOLI_ACUERDO_SEGUR_CADENA_SUMINI { get; set; }

        public bool? TerminoCondicionGeneralContracion { get; set; }
        public bool? ProcesoFacturacion { get; set; }
        public bool? DeclaracionJuaradVerecidad { get; set; }
        public bool? BrindaCargaFCL { get; set; }
        public bool? AcuerdoEndoceElectronico { get; set; }
        public bool? BrindaAgenciamientoAduanas { get; set; }

        public DateTime? FechaEvaluacion { get; set; }
        public int? IdUsuarioEvaluacion { get; set; }
        public string MotivoRechazo { get; set; }

        public string CodigoSunat { get; set; }
        public string UsuarioEvalua { get; set; }
        

        public IEnumerable<ObjetoDocumentoResult> ListaDocumentos { get; set; }

        public IEnumerable<ObjetoEventosResult> ListaEventos { get; set; }

        public IEnumerable<ObjetoEntidadesResult> ListaTipoEntidad { get; set; }
    }

    public class ListarDocumentoResult : BaseResult
    {
        public IEnumerable<ObjetoDocumentoResult> ListaDocumentos { get; set; }
    }

    public class ObjetoDocumentoResult
    {
        public string SADO_CODDOCUMENTO { get; set; }
        public string SADO_NOMDOCUMENTO { get; set; }
        public string SADO_URLDOCUMENTO { get; set; }
        public string SADO_ESTADO { get; set; }
        public string SADO_COD_MOTIVO_RECHAZO { get; set; }
        public string SADO_NOMMOTIVORECHAZO { get; set; }
    }
}
