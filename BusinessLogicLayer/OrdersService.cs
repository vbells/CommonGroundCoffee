using DataAccessLayer.Interfaces;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICartService _cartService;

        public OrdersService(IOrderRepository orderRepo,
                             IProductRepository productRepo,
                             ICartService cartService)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _cartService = cartService;
        }

        public async Task<IEnumerable<Orders>> GetOrdersAsync()
        {
            return await _orderRepo.GetOrdersByCustomerIdAsync(0); // keep for admin use
        }

        public async Task<bool> CheckoutAsync(int customerId)
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any()) return false;

            // check inventory first before doing anything
            foreach (var item in cart.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.Product_ID);
                if (product == null || product.Quantity_Available < item.Quantity)
                    return false;
            }

            // create order
            var order = new Orders
            {
                Customer_ID = customerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.GetTotal()
            };

            await _orderRepo.AddOrderAsync(order);
            await _orderRepo.SaveChangesAsync(); // save now to get Order_ID

            // save order items and deduct inventory
            foreach (var item in cart.Items)
            {
                var orderItem = new Order_Items
                {
                    Order_ID = order.Order_ID,
                    Product_ID = item.Product_ID,
                    Quantity = item.Quantity,
                    Price_at_Purchase = item.Price
                };

                await _orderRepo.AddOrderItemAsync(orderItem);

                // deduct stock
                var product = await _productRepo.GetByIdAsync(item.Product_ID);
                product!.Quantity_Available -= item.Quantity;
                await _productRepo.UpdateProductAsync(product);
            }

            await _orderRepo.SaveChangesAsync();

            // clear cart
            _cartService.ClearCart();

            return true;
        }

        public async Task<IEnumerable<Orders>> GetOrderHistoryAsync(int customerId) =>
            await _orderRepo.GetOrdersByCustomerIdAsync(customerId);

        public async Task<IEnumerable<Order_Items>> GetOrderItemsAsync(int orderId) =>
            await _orderRepo.GetOrderItemsByOrderIdAsync(orderId);
    }
}