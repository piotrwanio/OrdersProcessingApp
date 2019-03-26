using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class XmlService : IFileService
    {
        private RequestContext _context;

        public XmlService(RequestContext context)
        {
            _context = context;
        }

        public void LoadToDb(IFormFile file)
        {
            var result = string.Empty;

            //read json file to string
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                result = reader.ReadToEnd();
            }

            //convert xml to list of requests
        }
    }
}
