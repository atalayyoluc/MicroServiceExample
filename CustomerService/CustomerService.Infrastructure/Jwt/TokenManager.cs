using CustomerService.Application.Common.Extensions;
using CustomerService.Application.Common.Interface;
using CustomerService.Application.Common.Models.Identity;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Settings;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerService.Infrastructure.Jwt;

public class TokenManager : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenManager(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }


    public Task<AccessToken> GenerateAccessTokenAsync(User user)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: GetClaimsForAccessTokenAsync(user),
            signingCredentials: GetAccessTokenSigningCredentials(),
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.AccessTokenExpirationMinutes))
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return Task.FromResult(new AccessToken
        {
            Token = tokenString,
            Expires = tokenOptions.ValidTo
        });
    }

    public async Task<AuthenticateResult> GenerateRefreshTokenAsync(User user)
    {
        var tokenOptions = new JwtSecurityToken
                (
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: GetClaimsForRefreshTokenAsync(user),
                    signingCredentials: GetRefreshTokenSigningCredentials()
                );
        var accessToken = await GenerateAccessTokenAsync(user);
        var refreshToken = new AuthenticateResult
        {
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions)
        };
        return (refreshToken, accessToken).Adapt<AuthenticateResult>();
    }

    private List<Claim> GetClaimsForRefreshTokenAsync(User user)
    {
        var claims = new List<Claim>();
        claims.AddIdentifier(user.Id);
        claims.AddTel(user.PhoneNumber);
        return claims;
    }

    private List<Claim> GetClaimsForAccessTokenAsync(User user)
    {

        var claims = new List<Claim>();
        claims.AddIdentifier(user.Id);
        claims.AddTel(user.PhoneNumber);

        return claims;
    }

    private SigningCredentials GetRefreshTokenSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret);
        var secretKey = new SymmetricSecurityKey(key);

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

    private SigningCredentials GetAccessTokenSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenSecret);
        var secretKey = new SymmetricSecurityKey(key);

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }
}