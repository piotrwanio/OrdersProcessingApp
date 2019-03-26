using CoreServicesBootcamp.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Interfaces
{
    public interface IFileService
    {
        void LoadToDb(IFormFile file);
    }
}
