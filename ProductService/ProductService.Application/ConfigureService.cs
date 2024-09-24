using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Common.Services;
using System.Reflection;

namespace ProductService.Application
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHostedService<RabbitMqBackgroundService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
