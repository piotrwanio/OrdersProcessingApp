using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL.Entities;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.BLL.Interfaces;
using OrderBLL = CoreServicesBootcamp.BLL.Models.OrderBLL;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class OrderService : IOrderService
    {
        private RequestContext _context;

        public OrderService(RequestContext context)
        {
            _context = context;
        }

        public OrderDTO OrdersPriceRange(double min, double max)
        {
            //creating data transfer object
            OrderDTO orderDTO = new OrderDTO();

            //get orders in price range using LINQ
            List<Order> orders = GetAllOrders().OrdersList;
            orderDTO.OrdersList = (from order in orders
                    where order.Amount >= min && order.Amount <= max
                    select order).ToList();

            return orderDTO;
        }

        public double OrdersAverageAmount()
        {
            if(OrdersCount() != 0)
                return OrdersTotalAmount() / OrdersCount();
            return 0;
        }

        public double OrdersAverageAmountByClient(int clientId)
        {
            if (OrdersCountByClient(clientId) != 0)
                return OrdersTotalAmountByClient(clientId) / OrdersCountByClient(clientId);
            return 0;
        }

        public double OrdersTotalAmount()
        {
            List<Order> all = GetAllOrders().OrdersList;
            double total = 0;

            foreach(var req in all)
            {
                total += req.Amount;
            }

            return total;
        }

        public double OrdersTotalAmountByClient(int clientId)
        {
            List<Order> all = GetOrdersByClient(clientId).OrdersList;
            double total = 0;

            foreach (var req in all)
            {
                total += req.Amount;
            }

            return total;
        }

        public int OrdersCount()
        {
            return GetAllOrders().OrdersList.Count();
        }

        public int OrdersCountByClient(int clientId)
        {
            return GetOrdersByClient(clientId).OrdersList.Count();
        }

        public OrderDTO GetOrdersByClient(int clientId)
        {
            OrderDTO orderDTO = new OrderDTO();
            List<Order> orders = GetAllOrders().OrdersList;

            orderDTO.OrdersList = (from r in orders
                               where r.ClientId == clientId
                               select r).ToList();

            //return all orders for client with id = clientId 
            return orderDTO;
        }

        public OrderDTO GetAllOrders()
        {
            OrderDTO orderDTO = new OrderDTO();

            var requests = _context.Requests.Where(m => true).ToList();
            var orders = _context.Orders.Where(m => true).ToList();

            //add requests to each order
            foreach(var order in orders)
            {
                List<Request> requestsList = (from r in requests
                                              where r.Order.OrderId == order.OrderId
                                              select r).ToList();
                if (order.Requests == null) order.Requests = new List<Request>();
                order.Requests.AddRange(requestsList);
            }

            orderDTO.OrdersList = orders;
            return orderDTO;
        }
    }
}
