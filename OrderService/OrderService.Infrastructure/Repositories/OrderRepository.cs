using MongoDB.Driver;
using OrderService.Application.Common.Interface;
using OrderService.Application.Common.Interface.Repositories;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;


public class OrderRepository : IOrderRepository
{
    private readonly IApplicationDbContext _context;

    public OrderRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _context.Orders.InsertOneAsync(order);
    }

    public async Task<Order> GetOrderByIdAsync(string id)
    {
        return await _context.Orders.Find(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateOrderStatusAsync(string id, OrderStatus status)
    {
        var update = Builders<Order>.Update.Set(o => o.Status, status);
        await _context.Orders.UpdateOneAsync(o => o.Id == id, update);
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.Find(_ => true).ToListAsync();
    }

    public async Task<List<Order>> GetAllOrdersAsync(int customerId)
    {
        return await _context.Orders.Find(o => o.CustomerId == customerId).ToListAsync();
    }
}
