using MicroservicesRabbitMQ.Domain.Core.Events;

namespace MicroservicesRabbitMQ.Domain.Core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
    {
        public Task Handler(TEvent @event);
    }

    public interface IEventHandler { }
}