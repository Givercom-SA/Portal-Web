using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class UsuarioVM
    {


        public string getNombre() {


            String nombreCompleto = this.Nombres + " "+ this.ApellidoPaterno +" "+ this.ApellidoMaterno;

            return nombreCompleto;
        }



        public string getCodigoUsuario()
        {
            String nombreCorto = "" +  this.IdUsuario ;


            return nombreCorto;
        }


        public string getNombreCorto()
        {
            String nombreCorto = this.Nombres + " "+ this.ApellidoPaterno +" "+ this.ApellidoMaterno;
            if (nombreCorto.Length > 26) {
                nombreCorto = nombreCorto.Substring(1, 26) + " ...";
            }

            return nombreCorto;
        }


        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public int IdEntidad { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool EsAdmin { get; set; }
        public bool Activo { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaModificacion { get; set; }
        public string PerfilNombre { get; set; }

        public string EntidadNroDocumneto { get; set; }
        public string EntidadTipoDocumento { get; set; }


        public string EntidadRazonSocial { get; set; }
        public string EntidadRepresentanteNombre { get; set; }

        public List<UsuarioMenuVM> Menus { get; set; }
    }
}
