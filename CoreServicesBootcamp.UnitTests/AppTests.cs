using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using CoreServicesBootcamp.UI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using CoreServicesBootcamp.UI.Models;

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void Index_Contains_All_Clients()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetAllOrders()).Returns(new OrderDTO
            {
                OrdersList = new List<Order>
                {
                    new Order
                    {
                        OrderId = 1,
                        ClientId = 1,
                        RequestId = 1,
                        Requests = new List<Request>()
                    },
                    new Order
                    {
                        OrderId = 2,
                        ClientId = 2,
                        RequestId = 1,
                        Requests = new List<Request>()
                    }
                }
            });
            
            //preparation - creating controller
            AppController appController = new AppController(mock.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)appController.Index();

            //get client viewmodel from ViewResult
            ClientsViewModel clients = (ClientsViewModel)result.ViewData.Model;

            //assert - check if client quantity is equal 2
            Assert.AreEqual(2, clients.ClientList.Count);
            //asserts - check dropdown list values
            Assert.AreEqual("1", clients.ClientList[0].Value);
            Assert.AreEqual("2", clients.ClientList[1].Value);
        }

        [TestMethod]
        public void Can_Get_Orders_Amount()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.OrdersTotalAmount()).Returns(222.2);

            Mock<IProductService> mockProductService = new Mock<IProductService>();

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.OrdersTotalAmount();

            //get number of orders from ViewBag
            double ordersQuantity = (double)result.ViewData["TotalAmount"];

            //assert - check if orders total amount is equal 33
            Assert.AreEqual(222.2, ordersQuantity);
        }

        [TestMethod]
        public void Can_Get_Client_Orders_Amount()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.OrdersTotalAmountByClient(3)).Returns(333.1);

            Mock<IProductService> mockProductService = new Mock<IProductService>();

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.OrdersTotalAmountByClient(3);

            //get number of orders from ViewBag
            double clientAmount = (double)result.ViewData["TotalAmount"];

            //assert - check if client orders amount is equal 333.1
            Assert.AreEqual(333.1, clientAmount);
        }

        [TestMethod]
        public void Can_Get_Average_Orders_Amount()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.OrdersAverageAmount()).Returns(33.4);

            Mock<IProductService> mockProductService = new Mock<IProductService>();

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.OrdersAverageAmount();

            //get number of orders from ViewBag
            double ordersAverageAmount = (double)result.ViewData["AverageAmount"];

            //assert - check if orders average amount is equal 33.4
            Assert.AreEqual(33.4, ordersAverageAmount);
        }

        [TestMethod]
        public void Can_Get_Average_Client_Orders_Amount()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.OrdersAverageAmountByClient(2)).Returns(323.34);

            Mock<IProductService> mockProductService = new Mock<IProductService>();

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.OrdersAverageAmountByClient(2);

            //get number of orders from ViewBag
            double clientOrdersAvrgAmount = (double)result.ViewData["AverageAmount"];

            //assert - check if client orders average amount is equal 33.4
            Assert.AreEqual(323.34, clientOrdersAvrgAmount);
        }

        [TestMethod]
        public void Can_Get_Price_Range_Orders()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> orderServiceMock = new Mock<IOrderService>();
            Mock<IProductService> produckServiceMock = new Mock<IProductService>();

            orderServiceMock.Setup(m => m.OrdersPriceRange(0, 70)).Returns(new OrderDTO
            {
                OrdersList = new List<Order>
                {
                    new Order
                    {
                        OrderId = 1,
                        ClientId = 1,
                        Amount = 70,
                        RequestId = 1,
                        Requests = new List<Request>()
                    },
                    new Order
                    {
                        OrderId = 2,
                        Amount = 22,
                        ClientId = 2,
                        RequestId = 1,
                        Requests = new List<Request>()
                    }
                }
            });

            //preparation - creating controller
            OrderController orderController = new OrderController(orderServiceMock.Object,
                produckServiceMock.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.OrdersPriceRange(0, 70, null);

            //get client viewmodel from ViewResult
            OrderDTO orders = (OrderDTO)result.ViewData.Model;

            //assert - check if price range orders quantity is equal 2
            Assert.AreEqual(2, orders.OrdersList.Count);
            //asserts - check if PR orders in model are correct
            Assert.AreEqual(70, orders.OrdersList[0].Amount);
            Assert.AreEqual(22, orders.OrdersList[1].Amount);
        }

        [TestMethod]
        public void Can_Get_Orders_Quantity()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.OrdersCount()).Returns(4);

            Mock<IProductService> mockProductService = new Mock<IProductService>();

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.OrdersQuantity();

            //get number of orders from ViewBag
            int ordersQuantity = (int)result.ViewData["NumberOfOrders"];

            //assert - check if number of orders is equal 4
            Assert.AreEqual(4, ordersQuantity);
        }

        [TestMethod]
        public void Can_Get_Client_Orders_Quantity()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.OrdersCountByClient(1)).Returns(3);

            Mock<IProductService> mockProductService = new Mock<IProductService>();

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.ClientOrdersQuantity(1);

            //get number of orders from ViewBag
            int clientOrdersQuantity = (int)result.ViewData["NumberOfOrders"];

            //assert - check if client's number of orders is equal 3
            Assert.AreEqual(3, clientOrdersQuantity);
        }

        [TestMethod]
        public void Can_Get_Orders_List()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> orderServiceMock = new Mock<IOrderService>();
            Mock<IProductService> produckServiceMock = new Mock<IProductService>();

            orderServiceMock.Setup(m => m.GetAllOrders()).Returns(new OrderDTO
            {
                OrdersList = new List<Order>
                {
                    new Order
                    {
                        OrderId = 1,
                        ClientId = 1,
                        RequestId = 1,
                        Requests = new List<Request>()
                    },
                    new Order
                    {
                        OrderId = 2,
                        ClientId = 2,
                        RequestId = 1,
                        Requests = new List<Request>()
                    }
                }
            });

            //preparation - creating controller
            OrderController orderController = new OrderController(orderServiceMock.Object,
                produckServiceMock.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.AllOrdersList(null);

            //get client viewmodel from ViewResult
            OrderDTO orders = (OrderDTO)result.ViewData.Model;

            //assert - check if orders quantity is equal 2
            Assert.AreEqual(2, orders.OrdersList.Count);
            //asserts - check if orders in model are correct
            Assert.AreEqual(1, orders.OrdersList[0].OrderId);
            Assert.AreEqual(2, orders.OrdersList[1].OrderId);
        }

        [TestMethod]
        public void Can_Get_Client_Orders_List()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> orderServiceMock = new Mock<IOrderService>();
            Mock<IProductService> produckServiceMock = new Mock<IProductService>();

            orderServiceMock.Setup(m => m.GetOrdersByClient(7)).Returns(new OrderDTO
            {
                OrdersList = new List<Order>
                {
                    new Order
                    {
                        OrderId = 1,
                        ClientId = 7,
                        RequestId = 1,
                        Requests = new List<Request>()
                    },
                    new Order
                    {
                        OrderId = 2,
                        ClientId = 7,
                        RequestId = 2,
                        Requests = new List<Request>()
                    }
                }
            });

            //preparation - creating controller
            OrderController orderController = new OrderController(orderServiceMock.Object,
                produckServiceMock.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.ClientOrdersList(null, 7);

            //get client viewmodel from ViewResult
            OrderDTO orders = (OrderDTO)result.ViewData.Model;

            //assert - check if client's orders quantity is equal 2
            Assert.AreEqual(2, orders.OrdersList.Count);
            //asserts - check if client's orders in model are correct
            Assert.AreEqual(1, orders.OrdersList[0].OrderId);
            Assert.AreEqual(2, orders.OrdersList[1].OrderId);
        }

        [TestMethod]
        public void Can_Get_Number_Of_Product_Orders()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();

            Mock<IProductService> mockProductService = new Mock<IProductService>();
            mockProductService.Setup(m => m.GetProductsNumberOfOrders())
                .Returns(new ProductDTO
                {
                    ProductOrders = new Dictionary<string, int>()
                    {
                        { "Pierogi Cena: 20", 4 }
                    }
                });

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.ProductOrdersNumber();

            //get number of product orders from ViewResult
            ProductDTO productOrders = (ProductDTO)result.ViewData.Model;

            //assert - check if pierogi product is on the list
            Assert.AreEqual("Pierogi Cena: 20", productOrders.ProductOrders.Keys.First());
            //assert - check if number of pierogi's orders is equal 4
            Assert.AreEqual(4, productOrders.ProductOrders["Pierogi Cena: 20"]);
        }

        [TestMethod]
        public void Can_Get_Number_Of_Product_Orders_By_Client()
        {
            //preparation - creating imitation of OrderService
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();

            Mock<IProductService> mockProductService = new Mock<IProductService>();
            mockProductService.Setup(m => m.GetProdNrOfOrdersByClient(2))
                .Returns(new ProductDTO {
                    ProductOrders = new Dictionary<string, int>()
                    {
                        { "Pierogi Cena: 20", 2 }
                    }
                });

            //preparation - creating controller
            OrderController orderController = new OrderController(mockOrderService.Object,
                mockProductService.Object);

            //action - calling tested method
            ViewResult result = (ViewResult)orderController.ProductOrdersNrByClient(2);

            //get number of orders from ViewBag
            ProductDTO productOrders = (ProductDTO)result.ViewData.Model;

            //assert - check if pierogi product is on the list
            Assert.AreEqual("Pierogi Cena: 20", productOrders.ProductOrders.Keys.First());
            //assert - check if client's number of pierogi's orders is equal 2
            Assert.AreEqual(2, productOrders.ProductOrders["Pierogi Cena: 20"]);

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
