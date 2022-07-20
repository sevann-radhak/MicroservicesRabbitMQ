namespace MicroservicesRabbitMQ.Banking.Domain.Models
{
    public class Account
    {
        public int Id { get; set; }
        public decimal AccountBalance { get; set; }
        public string AccountType { get; set; } = string.Empty;
    }
}
