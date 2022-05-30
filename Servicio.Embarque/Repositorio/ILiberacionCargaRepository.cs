using Microsoft.AspNetCore.Mvc;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.Entidad;
using Servicio.Embarque.Models.LiberacionCarga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public interface ILiberacionCargaRepository
    {
        public CrearLiberacionCargaResult CrearLiberacionCarga(CrearLiberacionCargaParameter parameter);
        
    }
}
