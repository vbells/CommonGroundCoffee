using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace CommonGroundCoffee.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            _cartService.AddToCart(productId, quantity);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            _cartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }
    }
}
