using CoreServicesBootcamp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Models
{
    //represents all request rows in client's request
    public class Order
    {
        public int ClientId { get; set; }
        public long RequestId { get; set; }
        public double PriceSum { get; set; }
        public List<Request> RequestsList { get; set; }
    }
}
