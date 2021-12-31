using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.LibroReclamaciones.Utilitario
{

    public class ActionResponse
    {


        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public string Titulo { get; set; }
        public List<ActionErrorResponse> ListActionListResponse  { get; set; }
        public object Objeto { get; set; }
    }
}
