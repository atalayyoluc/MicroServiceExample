using CustomerService.Application.Features.Customers.Dtos;
using CustomerService.Application.Features.Customers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomerService.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "AccessToken")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var customer = await _mediator.Send(new GetCustomerDetailQuery(userId));
        return Ok(customer);
    }
}

