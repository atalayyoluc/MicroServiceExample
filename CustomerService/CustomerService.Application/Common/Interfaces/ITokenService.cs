using CustomerService.Application.Common.Models.Identity;
using CustomerService.Domain.Entities;

namespace CustomerService.Application.Common.Interface;

public interface ITokenService
{
    Task<AccessToken> GenerateAccessTokenAsync(User user);
    Task<AuthenticateResult> GenerateRefreshTokenAsync(User user);
}