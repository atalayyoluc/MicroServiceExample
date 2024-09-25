using CustomerService.Application.Common.Interface;
using CustomerService.Application.Common.Models.Identity;
using CustomerService.Application.Common.Models.Users;
using MediatR;

namespace CustomerService.Application.Features.Auth.Commands;

public record RegisterUserCommand
(
    UserRegister User
) : IRequest<AuthenticateResult>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthenticateResult>
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthenticateResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        return await _identityService.RegisterUserAsync(command.User, cancellationToken);
    }
}
