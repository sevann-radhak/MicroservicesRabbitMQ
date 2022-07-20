using MicroservicesRabbitMQ.Domain.Core.Commands;
using MicroservicesRabbitMQ.Domain.Core.Events;

namespace MicroservicesRabbitMQ.Domain.Core.Bus
{
    public interface IEventBus
    {
        public Task SendCommand<T>(T command) where T : Command;
        public void Publish<T>(T @event) where T : Event;
        public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
    }
}
