using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitario.Constante;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class UsuarioRegistroVM : BaseResultVM
    {
        public int IdEntidad { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }

        public string TipoEntidad { get; set; }

        public int idUsuario { get; set; }
        public string NombresUsuario { get; set; }
        public string ApellidoPaternousuario { get; set; }
        public string ApellidoMaternoUsuario { get; set; }
        public string CorreoUsuario { get; set; }

        public string obtenerNombreCompleto() {



            return $"{this.NombresUsuario} {this.ApellidoPaternousuario} {this.ApellidoMaternoUsuario}";
        }

        public int IdPerfil { get; set; }
        public string PerfilNombre { get; set; }
        public string TipoPerfil { get; set; }
        public string Dashboard { get; set; }

        public bool isCambioClave { get; set; }
        public List<MenuLoginVM> Menus { get; set; }
        public List<PerfilLoginVM> Perfiles { get; set; }
        

        public SesionUsuarioVM Sesion { get; set; }

        public List<string> DocumentosRevisar { get; set; }

        public string obtenerTipoEntidadTransmares() {


            string strTipoEntidadTransmares = "";

            if (this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS)) {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.AGENTE_ADUANAS.ToString();
            }
            else if (this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FINAL))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FINAL.ToString();
            }
            else if(this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.BROKER)) {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.BROKET.ToString();
            }
            else if (this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FORWARDER.ToString();
            }

            return strTipoEntidadTransmares;


        
        }

        public string obtenerTipoEntidadTransmares(string TipoEntidad)
        {


            string strTipoEntidadTransmares = "";

            if (TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.AGENTE_ADUANAS.ToString();
            }
            else if (TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FINAL))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FINAL.ToString();
            }
            else if (TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.BROKER))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.BROKET.ToString();
            }
            else if (TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FORWARDER))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FORWARDER.ToString();
            }

            return strTipoEntidadTransmares;



        }

    }


    public class SesionUsuarioVM { 
    

        public string CodigoSesion { get; set; }
        public string RucIngresadoUsuario { get; set; }
        public string CodigoTransGroupEmpresaSeleccionado { get; set; }
        public string RucTransGroupEmpresaSeleccionado { get; set; }
        public string NombreTransGroupEmpresaSeleccionado { get; set; }
        public string ImagenTransGroupEmpresaSeleccionado { get; set; }

        public DateTime FechaInicioSesion { get; set; }


    }


    public class PerfilLoginVM
    {
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string Dashboard{ get; set; }
    }
    public class MenuLoginVM
    {
        public int IdPerfil { get; set; }
        public int IdMenu { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Area { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }
        public string HttpMethod { get; set; }

        
    }
}
