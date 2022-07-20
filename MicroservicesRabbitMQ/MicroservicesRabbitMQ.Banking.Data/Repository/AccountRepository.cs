using MicroservicesRabbitMQ.Banking.Data.Context;
using MicroservicesRabbitMQ.Banking.Domain.Interfaces;
using MicroservicesRabbitMQ.Banking.Domain.Models;

namespace MicroservicesRabbitMQ.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _context;

        public AccountRepository(BankingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _context.Accounts;
        }
    }
}
