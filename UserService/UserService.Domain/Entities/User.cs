using System.ComponentModel.DataAnnotations;

namespace CustomerService.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
