using MongoDB.Driver;
using OrderService.Application.Common.Interface;
using OrderService.Domain.Entities;

using System.Threading.Tasks;

namespace OrderService.Infrastructure.Persistence
{
    public class ApplicationDbContext : IApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");

    }
}
