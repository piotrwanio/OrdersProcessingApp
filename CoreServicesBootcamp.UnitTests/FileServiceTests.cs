using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Interfaces;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.UI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CoreServicesBootcamp.DAL.Entities;
using CsvHelper;
using CoreServicesBootcamp.BLL.Entities;

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class FileServiceTests
    {
        [TestMethod]
        public void Can_Load_Orders_From_Json()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            //arrange
            var fileMock = new Mock<IFormFile>();

            RequestsJson requests = new RequestsJson
            {
                Requests = new List<RequestJson>
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

            JsonService jsonService = new JsonService(context);

            jsonService.LoadToDb(fileMock.Object);

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
        public void Cannot_Load_Invalid_Json_Format()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            //arrange
            var fileMock = new Mock<IFormFile>();

            Request requests = new Request
            {
                Quantity = 222,
                OrderId = 2222
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

            JsonService jsonService = new JsonService(context);

            jsonService.LoadToDb(fileMock.Object);

            var requestTest = (from r in context.Requests
                               where r.ClientId == 1
                               select r);
            var orderTest = (from o in context.Orders
                             where o.ClientId == 1
                             select o);

            Assert.AreEqual(0, requestTest.Count());
            Assert.AreEqual(0, orderTest.Count());
        }

        [TestMethod]
        public void Can_Load_Orders_From_Xml()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            //arrange
            var fileMock = new Mock<IFormFile>();

            RequestsXml requests = new RequestsXml
            {
                Requests = new List<RequestXml>
            {
                new RequestXml {
                    ClientId = "1",
                    Name = "test",
                    Price = "2.2",
                    Quantity = "10",
                    RequestId = "1"
                },
                new RequestXml {
                    ClientId = "1",
                    Name = "test",
                    Price = "2.2",
                    Quantity = "100",
                    RequestId = "1"
                }
            }
            };

            XmlSerializer serializer = new XmlSerializer(typeof(RequestsXml));

            var requestXmlTest = "";

            using (var sww = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(sww))
            {
                serializer.Serialize(xmlWriter, requests);
                requestXmlTest = sww.ToString(); // Your XML
            }

            //setup mock file using a memory stream
            var content = requestXmlTest;
            var fileName = "test.xml";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("text/xml");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            XmlService xmlService = new XmlService(context);

            xmlService.LoadToDb(fileMock.Object);

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
        public void Cannot_Load_Invalid_Xml_Format()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            //arrange
            var fileMock = new Mock<IFormFile>();

            Request requests = new Request { RequestId = 22 };
     

            XmlSerializer serializer = new XmlSerializer(typeof(Request));

            var requestXmlTest = "";

            using (var sww = new StringWriter())
            using (XmlWriter xmlWriter = XmlWriter.Create(sww))
            {
                serializer.Serialize(xmlWriter, requests);
                requestXmlTest = sww.ToString(); // Your XML
            }

            //setup mock file using a memory stream
            var content = requestXmlTest;
            var fileName = "test.xml";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("text/xml");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            XmlService xmlService = new XmlService(context);

            xmlService.LoadToDb(fileMock.Object);

            var requestTest = (from r in context.Requests
                               where r.ClientId == 1
                               select r);
            var orderTest = (from o in context.Orders
                             where o.ClientId == 1
                             select o);

            Assert.AreEqual(0, requestTest.Count());
            Assert.AreEqual(0, orderTest.Count());
        }

        [TestMethod]
        public void Can_Load_Orders_From_Csv()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            //arrange
            var fileMock = new Mock<IFormFile>();

            List<RequestCsv> requests = new List<RequestCsv>
            {
                new RequestCsv {
                    Client_Id = "1",
                    Name = "test",
                    Price = "2.2",
                    Quantity = "10",
                    Request_id = "1"
                },
                new RequestCsv {
                    Client_Id = "1",
                    Name = "test",
                    Price = "2.2",
                    Quantity = "100",
                    Request_id = "1"
                }
            };

            StringWriter stringWriter = new StringWriter();
            string requestsCsvTest = "";

            using (var csv = new CsvWriter(stringWriter))
            {
                csv.Configuration.Delimiter = ",";
                csv.WriteRecords(requests);
                requestsCsvTest = stringWriter.ToString();
            }

            //setup mock file using a memory stream
            var content = requestsCsvTest;
            var fileName = "test.csv";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("application / vnd.ms - excel");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            CsvService csvService = new CsvService(context);

            csvService.LoadToDb(fileMock.Object);

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
        public void Cannot_Load_Invalid_Csv_Format()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            //arrange
            var fileMock = new Mock<IFormFile>();

            List<RequestJson> requests = new List<RequestJson>
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
            };

            StringWriter stringWriter = new StringWriter();
            string requestsCsvTest = "";

            using (var csv = new CsvWriter(stringWriter))
            {
                csv.Configuration.Delimiter = ",";
                csv.WriteRecords(requests);
                requestsCsvTest = stringWriter.ToString();
            }

            //setup mock file using a memory stream
            var content = requestsCsvTest;
            var fileName = "test.csv";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("application / vnd.ms - excel");
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            CsvService csvService = new CsvService(context);

            csvService.LoadToDb(fileMock.Object);

            var requestTest = (from r in context.Requests
                               where r.ClientId == 1
                               select r);
            var orderTest = (from o in context.Orders
                             where o.ClientId == 1
                             select o);

            Assert.AreEqual(0, requestTest.Count());
            Assert.AreEqual(0, orderTest.Count());
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
