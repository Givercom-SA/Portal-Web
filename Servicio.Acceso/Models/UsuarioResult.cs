using AccesoDatos.Utils;
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
        public List<MenuLogin> MenusUserSecundario { get; set; }
        public List<PerfilLogin> Perfiles { get; set; }

        public List<string> DocumentosRevisar { get; set; }
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

    }
}

