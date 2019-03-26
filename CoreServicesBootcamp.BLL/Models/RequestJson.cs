using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Models
{
    public class RequestJson
    {
        public string ClientId { get; set; }
        public string RequestId { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
    }

    public class RequestsJson
    {
        [JsonProperty("requests")]
        public List<RequestJson> Requests { get; set;}
    }
}
