using CustomerService.Api;
using CustomerService.Application;
using CustomerService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplicationServices();
    builder.Services.AddnfrastructureServices(builder.Configuration);
    builder.Services.AddApiServices(builder.Configuration);
}

var app = builder.Build();
{

    // Configure the HTTP request pipeline.
    if (true)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}