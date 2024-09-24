namespace CustomerService.Application.Common.Models.Users;

public class HashedPassword
{
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}