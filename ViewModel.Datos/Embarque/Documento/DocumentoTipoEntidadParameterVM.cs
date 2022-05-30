using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Datos.Entidad;

namespace ViewModel.Datos.Documento
{
    public class ListDocumentoTipoEntidadParameterVM
    {
        public bool BrindaCargaFCL { get; set; }

        public bool AcuerdoSeguridadCadenaSuministro { get; set; }
        public bool SeBrindaAgenciamientodeAduanas { get; set; }

        public List<TipoEntidadVM> TiposEntidad { get; set; }
    }

   
}
