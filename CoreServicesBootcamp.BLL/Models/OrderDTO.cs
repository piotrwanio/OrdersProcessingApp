using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Models
{
    public class OrderDTO
    {
        public int Quantity { get; set; }
        public List<Order> Orders { get; set; }
    }
}
