using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System.Collections.Generic;

namespace ProductService.Application.Common.Interface;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }

    int SaveChanges();
    int SaveChanges(bool acceptAllChangesOnSuccess);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
}
