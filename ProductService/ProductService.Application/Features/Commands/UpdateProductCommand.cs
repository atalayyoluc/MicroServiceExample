using Mapster;
using MediatR;
using ProductService.Application.Features.Dtos;
using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Application.Features.Commands;

public record UpdateProductCommand
(
    int ProductId,
    UpdateProductCommandDto Product
) : IRequest<int>;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(command.ProductId);
        if (product == null)
        {
            throw new Exception("Ürün Bulunamadi");
        }
        command.Product.Adapt(product);
        await _productRepository.UpdateProductAsync(product);
        return product.Id;

    }
}
