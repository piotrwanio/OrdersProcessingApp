using CoreServicesBootcamp.BLL.Models;
using CoreServicesBootcamp.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServicesBootcamp.BLL.Interfaces
{
    public interface IOrderService
    {
        OrderDTO OrdersPriceRange(double min, double max);
        double OrdersAverageAmount();
        double OrdersAverageAmountByClient(int clientId);
        double OrdersTotalAmount();
        double OrdersTotalAmountByClient(int clientId);
        int OrdersCount();
        int OrdersCountByClient(int clientId);
        OrderDTO GetOrdersByClient(int clientId);
        OrderDTO GetAllOrders();

    }
}
