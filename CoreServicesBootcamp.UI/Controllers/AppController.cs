using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreServicesBootcamp.UI.Controllers
{
    public class AppController : Controller
    {

        private readonly RequestContext _context;

        public AppController(RequestContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllOrdersList()
        {
            OrderService orderService = new OrderService(_context);

            return View(orderService.GetAllOrders());
        }

        [HttpPost]
        public ActionResult FileLoad(List<IFormFile> files)
        {
            IFileService fileService;

            foreach(var file in files)
            {
                switch (file.ContentType)
                {
                    case "application/json":
                        fileService = new JsonService(_context);
                        break;
                    case "text/csv":
                        fileService = new JsonService(_context);
                        break;
                    case "application/xml":
                        fileService = new JsonService(_context);
                        break;
                    default:
                        return View();
                }
                fileService.LoadToDb(file);
            }
            return RedirectToAction("Index");
        }
    }
}