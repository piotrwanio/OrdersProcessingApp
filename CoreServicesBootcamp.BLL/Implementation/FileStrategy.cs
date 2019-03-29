using CoreServicesBootcamp.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class FileStrategy : IFileStrategy
    {
        private readonly IEnumerable<IFileService> _services;

        public FileStrategy(IEnumerable<IFileService> services)
        {
            _services = services;
        }

        public bool LoadToDb(IFormFile file, FileExtension extension)
        {
            return _services.FirstOrDefault(x => x.FileExtension == extension)?.LoadToDb(file) ?? throw new ArgumentNullException(nameof(extension));
        }
    }
}
