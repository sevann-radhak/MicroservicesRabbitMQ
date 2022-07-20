using MicroservicesRabbitMQ.Application.Interfaces;
using MicroservicesRabbitMQ.Application.Services;
using MicroservicesRabbitMQ.Banking.Data.Context;
using MicroservicesRabbitMQ.Banking.Data.Repository;
using MicroservicesRabbitMQ.Banking.Domain.Interfaces;
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

            services.AddTransient<BankingDbContext>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}