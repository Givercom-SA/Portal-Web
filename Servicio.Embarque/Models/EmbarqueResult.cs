using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Models
{
    public class EmbarqueResult
    {

        public string KEYBLD { get; set; }

        
        public string NROOT { get; set; }

        
        public string NROBL { get; set; }

        
        public string NRORO { get; set; }
        
        public string EMPRESA { get; set; }
        
        public string ORIGEN { get; set; }



        
        public string DES_CONDICION { get; set; }

        
        public string CONDICION { get; set; }
        
        public string POL { get; set; }
        
        public string POD { get; set; }
        
        public string ETAPOD { get; set; }
        
        public string EQUIPAMIENTO { get; set; }
        
        public string MANIFIESTO { get; set; }
        
        public string COD_LINEA { get; set; }
        
        public string DES_LINEA { get; set; }
        
        public string COD_TIPO_CONSIGNATARIO { get; set; }
        
        public string CONSIGNATARIO { get; set; }
        
        public string COD_INSTRUCCION { get; set; }
        
        public string DES_INSTRUCCION { get; set; }
        
        public string FLAG_LINEA_MEMO { get; set; }


        
        public string FLAG_MEMO_VIGENTE { get; set; }
        
        public string FLAG_COBROS_FACTURADOS { get; set; }
        
        public string FLAG_PLAZO_ETA { get; set; }
        
        public string FLAG_CARGA_PELIGROSA { get; set; }
        
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
    }
}
