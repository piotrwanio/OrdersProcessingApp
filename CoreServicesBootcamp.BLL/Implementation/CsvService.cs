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

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class CsvService : IFileService
    {
        private RequestContext _context;

        public CsvService(RequestContext context)
        {
            _context = context;
        }

        public bool LoadToDb(IFormFile file)
        {

            //convert csv to list of requests
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.Delimiter = ",";

                var records = csv.GetRecords<RequestCsv>();

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
                        _context.Add(request);
                        _context.SaveChanges();
                    }
                    return true;
                }

                return false;
            }
        }
    }
}
