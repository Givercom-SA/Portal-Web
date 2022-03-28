using Microsoft.AspNetCore.Mvc;
using Servicio.Embarque.Models;
using Servicio.Embarque.Models.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public interface IEntidadRepository
    {
        public ListarEntidadResult ListarTipoEnidad(ListarEntidadParameter parameter);
        
    }
}
