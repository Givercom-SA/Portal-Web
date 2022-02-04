using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Principal.Model
{
    public class RegistroSolicitudRequestModel
    {
        
           public string pIdSolicitudPW{get;set;}
        public string pFechaSolicitud { get; set; }
        public string pFechaEvaluacion { get; set; }
        
        public string pNroSolicitudPW { get; set; }
        public string pUsuarioPW{get;set;} 
            public string pEmpresa{get;set;} 
            public string pKeybld{get;set;}
            public string pTipoCliente{get;set;}
            public string pRucCliente{get;set;} 
            public string pTipoPago{get;set;}
            public string pFormaPago{get;set;}
            public string pId_Transaccion{get;set;} 
            public string pNumeroOperacion{get;set;}
        public string pUsuarioFinPW { get; set; }
        
            public string pFechaTransferencia {get;set;} 
            public double pImporte { get; set; }


    }
}
