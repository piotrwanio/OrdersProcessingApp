using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreServicesBootcamp.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreServicesBootcamp.UI.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileStrategy _fileStrategy;

        public FileController(IFileStrategy fileStrategy)
        {
            _fileStrategy = fileStrategy;
        }

        public IActionResult FilesLoad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FilesLoad(List<IFormFile> files)
        {
            bool loadSucceeded = false;
            int successCount = 0;

            foreach (var file in files)
            {
                //load file to db using correct strategy
                switch (file.ContentType)
                {
                    case "application/json":
                        loadSucceeded = _fileStrategy.LoadToDb(file, FileExtension.Json);
                        break;
                    case "application/vnd.ms-excel":
                        loadSucceeded = _fileStrategy.LoadToDb(file, FileExtension.Csv);
                        break;
                    case "text/xml":
                        loadSucceeded = _fileStrategy.LoadToDb(file, FileExtension.Xml);
                        break;
                    default:
                        loadSucceeded = false;
                        break;
                }
                if (loadSucceeded == true) successCount++;
                else ViewBag.WasFailure = true;
            }
            ViewBag.SuccessCount = successCount;
            return View();
        }
    }
}