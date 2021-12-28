using Autofac;
using EasyNetQ;
using EasyNetQ.Topology;
using EventBus.Common.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EventBus.EasyNetQ
{
    public class EventBusEasyNetQ : Common.Abstractions.IEventBus, IDisposable
    {
        private IAdvancedBus advanceBus;
        private readonly ILogger<EventBusEasyNetQ> logger;
        private readonly ILifetimeScope autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "AFPnet_event_bus";        

        private IExchange exchange;
        private IQueue queue;
        private IBinding binding;
        private string routingKey;

        public EventBusEasyNetQ(string connectionString, ILogger<EventBusEasyNetQ> logger, ILifetimeScope autofac,
                                string exchangeName, string queueName, string routingKey)
        {
            advanceBus = RabbitHutch.CreateBus(connectionString,
                serviceRegister => serviceRegister.Register(serviceProvider => logger)).Advanced;
            this.logger = logger;
            this.autofac = autofac;

            #region Creacion de Exchange, Queue, Binding

            exchange = advanceBus.ExchangeDeclare(exchangeName, ExchangeType.Topic);

            queue = advanceBus.QueueDeclare(queueName);

            binding = advanceBus.Bind(exchange,queue,routingKey);

            this.routingKey = routingKey;

            #endregion

        }

        public void Dispose()
        {
            advanceBus.Dispose();
        }

        public void Publish<T>(T message) where T : class
        {
            var messageToQueue = new Message<T>(message);
            
            advanceBus.Publish(exchange, routingKey, true, messageToQueue);
        }

        public void SubscribeAsync<T, TH>(int maxDegreeOfParallelism)
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var lcts = new LimitedConcurrencyLevelTaskScheduler(maxDegreeOfParallelism);
            var factory = new TaskFactory(lcts);

            /*advanceBus.SubscribeAsync<T>(suscriptionId, msg => factory.StartNew(async () =>
            {
                await ProcessEventAsync<T>(msg);
            }), x => x.WithTopic(routingKeySubscriber));
            */

            advanceBus.Consume<T>(queue, (message, info) => ProcessEventAsync<T>(message.Body).Wait());
        }

        private async Task ProcessEventAsync<T>(IntegrationEvent msg) where T : IntegrationEvent
        {
            try
            {
                using (var scope = autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var handler = scope.ResolveOptional<IIntegrationEventHandler<T>>();
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));

                    await handler.Handle(msg as T);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            //no eliminamos ninguna suscripción
        }
    }
}
