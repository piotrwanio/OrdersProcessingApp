using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CoreServicesBootcamp.BLL.Implementation
{
    public class ProductService
    {
        private RequestContext _context;

        public ProductService(RequestContext context)
        {
            _context = context;
        }

        public ProductDTO GetProductsOrders()
        {
            ProductDTO productDTO = new ProductDTO();
            Dictionary<string, int> productOrdersCount = new Dictionary<string, int>();

            var requests = (from r in _context.Requests
                            select r).ToList();

            foreach(var rq in requests)
            {
                if (!productOrdersCount.Where(m=> m.Key == rq.Name).Any())
                {
                    productOrdersCount.Add(rq.Name, 1);
                }
                else
                {
                    productOrdersCount[rq.Name]++;
                }
            }

            productDTO.ProductOrders = productOrdersCount;
            return productDTO;
        }
    }
}
