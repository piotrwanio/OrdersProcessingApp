using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using CoreServicesBootcamp.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using OrderBLL = CoreServicesBootcamp.BLL.Models.OrderBLL;

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class OrderServiceTests
    {
        [TestMethod]
        public void Can_Get_All_Orders()
        {
            //preparation - create new dbcontext with in-memory db
            using (var context = new RequestContext(CreateNewContextOptions()))
            {
                Request request = new Request
                {
                    Id = 1111,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 111,
                    Name = "product1"
                };

                DAL.Entities.Order order = new DAL.Entities.Order
                {
                    OrderId = 1,
                    ClientId = request.ClientId,
                    RequestId = request.RequestId,
                    Amount = request.Price * request.Quantity
                };

                request.Order = order;

                //adding order to db
                context.Requests.Add(request);
                context.Orders.Add(order);

                context.SaveChanges();

                var service = new OrderService(context);
                List<Order> orders = service.GetAllOrders().OrdersList;
                
                //assert - is quantity of orders equal 1 
                Assert.AreEqual(1, orders.Count);
                //assert - check correct of insert data
                Assert.AreEqual(111, orders[0].ClientId);
            }
        }

        [TestMethod]
        public void Can_Get_Client_Orders()
        {
            //create db context
            using (var context = new RequestContext(CreateNewContextOptions()))
            {
                //create requests to add to db
                Request request1 = new Request
                {
                    Id = 2222,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 1,
                    Name = "product1"
                };
                Request request2 = new Request
                {
                    Id = 3333,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 22,
                    Name = "product2"
                };
                Request request3 = new Request
                {
                    Id = 3334,
                    Price = 20,
                    Quantity = 12,
                    RequestId = 1,
                    ClientId = 22,
                    Name = "product3"
                };

                //create orders to add to db
                Order order1 = new Order
                {
                    OrderId = 1,
                    ClientId = request1.ClientId,
                    RequestId = request1.RequestId,
                    Amount = request1.Price * request1.Quantity
                };
                Order order2 = new Order
                {
                    OrderId = 2,
                    ClientId = request2.ClientId,
                    RequestId = request2.RequestId,
                    Amount = request2.Price * request2.Quantity
                };

                //assignment orders to requests
                request1.Order = order1;
                request2.Order = order2;
                request3.Order = order2;

                //add requests and orders to database
                context.Requests.Add(request1);
                context.Requests.Add(request2);
                context.Requests.Add(request3);
                context.Orders.Add(order1);
                context.Orders.Add(order2);

                context.SaveChanges();

                //create tested service
                var service = new OrderService(context);
                List<Order> orders = service.GetOrdersByClient(22).OrdersList;

                //assert - is number of client orders equal 1 
                Assert.AreEqual(1, orders.Count);
                //assert - check valid of retrieved data
                Assert.AreEqual("product2", orders[0].Requests[0].Name);

            }
        }

        [TestMethod]
        public void Can_Get_Orders_Quantity()
        {
            //create db context
            using (var context = new RequestContext(CreateNewContextOptions()))
            {
                //create requests to add to db
                Request request1 = new Request
                {
                    Id = 2222,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 1,
                    Name = "product1"
                };
                Request request2 = new Request
                {
                    Id = 3333,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 2,
                    ClientId = 22,
                    Name = "product2"
                };
                Request request3 = new Request
                {
                    Id = 3334,
                    Price = 20,
                    Quantity = 12,
                    RequestId = 1,
                    ClientId = 22,
                    Name = "product3"
                };

                //create orders to add to db
                Order order1 = new Order
                {
                    OrderId = 1,
                    ClientId = request1.ClientId,
                    RequestId = request1.RequestId,
                    Amount = request1.Price * request1.Quantity
                };
                Order order2 = new Order
                {
                    OrderId = 2,
                    ClientId = request2.ClientId,
                    RequestId = request2.RequestId,
                    Amount = request2.Price * request2.Quantity
                };
                Order order3 = new Order
                {
                    OrderId = 3,
                    ClientId = request3.ClientId,
                    RequestId = request3.RequestId,
                    Amount = request3.Price * request3.Quantity
                };


                //assignment orders to requests
                request1.Order = order1;
                request2.Order = order2;
                request3.Order = order3;

                //add requests and orders to database
                context.Requests.Add(request1);
                context.Requests.Add(request2);
                context.Requests.Add(request3);
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);

                context.SaveChanges();

                //create tested service
                var service = new OrderService(context);

                //action - call tested method
                int ordersQuantity = service.OrdersCount();

                //assert - is number of  orders equal 3
                Assert.AreEqual(3, ordersQuantity);
            }
        }

        [TestMethod]
        public void Can_Get_Client_Orders_Quantity()
        {
            //create db context
            using (var context = new RequestContext(CreateNewContextOptions()))
            {
                //create requests to add to db
                Request request1 = new Request
                {
                    Id = 2222,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 1,
                    Name = "product1"
                };
                Request request2 = new Request
                {
                    Id = 3333,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 2,
                    ClientId = 22,
                    Name = "product2"
                };
                Request request3 = new Request
                {
                    Id = 3334,
                    Price = 20,
                    Quantity = 12,
                    RequestId = 1,
                    ClientId = 22,
                    Name = "product3"
                };

                //create orders to add to db
                Order order1 = new Order
                {
                    OrderId = 1,
                    ClientId = request1.ClientId,
                    RequestId = request1.RequestId,
                    Amount = request1.Price * request1.Quantity
                };
                Order order2 = new Order
                {
                    OrderId = 2,
                    ClientId = request2.ClientId,
                    RequestId = request2.RequestId,
                    Amount = request2.Price * request2.Quantity
                };
                Order order3 = new Order
                {
                    OrderId = 3,
                    ClientId = request3.ClientId,
                    RequestId = request3.RequestId,
                    Amount = request3.Price * request3.Quantity
                };


                //assignment orders to requests
                request1.Order = order1;
                request2.Order = order2;
                request3.Order = order3;

                //add requests and orders to database
                context.Requests.Add(request1);
                context.Requests.Add(request2);
                context.Requests.Add(request3);
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);

                context.SaveChanges();

                //create tested service
                var service = new OrderService(context);

                //action - call tested method
                int ordersQuantity = service.OrdersCountByClient(22);

                //assert - is number of client orders equal 2
                Assert.AreEqual(2, ordersQuantity);
            }
        }

        [TestMethod]
        public void Can_Get_Orders_Total_Amount()
        {

        }

        [TestMethod]
        public void Can_Get_Client_Orders_Total_Amount()
        {

        }

        [TestMethod]
        public void Can_Get_Orders_Average_Amount()
        {
                  //create db context
            using (var context = new RequestContext(CreateNewContextOptions()))
            {
                //create requests to add to db
                Request request1 = new Request
                {
                    Id = 2222,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 1,
                    Name = "product1"
                };
                Request request2 = new Request
                {
                    Id = 3333,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 2,
                    ClientId = 22,
                    Name = "product2"
                };
                Request request3 = new Request
                {
                    Id = 3334,
                    Price = 20,
                    Quantity = 12,
                    RequestId = 1,
                    ClientId = 22,
                    Name = "product3"
                };

                //create orders to add to db
                Order order1 = new Order
                {
                    OrderId = 1,
                    ClientId = request1.ClientId,
                    RequestId = request1.RequestId,
                    Amount = request1.Price * request1.Quantity
                };
                Order order2 = new Order
                {
                    OrderId = 2,
                    ClientId = request2.ClientId,
                    RequestId = request2.RequestId,
                    Amount = request2.Price * request2.Quantity
                };
                Order order3 = new Order
                {
                    OrderId = 3,
                    ClientId = request3.ClientId,
                    RequestId = request3.RequestId,
                    Amount = request3.Price * request3.Quantity
                };


                //assignment orders to requests
                request1.Order = order1;
                request2.Order = order2;
                request3.Order = order3;

                //add requests and orders to database
                context.Requests.Add(request1);
                context.Requests.Add(request2);
                context.Requests.Add(request3);
                context.Orders.Add(order1);
                context.Orders.Add(order2);
                context.Orders.Add(order3);

                context.SaveChanges();

                //create tested service
                var service = new OrderService(context);

                //action - call tested method
                double ordersAvrgAmnt = service.OrdersAverageAmount();

                //assert - is average amount of orders equal 260/3
                Assert.AreEqual((double)260/(double)3, ordersAvrgAmnt);
            }
        }

        [TestMethod]
        public void Can_Get_Client_Orders_Average_Amount()
        {

        }

        [TestMethod]
        public void Can_Get_Orders_Price_Range()
        {

        }

        [TestMethod]
        public void Can_Get_Nr_Of_Product_Ordrs()
        {

        }

        [TestMethod]
        public void Can_Get_Nr_Of_Product_Ordrs_By_Client()
        {

        }

        private static DbContextOptions<RequestContext> CreateNewContextOptions()
        {
            // The key to keeping the databases unique and not shared is 
            // generating a unique db name for each.
            string dbName = Guid.NewGuid().ToString();

            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<RequestContext>();
            builder.UseInMemoryDatabase(dbName)
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
