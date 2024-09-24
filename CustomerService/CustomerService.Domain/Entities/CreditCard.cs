using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Domain.Entities;

public class CreditCard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] CardNumberHash { get; set; }
    public byte[] CardNumberSalt { get; set; }
    public DateTime ExpiryDate { get; set; }
    public byte[] CVCEncrypted { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
