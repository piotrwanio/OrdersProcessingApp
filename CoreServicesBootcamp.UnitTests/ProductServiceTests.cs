using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class ProductServiceTests
    {
        [TestMethod]
        public void Can_Get_Nr_Of_Product_Orders()
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
                var service = new ProductService(context);

                //action - call tested method
                ProductDTO ordersQuantity = service.GetProductsNumberOfOrders();

                //assert - is number of products equal 3
                Assert.AreEqual(3, ordersQuantity.ProductOrders.Count);
            }
        }

        [TestMethod]
        public void Can_Get_Nr_Of_Product_Orders_By_Client()
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
                    ClientId = 2,
                    Name = "product1"
                };
                Request request2 = new Request
                {
                    Id = 3333,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 2,
                    ClientId = 2,
                    Name = "product1"
                };
                Request request3 = new Request
                {
                    Id = 3334,
                    Price = 20,
                    Quantity = 12,
                    RequestId = 3,
                    ClientId = 2,
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
                var service = new ProductService(context);

                //action - call tested method
                ProductDTO ordersQuantity = service.GetProdNrOfOrdersByClient(2);

                //assert - is number of products for client equal 2
                Assert.AreEqual(2, ordersQuantity.ProductOrders.Count);
            }
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
