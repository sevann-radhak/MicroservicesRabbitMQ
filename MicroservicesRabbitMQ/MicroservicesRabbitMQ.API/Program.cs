using MicroservicesRabbitMQ.Banking.Data.Context;
using MicroservicesRabbitMQ.Infra.IoC;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankingDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BankingDbConnection"));
});

DependencyContainer.RegisterServices(builder.Services, builder.Configuration);

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
