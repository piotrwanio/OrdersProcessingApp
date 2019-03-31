using CoreServicesBootcamp.BLL.Entities;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using CoreServicesBootcamp.DAL.Entities;
using Order = CoreServicesBootcamp.DAL.Entities.Order;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class CsvService : IFileService
    {
        private RequestContext _context;

        public CsvService(RequestContext context)
        {
            _context = context;
            FileExtension = FileExtension.Csv;
        }

        public FileExtension FileExtension { get; }

        public bool LoadToDb(IFormFile file)
        {
            //convert csv to list of requests
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = ",";

                List<RequestCsv> records = null;

                try
                {
                    records = csv.GetRecords<RequestCsv>().ToList();
                }
                catch(HeaderValidationException exception)
                {
                    Debug.WriteLine(exception.Message);
                    return false;
                }
                catch(Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return false;
                }

                //add converted requests to database
                if (records != null)
                {
                    foreach (var rq in records)
                    {
                        Request request = new Request
                        {
                            ClientId = int.Parse(rq.Client_Id),
                            Name = rq.Name,
                            Price = Double.Parse(rq.Price, CultureInfo.InvariantCulture),
                            Quantity = int.Parse(rq.Quantity),
                            RequestId = long.Parse(rq.Request_id, CultureInfo.InvariantCulture)
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
                            var newOrder = new Order
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
}
