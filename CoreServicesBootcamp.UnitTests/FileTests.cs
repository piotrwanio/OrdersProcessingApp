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
    public class FileTests
    {
        [TestMethod]
        public void Can_Load_Requests_JSON()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

            //arrange
            var fileMock = new Mock<IFormFile>();

            RequestsJson requests = new RequestsJson { Requests = new List<RequestJson>
            {
                new RequestJson {
                    ClientId = "1",
                    Name = "test",
                    Price = "2.2",
                    Quantity = "10",
                    RequestId = "1"
                },
                new RequestJson {
                    ClientId = "1",
                    Name = "test",
                    Price = "2.2",
                    Quantity = "100",
                    RequestId = "1"
                }
            }
            };
            var requestJsonTest = JsonConvert.SerializeObject(requests);

            //setup mock file using a memory stream
            var content = requestJsonTest;
            var fileName = "test.json";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("application/json");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            formFiles.Add(fileMock.Object);

            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IEnumerable<IFileService>> mockFileServices = new Mock<IEnumerable<IFileService>>();
            IEnumerable<IFileService> services = new List<IFileService>
            {
                new JsonService(context),
                new XmlService(context),
                new CsvService(context)
            };
            //mockFileServices.Setup(m => m.FirstOrDefault(It.IsAny<Expression<Func<IFileService, bool>>>()))
            //    .Returns(services.FirstOrDefault());

            FileStrategy fileStrategy = new FileStrategy(services);
            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(fileStrategy);

            fileController.FilesLoad(formFiles);

            var requestTest = (from r in context.Requests
                      where r.ClientId == 1
                      select r);
            var orderTest = (from o in context.Orders
                             where o.ClientId == 1
                             select o);

            Assert.AreEqual(2, requestTest.Count());
            Assert.AreEqual(1, orderTest.Count());
            Assert.AreEqual("test", requestTest.First().Name);
            Assert.AreEqual(242, (int)orderTest.First().Amount);

        }

        [TestMethod]
        public void Can_Load_Requests_XML()
        {

        }

        [TestMethod]
        public void Can_Load_Requests_CSV()
        {

        }

        [TestMethod]
        public void Can_Load_Invalid_Files()
        {

        }

        [TestMethod]
        public void Can_Load_Invalid_Requests()
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
