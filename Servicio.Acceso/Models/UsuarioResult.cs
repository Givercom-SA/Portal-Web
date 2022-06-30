using AccesoDatos.Utils;
using Servicio.Acceso.Models.Perfil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Acceso.Models
{
    public class UsuarioResult : BaseResult
    {
        public int ENTI_ID { get; set; }
        public string ENTI_TIPO_DOCUMENTO { get; set; }
        public string ENTI_NUMERO_DOCUMENTO { get; set; }
        public string ENTI_RAZON_SOCIAL { get; set; }
        public string PERFIL_TIPO_ENTIDAD { get; set; }
        public int USU_ID { get; set; }
        public string USUA_NOMBRES { get; set; }
        public string USUA_APELLIDO_PATERNO { get; set; }
        public string USUA_APELLIDO_MATERNO { get; set; }
        public string USU_CORREO { get; set; }
        public string PEFL_DASHBOARD { get; set; }
        public string PEFL_TIPO { get; set; }
        public int PEFL_ID { get; set; }
        public string ModoAdminSistema { get; set; }
        public string PEFL_NOMBRE { get; set; }
        public long IdUsuarioInicioSesion { get; set; }
        public int USUA_CAMBIOCLAVE { get; set; }
        public int EsAdmin { get; set; }
        public int AdminSistema { get; set; }
        public List<MenuLogin> Menus { get; set; }
        public List<MenuLogin> MenusLogin { get; set; }
        public List<PerfilLogin> Perfiles { get; set; }
        public List<string> DocumentosRevisar { get; set; }
        public List<ItemAcceso> ListaAcceso { get; set; }
  
    }

    public class PerfilLogin
    {
        
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string Dashboard { get; set; }
        public string TipoPerfil { get; set; }
    }

    public class MenuLogin
    {
        public int IdPerfil { get; set; }
        public int IdMenu { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string Area { get; set; }
        public string Nombre { get; set; }
        public string Grupo { get; set; }
        public bool Permiso { get; set; }
        public string TipoMenu { get; set; }
        public int Orden { get; set; }
        public bool Visible { get; set; }
        public int IdPadre { get; set; }
        public List<VistaMenu> VistaMenus { get; set; }

    }

    public class ItemAcceso
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
        public int IdVistaMenuPerfil { get; set; }
        
        public string VistaNombre { get; set; }
        
            
    }
}

