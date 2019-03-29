using System;
using System.Collections.Generic;
using System.Text;
using CoreServicesBootcamp.DAL.Entities;

namespace CoreServicesBootcamp.BLL.Models
{
    public class OrderDTO
    {
        public int Quantity { get; set; }
        public List<OrderBLL> Orders { get; set; }
        public List<DAL.Entities.Order> OrdersList { get; set; }
    }
}
