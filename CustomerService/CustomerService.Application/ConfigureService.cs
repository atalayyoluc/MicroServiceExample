using CustomerService.Application.Common.Services;
using CustomerService.Application.Mappings;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CustomerService.Application;

public static class ConfigureService
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddMapperConfiguration();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddHostedService<RabbitMqCustomerCheckBackgroundService>();

        return services;
    }
}