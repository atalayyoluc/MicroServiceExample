using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 1, Confirmed = 2, Shipped = 3, Delivered = 4, Canceled = 5
    }
}
