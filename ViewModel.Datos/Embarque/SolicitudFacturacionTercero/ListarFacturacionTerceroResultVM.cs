using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.CobroPendienteFacturar;

namespace ViewModel.Datos.Embarque.SolicitudFacturacionTercero
{
    public class ListarFacturacionTerceroResultVM : BaseResultVM
    {


        public List<SolicitudFacturacionTercerosVM> SolicituresFacturacionTereceros { get; set; }


    }


    public class SolicitudFacturacionTercerosVM {
        public string Codigo { get; set; }
        public string IdFacturacionTercero { get; set; }
        public string IdEntidad { get; set; }
        public string IdUsuario { get; set; }

        public string CodigoCliente { get; set; }
        public string ClienteNombres { get; set; }
        public string ClienteNroDocumeto { get; set; }
        public string IdUsuarioCrea { get; set; }
        public string Archivo { get; set; }
        public string EmbarqueKeyBL { get; set; }
        public string EmbarqueNroBL { get; set; }

        public string Usuario { get; set; }
        public string EntidadDatos { get; set; }
        public DateTime FechaRegistro { get; set; }
        
        public string Estado { get; set; }
        public string EstadoNombre { get; set; }
        public string Correo { get; set; }

        public List<CobrosPendienteEmbarqueVM> CobrosPendientesEmbarque { get; set; }
    }


}
