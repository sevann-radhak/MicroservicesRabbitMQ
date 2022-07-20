using MicroservicesRabbitMQ.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesRabbitMQ.Banking.Data.Context
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
    }
}
