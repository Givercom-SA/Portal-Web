﻿using AccesoDatos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Embarque.Usuario
{
    public class ListarUsuarioResultVM: BaseResult
    {
        public List<UsuarioVM> Usuarios { get; set; }
    }

    public class UsuarioVM
    {
        public int IdUsuario { get; set; }
        public int IdEntidad { get; set; }
        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool EsAdmin { get; set; }
        public bool Activo { get; set; }
        public string TipoPerfil { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaModificacion { get; set; }

    }
}
