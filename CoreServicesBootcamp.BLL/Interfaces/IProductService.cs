using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Interfaces
{
    public interface IProductService
    {
        ProductDTO GetProductsNumberOfOrders();
        ProductDTO GetProdNrOfOrdersByClient(int clientId);
    }
}
