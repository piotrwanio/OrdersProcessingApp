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

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class AppTests
    {
        [TestMethod]
        public void Index_Contains_All_Requests()
        {

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
