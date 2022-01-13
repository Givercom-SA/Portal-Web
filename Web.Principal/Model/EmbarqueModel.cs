using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Common.Request;

namespace Web.Principal.Model
{
    public class EmbarqueModel
    {
        public string KEYBLD { get; set; }

        [Display(Name = "Nro. Root")]
        public string NROOT { get; set; }

        [Display(Name = "Nro. BL")]
        public string NROBL { get; set; }

        [Display(Name = "Nro. RO")]
        public string NRORO { get; set; }
        [Display(Name = "Empresa")]
        public string EMPRESA { get; set; }
        [Display(Name = "Origen")]
        public string ORIGEN { get; set; }



        [Display(Name = "Descipción de Condición")]
        public string DES_CONDICION { get; set; }

        [Display(Name = "Condición")]
        public string CONDICION { get; set; }
        [Display(Name = "Pol")]
        public string POL { get; set; }
        [Display(Name = "Pod")]
        public string POD { get; set; }
        [Display(Name = "Etapa Pod")]
        public string ETAPOD { get; set; }
        [Display(Name = "Equipamiento")]
        public string EQUIPAMIENTO { get; set; }
        [Display(Name = "Manifiesto")]
        public string MANIFIESTO { get; set; }
        [Display(Name = "Cod Linea")]
        public string COD_LINEA { get; set; }
        [Display(Name = "Descripción Linea")]
        public string DES_LINEA { get; set; }
        [Display(Name = "Tipo consignatorio")]
        public string COD_TIPO_CONSIGNATARIO { get; set; }
        [Display(Name = "Consignatorio")]
        public string CONSIGNATARIO { get; set; }
        [Display(Name = "Cod. Instrucción")]
        public string COD_INSTRUCCION { get; set; }
        [Display(Name = "Descripción Instrucción")]
        public string DES_INSTRUCCION { get; set; }
        [Display(Name = "Linea meno")]
        public string FLAG_LINEA_MEMO { get; set; }


        [Display(Name = "Memo vigente")]
        public string FLAG_MEMO_VIGENTE { get; set; }
        [Display(Name = "Cobros facturados")]
        public string FLAG_COBROS_FACTURADOS { get; set; }
        [Display(Name = "Cumple Pazo")]
        public string FLAG_PLAZO_ETA { get; set; }
        [Display(Name = "Carga peligrosa")]
        public string FLAG_CARGA_PELIGROSA { get; set; }
        [Display(Name = "Linea naviera")]
        public string FLAG_LOI { get; set; }
        public string VENCIMIENTO_PLAZO { get; set; }
        public string FLAG_DIRECCIONAMIENTO_PERMANENTE { get; set; }
        public string CODIGO_TAF_DEPOSITO_PERMANENTE { get; set; }
        public string RAZON_SOCIAL_DEPOSITO_PERMANENTE { get; set; }
        public string ALMACEN { get; set; }
        public string CANTIDAD_CNT { get; set; }
        public string NAVEVIAJE { get; set; }

        public string OPERADOR_MAIL { get; set; }
        public string FLAG_DESGLOSE { get; set; }
        public string CANTIDAD_DESGLOSE { get; set; }
        public string TIPO_PADRE { get; set; }
        public string FLAG_CONFIRMACION_AA { get; set; }
        public string FLAG_PLAZO_TERMINO_DESCARGA { get; set; }
        public string FLAG_EXONERADO_COBRO_GARANTIA { get; set; }
        public string PLAZO_TERMINO_DESCARGA { get; set; }
        public string FLAG_DIRECCIONAMIENTO_PERMANENTE_BL { get; set; }
        public string FINANZAS_MAIL { get; set; }

        public string FEC_CREATE_RO { get; set; }
        public string FEC_ETD { get; set; }
        public string FEC_ETA { get; set; }
        public string FEC_TRANSMISION_ADUANAS { get; set; }
        public string FEC_ING_CARGA { get; set; }
        public string FEC_RET_CARGA { get; set; }


    }

    public class ListaEmbarqueModel : DataRequestViewModelResponse
    {
        public IList<EmbarqueModel> listaEmbarques { get; set; }
    }
}
