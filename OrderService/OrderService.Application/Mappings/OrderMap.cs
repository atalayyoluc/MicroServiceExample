using Mapster;
using OrderService.Application.Feature.Orders.Commands;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Application.Mappings;

public class OrderMap : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(List<ProductDetails> products, CreateOrderCommand command), Order>()
            .Map(dest => dest.TotalPrice, src => src.products.Sum(s => s.Price * s.Stock))
            .Map(dest => dest.CustomerId, src => src.command.CustomerId)
            .Map(dest => dest.Status, src => OrderStatus.Pending)
            .Map(dest => dest.Products, src => src.products);
    }
}
