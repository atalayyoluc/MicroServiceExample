using CustomerService.Application.Common.Interface;
using CustomerService.Application.Common.Models.Identity;
using CustomerService.Application.Common.Models.Users;
using MediatR;

namespace CustomerService.Application.Features.Auth.Queries;

public record LoginQueries
(UserLogin User) : IRequest<AuthenticateResult>;

public class LoginQueriesHandler : IRequestHandler<LoginQueries, AuthenticateResult>
{
    private readonly IIdentityService _identityService;

    public LoginQueriesHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthenticateResult> Handle(LoginQueries query, CancellationToken cancellationToken)
    {
        return await _identityService.LoginUserAsync(query.User);

    }
}
