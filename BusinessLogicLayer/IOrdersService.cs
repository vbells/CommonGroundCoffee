using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public interface IOrdersService
    {
        Task<IEnumerable<Orders>> GetOrdersAsync();
        Task<bool> CheckoutAsync(int customerId);
        Task<IEnumerable<Orders>> GetOrderHistoryAsync(int customerId);
        Task<IEnumerable<Order_Items>> GetOrderItemsAsync(int orderId);

    }
}
