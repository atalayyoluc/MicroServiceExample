using CustomerService.Application.Common.Models.Identity;
using CustomerService.Application.Common.Models.Users;

namespace CustomerService.Application.Common.Interface;

public interface IIdentityService
{
    Task<AuthenticateResult> LoginUserAsync(UserLogin userLogin, CancellationToken cancellationToken = default);
    Task<AuthenticateResult> RegisterUserAsync(UserRegister userRegister, CancellationToken cancellationToken = default);
    Task<AccessToken> RefreshUserAsync(UserRefresh userRefresh, CancellationToken cancellationToken = default);
}