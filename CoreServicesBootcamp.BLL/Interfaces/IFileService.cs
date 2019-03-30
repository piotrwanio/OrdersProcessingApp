using CoreServicesBootcamp.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Interfaces
{
    public enum FileExtension
    {
        Json,
        Xml,
        Csv
    }

    public interface IFileService
    {
        FileExtension FileExtension { get; }

        bool LoadToDb(IFormFile file);
    }
}
