using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            ClientsViewModel viewModel = new ClientsViewModel();
            viewModel.ClientList = new List<SelectListItem>();

            //creating order service
            OrderService orderService = new OrderService(_context);

            //getting all orders from service
            var ordersDTO = orderService.GetAllOrders();
            List<int> ids = new List<int>();

            //find all clients
            foreach (var order in ordersDTO.Orders)
            {
                if(!ids.Contains(order.ClientId))
                ids.Add(order.ClientId);
            }

            //add client list to ViewModel
            foreach(var id in ids)
            {
                viewModel.ClientList.Add(new SelectListItem { Text = id.ToString(), Value = id.ToString() });
            }

            return View(viewModel);
        }

        public IActionResult OrdersTotalAmount()
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            //set ViewBag property for deliver average amount to view
            ViewBag.TotalAmount = orderService.OrdersTotalAmount();
            
            return View();
        }

        public IActionResult OrdersTotalAmountByClient(int clientId)
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            //set ViewBag properties
            ViewBag.TotalAmount = orderService.OrdersTotalAmountByClient(clientId);
            ViewBag.ClientId = clientId;

            return View("OrdersTotalAmount");
        }

        public IActionResult OrdersAverageAmount()
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            ViewBag.AverageAmount = orderService.OrdersAverageAmount();
            return View();
        }

        public IActionResult OrdersAverageAmountByClient(int clientId)
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            ViewBag.AverageAmount = orderService.OrdersAverageAmountByClient(clientId);
            ViewBag.ClientId = clientId;

            return View("OrdersAverageAmount");
        }

        public IActionResult OrdersPriceRange(double min, double max, string sortOrder)
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            //get OrderDTO from service
            //DTO containst list of orders and quantity of orders
            var ordersDTO = orderService.OrdersPriceRange(min, max);

            SortList(ordersDTO, sortOrder);

            //sending min and max to ViewBag object
            ViewBag.min = min;
            ViewBag.max = max;

            if (ordersDTO.Orders.Count == 0) ViewBag.Message = "Brak zamówień";

            return View("OrdersList", ordersDTO);
        }

        public IActionResult OrdersQuantity()
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            ViewBag.NumberOfOrders = orderService.OrdersCount();
            return View();
        }

        public IActionResult ClientOrdersQuantity(int clientId)
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            ViewBag.ClientId = clientId;
            ViewBag.NumberOfOrders = orderService.OrdersCountByClient(clientId);
            return View("OrdersQuantity");
        }

        public IActionResult ClientOrdersList(string sortOrder, int clientId)
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            //get OrderDTO from service
            //DTO containst list of orders and quantity of orders
            var ordersDTO = orderService.GetOrdersByClient(clientId);

            SortList(ordersDTO, sortOrder);

            ViewBag.ClientId = clientId;

            if (ordersDTO.Orders.Count == 0) ViewBag.Message = "Brak zamówień";

            return View("OrdersList", ordersDTO); 
        }

        public IActionResult AllOrdersList(string sortOrder)
        {
            //creating order service
            OrderService orderService = new OrderService(_context);

            //get OrderDTO from service
            //DTO containst list of orders and quantity of orders
            var ordersDTO = orderService.GetAllOrders();

            SortList(ordersDTO, sortOrder);

            if (ordersDTO.Orders.Count == 0) ViewBag.Message = "Brak zamówień";


            return View("OrdersList", ordersDTO);
        }

        public IActionResult ProductOrdersNumber()
        {
            ProductService productService = new ProductService(_context);

            ProductDTO products = productService.GetProductsOrders();

            if (products.ProductOrders.Count == 0) ViewBag.Message = "Brak zamówień";

            return View(products);
        }

        public IActionResult FilesLoad()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FilesLoad(List<IFormFile> files)
        {
            IFileService fileService;

            foreach (var file in files)
            {
                //create service for loaded file
                switch (file.ContentType)
                {
                    case "application/json":
                        fileService = new JsonService(_context);
                        break;
                    case "application/vnd.ms-excel":
                        fileService = new CsvService(_context);
                        break;
                    case "text/xml":
                        fileService = new XmlService(_context);
                        break;
                    default:
                        return View();
                }

                //load file to db
                fileService.LoadToDb(file);
            }
            return View();
        }

        void SortList(OrderDTO ordersDTO, string sortOrder)
        {
            //implementation of sorting by clientId
            ViewBag.ClientSortParm = String.IsNullOrEmpty(sortOrder) ? "client_desc" : "";
            ViewBag.AmountSortParm = sortOrder == "Amount" ? "amount_desc" : "Amount";
            ViewBag.RequestSortParm = sortOrder == "Request" ? "request_desc" : "Request";

            switch (sortOrder)
            {
                case "client_desc":
                    ordersDTO.Orders = ordersDTO.Orders
                        .OrderByDescending(s => s.ClientId).ToList();
                    break;
                case "Amount":
                    ordersDTO.Orders = ordersDTO.Orders
                        .OrderBy(s => s.Amount).ToList();
                    break;
                case "amount_desc":
                    ordersDTO.Orders = ordersDTO.Orders
                        .OrderByDescending(s => s.Amount).ToList();
                    break;
                case "Request":
                    ordersDTO.Orders = ordersDTO.Orders
                        .OrderBy(s => s.RequestId).ToList();
                    break;
                case "request_desc":
                    ordersDTO.Orders = ordersDTO.Orders
                        .OrderByDescending(s => s.RequestId).ToList();
                    break;
                default:
                    ordersDTO.Orders = ordersDTO.Orders
                        .OrderBy(s => s.ClientId).ToList();
                    break;
            }
        }
    }
}