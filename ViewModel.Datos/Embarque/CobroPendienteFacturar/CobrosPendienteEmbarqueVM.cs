using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Common.Request;
using ViewModel.Datos.Embarque.SolicitudFacturacionTercero;

namespace ViewModel.Datos.Embarque.CobroPendienteFacturar
{
    public class CobrosPendienteEmbarqueVM
    {
        public string ID { get; set; }
        public string RubroCodigo { get; set; }
        public string ConceptoCodigo { get; set; }
        public string Descripcion { get; set; }
        public string ConceptoCodigoDescripcion { get; set; }
        public string Moneda { get; set; }
        public string Importe { get; set; }
        public string Igv { get; set; }
        public string Total { get; set; }
        public string FlagAsignacion { get; set; }
        public bool Check { get; set; }


        public string Desglose { get; set; }

        public string NroBl { get; set; }
        public string keyBl { get; set; }
        public string BlPagar { get; set; }
        public string IdBlPagar { get; set; }
 


    }

    public class ListCobrosPendienteEmbarqueVM : DataRequestViewModelResponse
    {

        public string KEYBL { get; set; }
        public string BL { get; set; }

        [Display(Name = "BLs")]
        public string BlNietos { get; set; }

        public SolicitarFacturacionTerceroParameterVM SolicitarFacturacionTercero { get; set; }

        public List<CobrosPendienteEmbarqueVM> CobrosPendientesEmbarque { get; set; }

        public IList<DesgloseModel> listaDesglose { get; set; }

        public int existeDesglosePendiente { get; set; }

        public string EstaAsginadoAgenteAduanas { get; set; }

        public Tenor Tenor { get; set; }

        public string Servicio { get; set; }
        public string Origen { get; set; }

        public string ParKey { get; set; }



    }

    public class Tenor {

        public string RepresentanteLegalEmpresaLogeada { get; set; }
        public string RazonSocialEmpresaLogeada { get; set; }
        public string RucEmpresaLogeada { get; set; }
        public string NroEmbarque { get; set; }
  

        
    }

    public class DesgloseModel
    {
        public string KEYBLD { get; set; }
        public bool Check { get; set; }
        public string NROBL { get; set; }
        public string CONSIGNATARIO { get; set; }
        public int FLAG_AUTORIZADO { get; set; }
            
    }



}
