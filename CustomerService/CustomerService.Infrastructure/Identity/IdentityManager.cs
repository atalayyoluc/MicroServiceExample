using CustomerService.Application.Common.Interface;
using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Common.Interfaces.Repositories;
using CustomerService.Application.Common.Models.Identity;
using CustomerService.Application.Common.Models.Users;
using CustomerService.Domain.Entities;
using CustomerService.Infrastructure.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Infrastructure.Identity;
public class IdentityManager : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public IdentityManager(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    public async Task<AuthenticateResult> LoginUserAsync(UserLogin userLogin, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByLoginKeyAsync(userLogin.LoginKey);
        if (user == null)
        {
            throw new Exception("User Not Found");
        }

        var userPassword = user.Adapt<HashedPassword>();
        var hashedPassword = HashingHelper.VerifyPasswordHash(userLogin.LoginPassword, userPassword);
        if (!hashedPassword)
        {
            throw new Exception("Password Not Correct");
        }
        var token = await _tokenService.GenerateRefreshTokenAsync(user);
        return token;
    }

    public async Task<AccessToken> RefreshUserAsync(UserRefresh userRefresh, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetUserByIdAsync(userRefresh.UserId);
        if (user == null)
        {
            throw new Exception("User Not Found");
        }
        var token = await _tokenService.GenerateAccessTokenAsync(user);
        return token;
    }

    public async Task<AuthenticateResult> RegisterUserAsync(UserRegister userRegister, CancellationToken cancellationToken)
    {

        var userEmailCheck = await _userRepository.GetUserByLoginKeyAsync(userRegister.Email);
        var userNameCheck = await _userRepository.GetUserByLoginKeyAsync(userRegister.UserName);
        var userPhoneNumberCheck = await _userRepository.GetUserByLoginKeyAsync(userRegister.PhoneNumber);
        if (userEmailCheck != null)
        {
            throw new Exception("Email already exist");
        }
        if (userNameCheck != null)
        {
            throw new Exception("Username already exist");
        }
        if (userPhoneNumberCheck != null)
        {
            throw new Exception("Phone number already exist");
        }

        var hashedPassword = HashingHelper.CreatePasswordHash(userRegister.Password);
        var user = (userRegister, hashedPassword).Adapt<User>();

        await _userRepository.CreateUserAsync(user);

        var token = await _tokenService.GenerateRefreshTokenAsync(user);
        return token;
    }

}