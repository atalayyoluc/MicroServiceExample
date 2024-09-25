using System.Security.Claims;

namespace CustomerService.Application.Common.Extensions;

public static class ClaimExtensions
{
    public static void AddEmail(this ICollection<Claim> claims, string email)
    {
        claims.Add(new Claim(ClaimTypes.Email, email));
    }

    public static void AddName(this ICollection<Claim> claims, string name)
    {
        claims.Add(new Claim(ClaimTypes.Name, name));
    }
   
    public static void AddTel(this ICollection<Claim> claims, string tel)
    {
        claims.Add(new Claim(ClaimTypes.MobilePhone, tel));
    }

    public static void AddIdentifier(this ICollection<Claim> claims, int id)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
    }
}