using AccesoDatos.Utils;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models.Vista
{
    public class ListarTodoVistaResult : BaseResult
    {
        public IEnumerable<VistaTodo> Vistas { get; set; }
        

     
    }

    public class VistaTodo { 
    
        public int IdVista { get; set; }
        public string Nombres { get; set; }
    }

}
