using CoreServicesBootcamp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Models
{
    //represents all request rows in client's request
    public class WholeRequest
    {
        public int ClientId { get; set; }
        public long RequestId { get; set; }
        public int PriceSum { get; set; }
        public List<Request> RequestsList { get; set; }
    }
}
