using MicroservicesRabbitMQ.Domain.Core.Events;

namespace MicroservicesRabbitMQ.Domain.Core.Commands
{
    public abstract class Command : Message
    {
        public Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp { get; protected set; }
    }
}
