
using MediatR;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Application.Products.Commands;

public class CreateProductCommand : IRequest<int>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock
        };

        await _productRepository.CreateProductAsync(product);

        return product.Id;
    }
}