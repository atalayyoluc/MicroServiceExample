using MongoDB.Driver;
using OrderService.Domain.Entities;
using System.Threading.Tasks;

namespace OrderService.Application.Common.Interface
{
    public interface IApplicationDbContext
    {
        IMongoCollection<Order> Orders { get; }

    }
}
