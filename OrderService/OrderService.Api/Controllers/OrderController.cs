using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Feature.Orders.Commands;
using OrderService.Application.Feature.Orders.Dtos;
using OrderService.Application.Feature.Orders.Queries;
using System.Security.Claims;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "AccessToken")]
    public async Task<IActionResult> Create(CreateOrderCommandDto createOrderCommandDto)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var orderId = await _mediator.Send(new CreateOrderCommand(userId, createOrderCommandDto));
        return Ok(orderId);
    }


    [HttpGet("MyOrder")]
    [Authorize(AuthenticationSchemes = "AccessToken")]
    public async Task<IActionResult> Get()
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var order = await _mediator.Send(new GetOrderQueries(userId));
        return Ok(order);
    }

}
