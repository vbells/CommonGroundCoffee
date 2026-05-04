using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;

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

        public async Task AddToCart(int productId, int quantity)
        {
            var product = await _productService.GetByIdAsync(productId);
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

        
        public void UpdateQuantity(int productId, string direction)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.Product_ID == productId);
            if (item == null) return;

            if (direction == "up")
            {
                item.Quantity++;
            }
            else if (direction == "down")
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                    cart.RemoveItem(productId); // auto remove if hits 0
            }

            SaveCart(cart);
        }

        private void SaveCart(Cart cart)
        {
            _httpContextAccessor.HttpContext!.Session.SetObject(CartSessionKey, cart);
        }
        public void ClearCart()
        {
            _httpContextAccessor.HttpContext!.Session.Remove(CartSessionKey);
        }
    }
}