using MediatR;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Application.Features.Queries;

public record GetProductDetailQueries
(int Id) : IRequest<Product>;

public class GetProductDetailQueriesHandler : IRequestHandler<GetProductDetailQueries, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductDetailQueriesHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductDetailQueries query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(query.Id);
        if (product == null)
        {
            throw new Exception("Ürün Bulunamadı");
        }
        return product;
    }
}
