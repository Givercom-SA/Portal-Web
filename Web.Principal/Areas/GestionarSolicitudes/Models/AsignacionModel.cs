using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Datos.Embarque.AsignarAgente;

namespace Web.Principal.Areas.GestionarSolicitudes.Models
{
    public class AsignacionModel
    {

        public AsignacionModel() {
            ListarAsignacionParameter = new AsignarAgenteListarParameterVM();

            listAsignarAgenteResult = new ListarAsignarAgenteResultVM();

        }


        public string TipoEntidad { get; set; }

        public AsignarAgenteListarParameterVM ListarAsignacionParameter { get; set; }
        public ListarAsignarAgenteResultVM listAsignarAgenteResult { get; set; }
    }
}
