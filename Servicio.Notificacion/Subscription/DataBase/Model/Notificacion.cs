﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Notificacion.Subscription.Model
{
    /// <summary>
    ///  Clase que se usa para obtener los mensajes desde el Service Broker de DB.
    ///  Atencion : Si se cambia la estructura tambien modificar la estructura de la clase Web.Principal.Entities.Notificacion.
    /// </summary>
    [Table("Paa_Ta_Notificacion_Test")]
    public class Notificacion
    {
        [Column(name: "In_Codigo_Usuario")]
        public int CodigoUsuario { get; set; }

        [Column(name: "Ch_Proceso")]
        public string Proceso { get; set; }

        [Column(name: "Vc_Mensaje")]
        public string Mensaje { get; set; }

        [Column(name: "Dt_Creacion_Fecha")]
        public DateTime CreacionFecha { get; set; }
    }
}