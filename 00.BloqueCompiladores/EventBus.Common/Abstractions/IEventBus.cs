namespace EventBus.Common.Abstractions
{
    public interface IEventBus
    {
        //Se ha quitado el SubscriptionId para realizar configuraciones mas avanzadas.
        void SubscribeAsync<T, TH>(int maxDegreeOfParallelism) 
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        void Publish<T>(T message) where T : class;
    }
}
