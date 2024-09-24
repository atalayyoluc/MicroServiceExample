using MediatR;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Application.Features.Queries;

public record GetAllProductQueries
() : IRequest<List<Product>>;

public class GetAllProductQueriesHandler : IRequestHandler<GetAllProductQueries, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductQueriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> Handle(GetAllProductQueries query, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllProductsAsync();
        return products;
    }
}
