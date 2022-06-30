using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Datos.Acceso;
using ViewModel.Datos.Perfil;

namespace ViewModel.Datos.UsuarioRegistro
{
    public class UsuarioRegistroVM : BaseResultVM
    {

        public int IdEntidad { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string RazonSocial { get; set; }
        public string TipoEntidad { get; set; }
        public string BearerToken { get; set; }
        public int idUsuario { get; set;}
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
        public int AdminSistema { get; set; }

        public string ModoAdminSistema { get; set; }

        public bool isCambioClave { get; set; }
        public List<MenuLoginVM> Menus { get; set; }
        public List<PerfilLoginVM> Perfiles { get; set; }
        public ListarTransGroupEmpresaVM Empresas { get; set; }
        public List<MenuLoginVM> MenusLogin { get; set; }
        public SesionUsuarioVM Sesion { get; set; }
        public List<MenuElementoVM> Accesos { get; set; }
  
        public List<string> DocumentosRevisar { get; set; }
        public int EsAdmin { get; set; }
        public string obtenerTipoEntidadTransmares() {


            string strTipoEntidadTransmares = "";

            if (this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.AGENTE_ADUANAS)) {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.AGENTE_ADUANAS.ToString();
            }
            else if (this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.CLIENTE_FINAL))
            {
                strTipoEntidadTransmares = EmbarqueConstante.TipoEntidadTransmares.CLIENTE_FINAL.ToString();
            }
            else if (this.TipoEntidad.Equals(EmbarqueConstante.TipoEntidad.BROKER)) {
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

        public long IdUsuarioInicioSesion { get; set; }

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
        public string Dashboard { get; set; }
    }
    public class MenuLoginVM
    {

        public string _controller;
        public string _area;
        public string _action;
        public string _grupo;
        public string _metodo;

        private string tipoPerfil;
        public int IdPerfil { get; set; }
        public int IdMenu { get; set; }
        public string Controlador { get {
                if (this._controller == null) {
                    return "";
                }
                else {
                    return this._controller; }
            } set { this._controller = value; } }
        public string Accion
        {
            get
            {
                if (this._action == null)
                {
                    return "";
                }
                else
                {
                    return this._action;
                }
            }
            set { this._action = value; }
        }
        public string Area
        {
            get
            {
                if (this._area == null)
                {
                    return "";
                }
                else
                {
                    return this._area;
                }
            }
            set { this._area = value; }
        }
        public string Nombre { get; set; }
        public string Grupo
        {
            get
            {
                if (this._grupo == null)
                {
                    return "";
                }
                else
                {
                    return this._grupo;
                }
            }
            set { this._grupo = value; }
        }
        public string HttpMethod
        {
            get
            {
                if (this._metodo == null)
                {
                    return "";
                }
                else
                {
                    return this._metodo;
                }
            }
            set { this._metodo = value; }
        }
        public bool Permiso { get; set; }
        public string TipoMenu { get {
                if (tipoPerfil == null)
                    return "";
                        else
                    return tipoPerfil;
            }
            set { tipoPerfil = value; } 
        }

        public int Orden { get; set; }
        public bool Visible { get; set; }
        public int IdPadre { get; set; }

        public List<VistaMenuVM> VistaMenus { get; set; }

    }
    public class MenuElementoVM
    {
        public int UserId { get; set; }
        public long IdVistaMenu { get; set; }
        public long IdVista { get; set; }
        public long MenuId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public int ParentId { get; set; }
        public bool HasChildren { get; set; }
        public int Type { get; set; }
        public int Level { get; set; }
        public int Order { get; set; }
        public string Path { get; set; }

        public string NameControlHtml { get; set; }
        public string ItemType { get; set; }

        public string HttpArea { get; set; }
        public string HttpController { get; set; }
        public string HttpAction { get; set; }
        public string HttpMethod { get; set; }
        public int IsMainForm { get; set; }
        public int VistaOpcion { get; set; }
        public int IdPerfil { get; set; }
        
        public int SortingId { get; set; }
        public int IdVistaMenuPerfil { get; set; }

        public string VistaNombre { get; set; }
    }
}
