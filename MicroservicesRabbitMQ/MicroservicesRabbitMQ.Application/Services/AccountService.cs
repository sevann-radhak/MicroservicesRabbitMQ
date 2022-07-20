using MicroservicesRabbitMQ.Application.Interfaces;
using MicroservicesRabbitMQ.Banking.Domain.Interfaces;
using MicroservicesRabbitMQ.Banking.Domain.Models;

namespace MicroservicesRabbitMQ.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _repository.GetAccounts();
        }
    }
}
