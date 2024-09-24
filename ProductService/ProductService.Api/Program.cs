using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure;
using ProductService.Application;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Application services (MediatR, business logic)
builder.Services.AddApplicationServices();

// Infrastructure services (PostgreSQL, IProductRepository)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
