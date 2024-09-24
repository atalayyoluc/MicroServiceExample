using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Repositories.Interfaces;

public interface IProductRepository
{
    Task CreateProductAsync(Product product);
    Task<Product?> GetProductByIdAsync(int id);
    Task UpdateProductAsync(Product product);
    Task DeleteProductAsync(Product product);
    Task<List<Product>> GetAllProductsAsync();
}
