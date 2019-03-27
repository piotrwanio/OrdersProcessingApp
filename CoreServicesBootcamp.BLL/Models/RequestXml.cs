using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CoreServicesBootcamp.BLL.Models
{
    [XmlRoot(ElementName = "request")]
    public class RequestXml
    {
        [XmlElement(ElementName = "clientId")]
        public string ClientId { get; set; }
        [XmlElement(ElementName = "requestId")]
        public string RequestId { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "quantity")]
        public string Quantity { get; set; }
        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
    }

    [XmlRoot(ElementName = "requests")]
    public class RequestsXml
    {
        [XmlElement(ElementName = "request")]
        public List<RequestXml> Requests { get; set; }
    }
}
