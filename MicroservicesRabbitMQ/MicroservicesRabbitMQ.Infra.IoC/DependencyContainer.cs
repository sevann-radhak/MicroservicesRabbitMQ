using MicroservicesRabbitMQ.Domain.Core.Bus;
using MicroservicesRabbitMQ.Infra.Bus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesRabbitMQ.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEventBus, RabbitMQBus>();
            services.Configure<RabbitMQSettings>(c => configuration.GetSection("RabbitMQSettings"));
        }
    }
}