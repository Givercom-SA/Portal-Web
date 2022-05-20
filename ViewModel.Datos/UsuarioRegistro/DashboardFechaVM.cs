using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class DashboardFechaVM
    {


        public string Anio { get; set; }
        public string Mes { get; set; }
        public string Proceso { get; set; }
        public string Cantidad { get; set; }
        public string FechaTexto { get {

                return new Utilitario.Utilitario().MesXNumero(this.Mes);

            } }

    }
    
}
