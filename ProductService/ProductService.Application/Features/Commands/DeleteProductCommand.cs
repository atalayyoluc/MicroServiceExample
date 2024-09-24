using MediatR;
using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Application.Features.Commands;

public record DeleteProductCommand
(int Id) : IRequest;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(command.Id);
        if (product == null)
        {
            throw new Exception("Ürün Bulunamadi");
        }
        await _productRepository.DeleteProductAsync(product);
    }
}
