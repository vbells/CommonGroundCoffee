using DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Orders order);
        Task AddOrderItemAsync(Order_Items item);
        Task<IEnumerable<Orders>> GetOrdersAsync();
        Task<IEnumerable<Orders>> GetOrdersByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order_Items>> GetOrderItemsByOrderIdAsync(int orderId);
        Task SaveChangesAsync();
        Task<IEnumerable<string>> GetPurchasedProductNamesByCustomerIdAsync(int customerId);
    }
}