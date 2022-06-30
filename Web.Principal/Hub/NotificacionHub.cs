using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Security.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilitario.Constante;
using ViewModel.Notificacion;
using Web.Principal.Entities;
using Web.Principal.ServiceConsumer;

namespace Web.Principal.Hub
{
    public class NotificacionHub : Hub<INotificacionHub>
    {
        private ILogger<NotificacionHub> _logger;
        private readonly ServicioNotificacion _notificacionService;
        private IMemoryCache _memoryCache;

        public NotificacionHub(
            ILogger<NotificacionHub> logger,
            ServicioNotificacion notificacionService,
            IMemoryCache memoryCache
        ) : base()
        {
            _logger = logger;
            _notificacionService = notificacionService;
            _memoryCache = memoryCache;
        }

        public static List<Tuple<int, string>> Usuario { get; set; } = new List<Tuple<int, string>>();

        public async Task<List<Notificacion>> Matricula(string codigoUsuarioEncriptado)
        {

            var notificacionesAEnviar = new List<Notificacion>();

            try
            {
                var codigoUsuario = Int32.Parse(Encriptador.Instance.DesencriptarTexto(codigoUsuarioEncriptado));

                //_logger.LogInformation("Matricula del usuario:" + codigoUsuario);

                var t = Tuple.Create(codigoUsuario, Context.ConnectionId);
                Usuario.Add(t);

                var notificaciones = _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones);

                var notificacionesPorUsuario = new List<NotificacionVM>();

                notificaciones.TryGetValue(codigoUsuario, out notificacionesPorUsuario);

                //Por Seguridad no se envia el codigo de usuario, ademas no es necesario
                if (notificacionesPorUsuario != null)
                    notificacionesAEnviar = notificacionesPorUsuario.Select(x => new Notificacion { CodigoUsuario = 0, CreacionFecha = x.Fecha, Mensaje = x.Mensaje, Proceso = x.Proceso }).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error en Matricula");
            }

            return notificacionesAEnviar;
        }

        public async Task LimpiarNotificaciones(string codigoUsuarioEncriptado)
        {
            try
            {
                var codigoUsuario = Int32.Parse(Encriptador.Instance.DesencriptarTexto(codigoUsuarioEncriptado));
                await _notificacionService.LimpiarNotificacionesPorUsuario(codigoUsuario);

                _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[codigoUsuario] = new List<NotificacionVM>();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al LimpiarNotificaciones");
            }

        }

        public async Task RecibirNotificacionDB(Notificacion notificacion)
        {
            _logger.LogInformation("Se recibio de DB el mensaje :" + notificacion);

            try
            {
                //Grabar en cache
                _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[notificacion.CodigoUsuario]
                    .Add(new NotificacionVM
                    {
                        Proceso = notificacion.Proceso,
                        Mensaje = notificacion.Mensaje,
                        Fecha = notificacion.CreacionFecha
                    });

                foreach (var t in Usuario)
                {
                    if (t.Item1 == notificacion.CodigoUsuario)
                    {
                        //Si es necesario realizar otra accion para un proceso diferente agregar un else if.
                        await Clients.Client(t.Item2).RecibirNotificacion(notificacion);
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                _memoryCache.Get<Dictionary<int, List<NotificacionVM>>>(SistemaConstante.Cache.Notificaciones)[notificacion.CodigoUsuario] = new List<NotificacionVM>();
                _logger.LogWarning("No se pudo obtener la informacion de Notificaciones desde Cache, se creará la información para el usuario : " + notificacion.CodigoUsuario);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Sucedio un error al RecibirNotificacionDB");
            }

        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            int conexionesEliminadas = 0;

            try
            {
                conexionesEliminadas = Usuario.RemoveAll(x => {
                    return x.Item2.Equals(Context.ConnectionId);
                });

            }
            catch (Exception e1)
            {
                _logger.LogError(e1, "Sucedio un error en OnDisconnectedAsync");
            }

            await base.OnDisconnectedAsync(e);
        }
    }
}