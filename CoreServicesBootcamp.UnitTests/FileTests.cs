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
using System.Xml.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using CoreServicesBootcamp.BLL.Entities;
using CsvHelper;

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class FileTests
    {
        [TestMethod]
        public void Can_Load_Json_File()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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

            formFiles.Add(fileMock.Object);

            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IEnumerable<IFileService>> mockFileServices = new Mock<IEnumerable<IFileService>>();
            IEnumerable<IFileService> services = new List<IFileService>
            {
                new JsonService(context),
                new XmlService(context),
                new CsvService(context)
            };

            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Json))
                .Returns(true);

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(mockStrategy.Object);

            ViewResult result = (ViewResult)fileController.FilesLoad(formFiles);

            int succeededFiles = (int)result.ViewData["SuccessCount"];

            Assert.AreEqual(1, succeededFiles);
        }

        [TestMethod]
        public void Cannot_Load_Not_Json_File()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("application/pdf");
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

            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Json))
                .Returns(true);

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(mockStrategy.Object);

            ViewResult result = (ViewResult)fileController.FilesLoad(formFiles);

            var succeededFiles = result.ViewData["SuccessCount"];
            var wasFailure = result.ViewData["WasFailure"];

            Assert.AreEqual(0, succeededFiles);
            Assert.AreEqual(true, wasFailure);
        }

        [TestMethod]
        public void Can_Load_Xml_File()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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
                requestXmlTest = sww.ToString(); // XML file
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

            formFiles.Add(fileMock.Object);

            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IEnumerable<IFileService>> mockFileServices = new Mock<IEnumerable<IFileService>>();
            IEnumerable<IFileService> services = new List<IFileService>
            {
                new JsonService(context),
                new XmlService(context),
                new CsvService(context)
            };

            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Xml))
                .Returns(true);

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(mockStrategy.Object);

            ViewResult result = (ViewResult)fileController.FilesLoad(formFiles);
            
            int succeededFiles = (int)result.ViewData["SuccessCount"];

            Assert.AreEqual(1, succeededFiles);
        }

        [TestMethod]
        public void Cannot_Load_Not_Xml_File()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("application/pdf");
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

            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Xml))
                .Returns(true);

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(mockStrategy.Object);

            ViewResult result = (ViewResult)fileController.FilesLoad(formFiles);

            var succeededFiles = result.ViewData["SuccessCount"];
            var wasFailure = result.ViewData["WasFailure"];

            Assert.AreEqual(0, succeededFiles);
            Assert.AreEqual(true, wasFailure);
        }

        [TestMethod]
        public void Can_Load_Csv_File()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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
            fileMock.Setup(_ => _.ContentType).Returns("application/vnd.ms-excel");
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

            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Csv))
                .Returns(true);

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(mockStrategy.Object);

            ViewResult result = (ViewResult)fileController.FilesLoad(formFiles);

            int succeededFiles = (int)result.ViewData["SuccessCount"];

            Assert.AreEqual(1, succeededFiles);
        }

        [TestMethod]
        public void Cannot_Load_Not_Csv_File()
        {
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.ContentType).Returns("application/pdf");
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

            Mock<IFileStrategy> mockStrategy = new Mock<IFileStrategy>();
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Csv))
                .Returns(true);

            //target - create app controller, whose method will be tested
            FileController fileController = new FileController(mockStrategy.Object);

            ViewResult result = (ViewResult)fileController.FilesLoad(formFiles);

            var succeededFiles = result.ViewData["SuccessCount"];
            var wasFailure = result.ViewData["WasFailure"];

            Assert.AreEqual(0, succeededFiles);
            Assert.AreEqual(true, wasFailure);
        }

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
            mockStrategy.Setup(m => m.LoadToDb(fileMock.Object, FileExtension.Json))
                .Returns(true);

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
            //preparation - create context with created options passed as argument
            RequestContext context = new RequestContext(CreateNewContextOptions());

            List<IFormFile> formFiles = new List<IFormFile>();

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
