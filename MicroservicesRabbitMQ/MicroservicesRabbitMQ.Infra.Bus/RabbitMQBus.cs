using MediatR;
using MicroservicesRabbitMQ.Domain.Core.Bus;
using MicroservicesRabbitMQ.Domain.Core.Commands;
using MicroservicesRabbitMQ.Domain.Core.Events;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MicroservicesRabbitMQ.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly RabbitMQSettings _settings;

        public RabbitMQBus(IMediator mediator, IOptions<RabbitMQSettings> settings)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            _settings = settings.Value;
        }

        public void Publish<T>(T @event) where T : Event
        {
            ConnectionFactory factory = GetConnectionFactory();

            using IConnection? connection = factory.CreateConnection();
            using IModel? channel = connection.CreateModel();
            string? eventName = @event.GetType().Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            string? message = JsonConvert.SerializeObject(@event);
            byte[]? body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", eventName, null, body);
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            string? eventName = typeof(T).Name;
            Type? handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException($"Handler exception {handlerType.Name} have already been registered by '{eventName}'.", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            StartBasicConsume<T>();
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            string? eventName = e.RoutingKey;
            string? message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEventAsync(eventName, message).ConfigureAwait(false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private ConnectionFactory GetConnectionFactory()
        {
            return new()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };
        }

        private async Task ProcessEventAsync(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                List<Type>? subscriptions = _handlers[eventName];

                foreach (Type? subscription in subscriptions)
                {
                    object? handler = Activator.CreateInstance(subscription);
                    if (handler == null)
                    {
                        continue;
                    }

                    Type? eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    object? @event = JsonConvert.DeserializeObject(message, eventType);
                    Type? concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);

                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                }
            }
        }

        private void StartBasicConsume<T>() where T : Event
        {
            ConnectionFactory factory = GetConnectionFactory();

            IConnection? connection = factory.CreateConnection();
            IModel? channel = connection.CreateModel();

            string? eventName = typeof(T).Name;
            channel.QueueDeclare(eventName, false, false, false, null);
            AsyncEventingBasicConsumer? consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ConsumerReceived;
            channel.BasicConsume(eventName, true, consumer);
        }
    }
}