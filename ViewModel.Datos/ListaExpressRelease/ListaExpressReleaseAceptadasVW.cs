using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.ListaExpressRelease
{
    public class ListaExpressReleaseAceptadasVW : BaseResultVM
    {
        public List<ExpressReleaseAceptada> listaExpressRelease { get; set; }
    }

    public class ExpressReleaseAceptada
    {
        public string KeyBl { get; set; }
        public string NroBl { get; set; }
        public int IdUsuario { get; set; }
    }
}
