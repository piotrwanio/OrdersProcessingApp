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
        private readonly IOrderService _orderService;

        public AppController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            ClientsViewModel viewModel = new ClientsViewModel
            {
                ClientList = new List<SelectListItem>()
            };
            
            //getting all orders from service
            var ordersDTO = _orderService.GetAllOrders();
            List<int> ids = new List<int>();


            //find all clients
            foreach (var order in ordersDTO.OrdersList)
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
    }
}