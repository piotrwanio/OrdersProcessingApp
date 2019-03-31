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
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Order = CoreServicesBootcamp.DAL.Entities.Order;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class XmlService : IFileService
    {
        private RequestContext _context;

        public XmlService(RequestContext context)
        {
            _context = context;
            FileExtension = FileExtension.Xml;
        }

        public FileExtension FileExtension { get; }

        public bool LoadToDb(IFormFile file)
        {
            //read json file to stream
            var reader = new StreamReader(file.OpenReadStream());


            //convert xml to list of requests
            XmlSerializer serializer = new XmlSerializer(typeof(RequestsXml));

            RequestsXml requests;

            try
            {
                requests = (RequestsXml)serializer.Deserialize(reader);
            }
            catch(Exception e)
            {
                return false;
            }

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

                    var order = _context.Orders.Where(m => m.ClientId == request.ClientId
                        && m.RequestId == request.RequestId);

                    if (order.Count() != 0)
                    {
                        request.Order = order.First();
                        order.First().Amount += request.Price * request.Quantity;
                    }
                    else
                    {
                        Order newOrder = new Order
                        {
                            ClientId = request.ClientId,
                            RequestId = request.RequestId,
                            Amount = request.Price * request.Quantity
                        };
                        _context.Orders.Add(newOrder);
                        _context.SaveChanges();

                        request.Order = newOrder;
                    }

                    _context.Requests.Add(request);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }
    }
}
