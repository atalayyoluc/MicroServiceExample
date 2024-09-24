using CustomerService.Application.Common.Models.Users;
using CustomerService.Application.Features.Auth.Commands;
using CustomerService.Application.Features.Auth.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomerService.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegister userRegister)
    {
        var authenticationResult = await _mediator.Send(new RegisterUserCommand(userRegister));
        return Ok(authenticationResult);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLogin userLogin)
    {
        var authenticationResult = await _mediator.Send(new LoginQueries(userLogin));
        return Ok(authenticationResult);
    }


    [Authorize(Policy = "RefreshToken")]
    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var token = await _mediator.Send(new RefreshQuery(userId));
        return Ok(token);
    }

}