using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CoreServicesBootcamp.BLL.Interfaces;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class ProductService : IProductService
    {
        private RequestContext _context;

        public ProductService(RequestContext context)
        {
            _context = context;
        }

        public ProductDTO GetProdNrOfOrdersByClient(int clientId)
        {
            ProductDTO productDTO = new ProductDTO();
            Dictionary<string, int> productOrdersCount = new Dictionary<string, int>();
            List<int> orderList = new List<int>();

            var requests = (from r in _context.Requests
                            where r.ClientId == clientId
                            select r).ToList();

            foreach (var rq in requests)
            {
                if (!productOrdersCount.Where(m => m.Key == rq.Name + " Cena: " + rq.Price).Any())
                {
                    productOrdersCount.Add(rq.Name + " Cena: " + rq.Price, 1);
                    orderList.Add(rq.OrderId);
                }
                else
                {
                    if (!orderList.Contains(rq.OrderId))
                        productOrdersCount[rq.Name + " Cena: " + rq.Price]++;
                }
            }

            productDTO.ProductOrders = productOrdersCount;
            return productDTO;
        }

        public ProductDTO GetProductsNumberOfOrders()
        {
            ProductDTO productDTO = new ProductDTO();
            Dictionary<string, int> productOrdersCount = new Dictionary<string, int>();
            List<int> orderList = new List<int>();

            var requests = (from r in _context.Requests
                            select r).ToList();

            foreach(var rq in requests)
            {
                if (!productOrdersCount.Where(m => m.Key == rq.Name + " Cena: " + rq.Price).Any())
                {
                    productOrdersCount.Add(rq.Name + " Cena: " + rq.Price, 1);
                    orderList.Add(rq.OrderId);
                }
                else
                {
                    if(!orderList.Contains(rq.OrderId))
                    productOrdersCount[rq.Name + " Cena: " + rq.Price]++;
                }
            }

            productDTO.ProductOrders = productOrdersCount;
            return productDTO;
        }
    }
}
