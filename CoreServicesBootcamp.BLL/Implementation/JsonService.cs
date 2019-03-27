using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using CoreServicesBootcamp.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class JsonService : IFileService
    {
        private RequestContext _context;

        public JsonService(RequestContext context)
        {
            _context = context;
        }

        public bool LoadToDb(IFormFile file)
        {
            //RequestRepository repository = new RequestRepository(_context);
            OrderService orderService = new OrderService(_context);

            var result = string.Empty;

            //read json file to string
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                result = reader.ReadToEnd();
            }

            //convert json to list of requests
            var rqst = JsonConvert.DeserializeObject<RequestsJson>(result);

            if (rqst != null && rqst.Requests != null)
            {
                foreach (RequestJson rq in rqst.Requests)
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
