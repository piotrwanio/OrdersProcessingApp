using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using Microsoft.AspNetCore.Mvc;

namespace CoreServicesBootcamp.UI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly RequestContext _context;

        public OrderController(IOrderService orderService, RequestContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        public IActionResult OrdersTotalAmount()
        {
            //set ViewBag property for deliver average amount to view
            ViewBag.TotalAmount = _orderService.OrdersTotalAmount();

            return View();
        }

        public IActionResult OrdersTotalAmountByClient(int clientId)
        {
            //set ViewBag properties
            ViewBag.TotalAmount = _orderService.OrdersTotalAmountByClient(clientId);
            ViewBag.ClientId = clientId;

            return View("OrdersTotalAmount");
        }

        public IActionResult OrdersAverageAmount()
        {
            ViewBag.AverageAmount = _orderService.OrdersAverageAmount();
            return View();
        }

        public IActionResult OrdersAverageAmountByClient(int clientId)
        {
            ViewBag.AverageAmount = _orderService.OrdersAverageAmountByClient(clientId);
            ViewBag.ClientId = clientId;

            return View("OrdersAverageAmount");
        }

        public IActionResult OrdersPriceRange(double min, double max, string sortOrder)
        {
            //get OrderDTO from service
            //DTO containst list of orders and quantity of orders
            var ordersDTO = _orderService.OrdersPriceRange(min, max);

            SortList(ordersDTO, sortOrder);

            //sending min and max to ViewBag object
            ViewBag.min = min;
            ViewBag.max = max;

            if (ordersDTO.OrdersList.Count == 0) ViewBag.Message = "Brak zamówień";

            return View("OrdersList", ordersDTO);
        }

        public IActionResult OrdersQuantity()
        {
            ViewBag.NumberOfOrders = _orderService.OrdersCount();
            return View();
        }

        public IActionResult ClientOrdersQuantity(int clientId)
        {
            ViewBag.ClientId = clientId;
            ViewBag.NumberOfOrders = _orderService.OrdersCountByClient(clientId);
            return View("OrdersQuantity");
        }

        public IActionResult ClientOrdersList(string sortOrder, int clientId)
        {
            //get OrderDTO from service
            //DTO containst list of orders and quantity of orders
            var ordersDTO = _orderService.GetOrdersByClient(clientId);

            SortList(ordersDTO, sortOrder);

            ViewBag.ClientId = clientId;

            if (ordersDTO.OrdersList.Count == 0) ViewBag.Message = "Brak zamówień";

            return View("OrdersList", ordersDTO);
        }

        public IActionResult AllOrdersList(string sortOrder)
        {
            //get OrderDTO from service
            //DTO containst list of orders and quantity of orders
            var ordersDTO = _orderService.GetAllOrders();

            SortList(ordersDTO, sortOrder);

            if (ordersDTO.OrdersList.Count == 0) ViewBag.Message = "Brak zamówień";


            return View("OrdersList", ordersDTO);
        }

        public IActionResult ProductOrdersNumber()
        {
            ProductService productService = new ProductService(_context);

            ProductDTO products = productService.GetProductsOrders();

            if (products.ProductOrders.Count == 0) ViewBag.Message = "Brak zamówień";

            return View(products);
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
                    ordersDTO.OrdersList = ordersDTO.OrdersList
                        .OrderByDescending(s => s.ClientId).ToList();
                    break;
                case "Amount":
                    ordersDTO.OrdersList = ordersDTO.OrdersList
                        .OrderBy(s => s.Amount).ToList();
                    break;
                case "amount_desc":
                    ordersDTO.OrdersList = ordersDTO.OrdersList
                        .OrderByDescending(s => s.Amount).ToList();
                    break;
                case "Request":
                    ordersDTO.OrdersList = ordersDTO.OrdersList
                        .OrderBy(s => s.RequestId).ToList();
                    break;
                case "request_desc":
                    ordersDTO.OrdersList = ordersDTO.OrdersList
                        .OrderByDescending(s => s.RequestId).ToList();
                    break;
                default:
                    ordersDTO.OrdersList = ordersDTO.OrdersList
                        .OrderBy(s => s.ClientId).ToList();
                    break;
            }
        }
    }
}