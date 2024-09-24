using MediatR;
using OrderService.Application.Common.Interface.Repositories;
using OrderService.Domain.Entities;
using ZstdSharp.Unsafe;

namespace OrderService.Application.Feature.Orders.Queries;

public record GetOrderQueries
(
    int CustomerId
) : IRequest<List<Order>>;

public class GetOrderQueriesHandler : IRequestHandler<GetOrderQueries, List<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueriesHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<Order>> Handle(GetOrderQueries queries, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllOrdersAsync(queries.CustomerId);
        return orders;
    }
}
