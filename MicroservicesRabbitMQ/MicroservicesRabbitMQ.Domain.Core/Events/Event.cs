namespace MicroservicesRabbitMQ.Domain.Core.Events
{
    public abstract class Event
    {
        public Event()
        {
            Timestamp = DateTime.UtcNow;
        }

        public DateTime Timestamp { get; protected set; }
    }
}
