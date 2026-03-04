using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;

        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor,
                           IProductService productService)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            var cart = session.GetObject<Cart>(CartSessionKey);

            if (cart == null)
            {
                cart = new Cart();
                session.SetObject(CartSessionKey, cart);
            }

            return cart;
        }

        public void AddToCart(int productId, int quantity)
        {
            var product = _productService.GetById(productId);
            if (product == null) return;

            var cart = GetCart();

            cart.AddItem(new CartItem
            {
                Product_ID = product.Product_ID,
                Product_Name = product.Product_Name,
                Price = product.Unit_Price,
                Quantity = quantity
            });

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveItem(productId);
            SaveCart(cart);
        }

        private void SaveCart(Cart cart)
        {
            _httpContextAccessor.HttpContext!.Session
                .SetObject(CartSessionKey, cart);
        }
    }
}
