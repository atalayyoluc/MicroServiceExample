namespace CustomerService.Application.Common.Models.Identity;


public class AccessToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}