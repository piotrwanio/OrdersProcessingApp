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

            //get client viewmodel from viewdata
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

        }

        [TestMethod]
        public void Can_Get_Client_Orders_Amount()
        {

        }

        [TestMethod]
        public void Can_Get_Average_Orders_Amount()
        {

        }

        [TestMethod]
        public void Can_Get_Average_Client_Orders_Amount()
        {

        }

        [TestMethod]
        public void Can_Get_Price_Range_Orders()
        {

        }

        [TestMethod]
        public void Can_Get_Orders_Quantity()
        {

        }

        [TestMethod]
        public void Can_Get_Client_Orders_Quantity()
        {

        }

        [TestMethod]
        public void Can_Get_Orders_List()
        {

        }

        [TestMethod]
        public void Can_Get_Client_Orders_List()
        {

        }

        [TestMethod]
        public void Can_Get_Number_Of_Product_Orders()
        {

        }

        [TestMethod]
        public void Can_Get_Number_Of_Product_Orders_By_Client()
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
