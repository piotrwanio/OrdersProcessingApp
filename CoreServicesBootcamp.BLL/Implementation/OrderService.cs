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

        public List<Order> OrdersPriceRange(double min, double max)
        {
            List<Order> orders = GetAllOrders().Orders;
            return (from order in orders
                    where order.PriceSum >= min && order.PriceSum <= max
                    select order).ToList();
        }

        public double OrdersAverageAmount()
        {
            return OrdersTotalAmount() / OrdersCount();
        }

        public double OrdersAverageAmountByClient(int clientId)
        {
            return OrdersTotalAmountByClient(clientId) / OrdersCountByClient(clientId);
        }

        public double OrdersTotalAmount()
        {
            List<Order> all = GetAllOrders().Orders;
            double average = 0;

            foreach(var req in all)
            {
                average += req.PriceSum;
            }

            return average;
        }

        public double OrdersTotalAmountByClient(int clientId)
        {
            List<Order> all = GetOrdersByClient(clientId);
            double average = 0;

            foreach (var req in all)
            {
                average += req.PriceSum;
            }

            return average;
        }

        public int OrdersCount()
        {
            return GetAllOrders().Orders.Count();
        }

        public int OrdersCountByClient(int clientId)
        {
            return GetOrdersByClient(clientId).Count();
        }

        public List<Order> GetOrdersByClient(int clientId)
        {
            List<Order> wholeRequests = GetAllOrders().Orders;

            //return all whole requests for client with id = clientId 
            return (from r in wholeRequests
                    where r.ClientId == clientId
                    select r).ToList();
        }

        public OrderDTO GetAllOrders()
        {
            OrderDTO orderDTO = new OrderDTO();
            List<Order> orders = new List<Order>();

            var allFromRepo = _context.Requests.Where(m => true).ToList();

            //var duplicates = (from r in allFromRepo
            //                  group r by new { r.ClientId, r.RequestId } into results
            //                  select results.Skip(1)
            //     ).SelectMany(a => a).ToList();


            //foreach (var dup in duplicates)
            //{
            //    allFromRepo.Remove(dup);
            //}

            foreach(var req in allFromRepo)
            {
                var contains = (from r in orders
                                where r.ClientId == req.ClientId
                                && r.RequestId == req.RequestId
                                select r);
                if (contains.Count() != 0)
                {
                    contains.First().RequestsList.Add(req);
                    contains.First().PriceSum += req.Price;
                }
                else
                {
                    Order wholeRequest = new Order();
                    wholeRequest.ClientId = req.ClientId;
                    wholeRequest.RequestId = req.RequestId;
                    wholeRequest.RequestsList = new List<Request>();
                    wholeRequest.RequestsList.Add(req);
                    wholeRequest.PriceSum += req.Price;
                    orders.Add(wholeRequest);
                }
            }

            orderDTO.Orders = orders;
            return orderDTO;
        }



    }
}
