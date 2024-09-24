using OrderService.Domain.Entities;

namespace OrderService.Application.Common.Interface;

public interface IRabbitMqProductCheckService
{
    Task<ProductDetails> PlaceOrder(int productId);
}
