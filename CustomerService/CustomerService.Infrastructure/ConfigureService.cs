using CustomerService.Application.Common.Interface;
using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Common.Interfaces.Repositories;
using CustomerService.Infrastructure.Identity;
using CustomerService.Infrastructure.Jwt;
using CustomerService.Infrastructure.Persistence;
using CustomerService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddnfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            )
        );



        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        services.AddIdentityServices(configuration);

        services.AddScoped<IIdentityService, IdentityManager>();
        services.AddScoped<ITokenService, TokenManager>();

        return services;
    }
}