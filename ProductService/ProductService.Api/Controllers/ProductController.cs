using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Commands;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Queries;
using ProductService.Application.Products.Commands;

namespace ProductService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return Ok(productId);
    }
    [HttpPut("id")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductCommandDto command)
    {
        var productId = await _mediator.Send(new UpdateProductCommand(id, command));
        return Ok(productId);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int productId)
    {
        await _mediator.Send(new DeleteProductCommand(productId));
        return NoContent();
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var product = await _mediator.Send(new GetAllProductQueries());
        return Ok(product);
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _mediator.Send(new GetProductDetailQueries(id));
        return Ok(product);
    }
}
