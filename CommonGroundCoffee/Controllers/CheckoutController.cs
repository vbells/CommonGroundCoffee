using BusinessLogicLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommonGroundCoffee.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly ICartService _cartService;

        public CheckoutController(IOrdersService ordersService, ICartService cartService)
        {
            _ordersService = ordersService;
            _cartService = cartService;
        }

        // GET /Checkout — order summary before confirming
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Cart");
            return View(cart);
        }

        // POST /Checkout/PlaceOrder — save order to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {
            var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _ordersService.CheckoutAsync(customerId);

            if (!success)
            {
                TempData["ErrorMessage"] = "Checkout failed. Some items may be out of stock.";
                return RedirectToAction("Index", "Cart");
            }

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("Confirmation");
        }

        // GET /Checkout/Confirmation
        public IActionResult Confirmation() => View();

        // GET /Checkout/OrderHistory
        public async Task<IActionResult> OrderHistory()
        {
            var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var orders = await _ordersService.GetOrderHistoryAsync(customerId);
            return View(orders);
        }

        // GET /Checkout/OrderDetails/5
        public async Task<IActionResult> OrderDetails(int id)
        {
            var items = await _ordersService.GetOrderItemsAsync(id);
            return View(items);
        }
    }
}