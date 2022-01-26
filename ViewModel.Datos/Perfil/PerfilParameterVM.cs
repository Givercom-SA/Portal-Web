﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.Datos.Perfil
{
    public class PerfilParameterVM
    {
        public PerfilParameterVM() { }
        public int IdPerfil { get; set; }
        public string Nombre { get; set; }
        public string UsuarioCrea { get; set; }
        public int Activo { get; set; }
        public int IdSesion { get; set; }
        public int IdUsuarioCrea { get; set; }
        public int IdUsuarioModifica { get; set; }
        public string Tipo { get; set; }
        public int IdEntidad { get; set; }
        public List<int> Menus { get; set; }
    }

}
