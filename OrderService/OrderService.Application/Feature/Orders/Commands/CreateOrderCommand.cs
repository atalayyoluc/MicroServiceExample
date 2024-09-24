using Mapster;
using MediatR;
using OrderService.Application.Common.Interface;
using OrderService.Application.Common.Interface.Repositories;
using OrderService.Application.Feature.Orders.Dtos;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Feature.Orders.Commands;

public record CreateOrderCommand
(
     int CustomerId,
     CreateOrderCommandDto Product
) : IRequest<string>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRabbitMqProductCheckService _rabbitMqProductCheckService;
    private readonly IRabbitMqCustomerCheckService _rabbitMqCustomerCheckService;
    public CreateOrderCommandHandler(IOrderRepository orderRepository, IRabbitMqProductCheckService rabbitMqProductCheckService, IRabbitMqCustomerCheckService rabbitMqCustomerCheckService)
    {
        _orderRepository = orderRepository;
        _rabbitMqProductCheckService = rabbitMqProductCheckService;
        _rabbitMqCustomerCheckService = rabbitMqCustomerCheckService;
    }

    public async Task<string> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var customerCheck = await _rabbitMqCustomerCheckService.PlaceOrder(command.CustomerId);
        if (!customerCheck)
        {
            return "Başarısız";
        }
        var products = new List<ProductDetails>();
        foreach (var productId in command.Product.ProductId)
        {
            var product = await _rabbitMqProductCheckService.PlaceOrder(productId);
            if (product == null)
            {
                return "Başarısız";
            }

            products.Add(product);
        }
        var a = (products, command);
        var order = new Order()
        {
            CustomerId = command.CustomerId,
            Products = products,
            Status = OrderStatus.Pending,
            TotalPrice = products.Sum(p => p.Price)
        };
        await _orderRepository.CreateOrderAsync(order);

        return order.Id;
    }
}
