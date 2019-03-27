using CoreServicesBootcamp.BLL.Implementation;
using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using CoreServicesBootcamp.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CoreServicesBootcamp.UnitTests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Can_Get_All_Orders()
        {
            //preparation - create dbcontext options with in-memory db
            var options = new DbContextOptionsBuilder<RequestContext>()
               .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
               .Options;

            //adding order to db
            using (var context = new RequestContext(options))
            {
                Request request = new Request
                {
                    Id = 1,
                    Price = 10,
                    Quantity = 1,
                    RequestId = 1,
                    ClientId = 1,
                    Name = "product1"
                };
                context.Add(request);
                context.SaveChanges();
            }


            //use context with prepared options to check if order was added
            using (var context = new RequestContext(options))
            {
                var service = new OrderService(context);
                List<Order> orders = service.GetAllOrders().Orders;

                //assert - is quantity of orders equal 1 
                Assert.AreEqual(1, orders.Count);
                //assert - check correct of insert data
                Assert.AreEqual("product1", orders[0].RequestsList[0].Name);

            }
        }
    }
}
