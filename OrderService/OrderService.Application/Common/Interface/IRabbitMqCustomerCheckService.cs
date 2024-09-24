namespace OrderService.Application.Common.Interface;

public interface IRabbitMqCustomerCheckService
{
    Task<bool> PlaceOrder(int productId);
}
