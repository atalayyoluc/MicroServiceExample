using CustomerService.Application.Common.Models.Users;
using System.Text;

namespace CustomerService.Infrastructure.Helpers;

public class HashingHelper
{
    public static HashedPassword CreatePasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        return new()
        {
            PasswordSalt = hmac.Key,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
        };
    }

    public static bool VerifyPasswordHash(string password, HashedPassword hashedPassword)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(hashedPassword.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hashedPassword.PasswordHash);
    }
}