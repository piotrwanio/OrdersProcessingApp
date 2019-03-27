using CoreServicesBootcamp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL.Entities;
using CoreServicesBootcamp.DAL;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class OrderService
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
            List<Order> orders = GetAllOrders().Orders;
            orderDTO.Orders = (from order in orders
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
            List<Order> all = GetAllOrders().Orders;
            double total = 0;

            foreach(var req in all)
            {
                total += req.Amount;
            }

            return total;
        }

        public double OrdersTotalAmountByClient(int clientId)
        {
            List<Order> all = GetOrdersByClient(clientId).Orders;
            double total = 0;

            foreach (var req in all)
            {
                total += req.Amount;
            }

            return total;
        }

        public int OrdersCount()
        {
            return GetAllOrders().Orders.Count();
        }

        public int OrdersCountByClient(int clientId)
        {
            return GetOrdersByClient(clientId).Orders.Count();
        }

        public OrderDTO GetOrdersByClient(int clientId)
        {
            OrderDTO orderDTO = new OrderDTO();
            List<Order> orders = GetAllOrders().Orders;

            orderDTO.Orders = (from r in orders
                               where r.ClientId == clientId
                               select r).ToList();

            //return all orders for client with id = clientId 
            return orderDTO;
        }

        public OrderDTO GetAllOrders()
        {
            OrderDTO orderDTO = new OrderDTO();
            List<Order> orders = new List<Order>();

            var allFromRepo = _context.Requests.Where(m => true).ToList();

            foreach(var req in allFromRepo)
            {
                var contains = (from r in orders
                                where r.ClientId == req.ClientId
                                && r.RequestId == req.RequestId
                                select r);
                if (contains.Count() != 0)
                {
                    contains.First().RequestsList.Add(req);
                    contains.First().Amount += req.Price * req.Quantity;
                }
                else
                {
                    Order wholeRequest = new Order();
                    wholeRequest.ClientId = req.ClientId;
                    wholeRequest.RequestId = req.RequestId;
                    wholeRequest.RequestsList = new List<Request>();
                    wholeRequest.RequestsList.Add(req);
                    wholeRequest.Amount += req.Price * req.Quantity;
                    orders.Add(wholeRequest);
                }
            }

            orderDTO.Orders = orders;
            return orderDTO;
        }



    }
}
