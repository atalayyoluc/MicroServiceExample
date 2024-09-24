namespace CustomerService.Application.Common.Models.Identity;

public class AuthenticateResult
{
    public AccessToken? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}