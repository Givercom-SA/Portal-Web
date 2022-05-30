using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.LiberacionCarga
{
    public class CrearLiberacionCargaParameterVM
    {
        public string KeyBLD { get; set; }
        public string NroBL { get; set; }
        public string Origen { get; set; }
        public string Servicio { get; set; }
        public int IdUsuarioCrea { get; set; }
        public string IdEmpresaGtrm { get; set; }
        public int IdSesion { get; set; }
        public List<LiberacionCargaDetalleVM> Detalles { get; set; }


    }
    public class LiberacionCargaDetalleVM
    {
        public string KeyBl { get; set; }
        public string NroBl { get; set; }
        public string Consignatario { get; set; }

    }


}
