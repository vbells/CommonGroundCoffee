using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Orders order) =>
            await _context.Orders.AddAsync(order);

        public async Task AddOrderItemAsync(Order_Items item) =>
            await _context.Order_Items.AddAsync(item);

        public async Task<IEnumerable<Orders>> GetOrdersAsync() =>
            await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<Orders>> GetOrdersByCustomerIdAsync(int customerId) =>
            await _context.Orders
                .Where(o => o.Customer_ID == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<Order_Items>> GetOrderItemsByOrderIdAsync(int orderId) =>
            await _context.Order_Items
                .Include(oi => oi.Product)
                .Where(oi => oi.Order_ID == orderId)
                .ToListAsync();

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();

        public async Task<IEnumerable<string>> GetPurchasedProductNamesByCustomerIdAsync(int customerId)
        {
            return await _context.Order_Items
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.Customer_ID == customerId)
                .Select(oi => oi.Product.Product_Name)
                .Distinct()
                .ToListAsync();
        }
    }
}