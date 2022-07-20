using MicroservicesRabbitMQ.Banking.Domain.Models;

namespace MicroservicesRabbitMQ.Banking.Domain.Interfaces
{
    public interface IAccountRepository
    {
        public IEnumerable<Account> GetAccounts();
    }
}
