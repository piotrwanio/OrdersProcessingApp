using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreServicesBootcamp.BLL.Entities
{
    public class RequestCsv
    {
        public string Client_Id { get; set; }
        public string Request_id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
    }
    
}
