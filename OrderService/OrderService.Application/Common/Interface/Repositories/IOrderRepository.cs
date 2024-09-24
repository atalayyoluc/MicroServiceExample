using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Common.Interface.Repositories;

public interface IOrderRepository
{
    Task CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(string id);
    Task UpdateOrderStatusAsync(string id, OrderStatus status);
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetAllOrdersAsync(int customerId);
}

