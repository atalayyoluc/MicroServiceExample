using CustomerService.Application.Common.Interface;
using CustomerService.Application.Common.Models.Identity;
using CustomerService.Application.Common.Models.Users;
using Mapster;
using MediatR;

namespace CustomerService.Application.Features.Auth.Queries;

public record RefreshQuery
(int UserId) : IRequest<AccessToken>;

public class RefreshQueryHandler : IRequestHandler<RefreshQuery, AccessToken>
{
    private readonly IIdentityService _identityService;

    public RefreshQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AccessToken> Handle(RefreshQuery query, CancellationToken cancellationToken)
    {
        return await _identityService.RefreshUserAsync(query.Adapt<UserRefresh>());
    }
}
