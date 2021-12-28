using Microsoft.AspNetCore.Mvc;
using Servicio.Embarque.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Servicio.Embarque.Repositorio
{
    public interface INotificacionArriboRepository
    {
        public string RegistrarNotificacionArribo(RegistrarNotificacionArriboParameter parameter);
        public ListarNotificacionesPendientesResult ListaNotificacionesArriboPendientes();
        public void ActualizarEstadoNotificacion(string _keyBld, int _idUsuario, string tipoDoc);
        public ListaExpressReleaseAceptadasResult ListaExpressReleaseAceptadas();
        public string RegistrarExpressReleaseAceptadas(string keyBld, string nroBl, int idUsuario);
    }
}
