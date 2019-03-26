using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreServicesBootcamp.DAL.Entities
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public long RequestId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
    
}
