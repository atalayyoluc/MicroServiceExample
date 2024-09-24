using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using OrderService.Application.Common.Interface;
using OrderService.Application.Common.Interface.Repositories;
using OrderService.Infrastructure.Identity;
using OrderService.Infrastructure.Messages;
using OrderService.Infrastructure.Persistence;

namespace OrderService.Infrastructure
{
    public static class ConfigureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration.GetValue<string>("MongoDbConnection:DefaultConnection");
                return new MongoClient(connectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = configuration.GetValue<string>("MongoDbConnection:Database");
                return client.GetDatabase(databaseName);
            });

            services.AddIdentityServices(configuration);

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRabbitMqProductCheckService, RabbitMqProductCheckService>();
            services.AddScoped<IRabbitMqCustomerCheckService, RabbitMqCustomerCheckService>();
            return services;
        }
    }
}
