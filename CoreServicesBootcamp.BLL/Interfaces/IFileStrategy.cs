using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Interfaces
{
    
    public interface IFileStrategy
    {
        bool LoadToDb(IFormFile file, FileExtension extension);
    }

}
