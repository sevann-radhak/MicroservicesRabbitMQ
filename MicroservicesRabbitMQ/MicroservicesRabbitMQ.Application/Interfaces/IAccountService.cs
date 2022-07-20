using MicroservicesRabbitMQ.Banking.Domain.Models;

namespace MicroservicesRabbitMQ.Application.Interfaces
{
    public interface IAccountService
    {
        public IEnumerable<Account> GetAccounts();
    }
}
