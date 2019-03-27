using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class XmlService : IFileService
    {
        private RequestContext _context;

        public XmlService(RequestContext context)
        {
            _context = context;
        }

        public bool LoadToDb(IFormFile file)
        {
            //read json file to stream
            var reader = new StreamReader(file.OpenReadStream());


            //convert xml to list of requests
            XmlSerializer serializer = new XmlSerializer(typeof(RequestsXml));

            RequestsXml requests = (RequestsXml)serializer.Deserialize(reader);
            reader.Close();

            if (requests != null && requests.Requests != null)
            {
                foreach (RequestXml rq in requests.Requests)
                {
                    Request request = new Request
                    {
                        ClientId = int.Parse(rq.ClientId),
                        Name = rq.Name,
                        Price = Double.Parse(rq.Price, CultureInfo.InvariantCulture),
                        Quantity = int.Parse(rq.Quantity),
                        RequestId = long.Parse(rq.RequestId, CultureInfo.InvariantCulture)
                    };
                    _context.Add(request);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }
    }
}
