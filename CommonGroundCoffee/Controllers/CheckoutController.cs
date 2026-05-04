using BusinessLogicLayer;
using CommonGroundCoffee.ViewModels;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommonGroundCoffee.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly ICartService _cartService;
        private readonly ICustomerRepository _customerRepository;

        public CheckoutController(IOrdersService ordersService,
                                  ICartService cartService,
                                  ICustomerRepository customerRepository)
        {
            _ordersService = ordersService;
            _cartService = cartService;
            _customerRepository = customerRepository;
        }

        // GET /Checkout — show order summary before checkout
        public async Task<IActionResult> Index()
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            var model = new CheckoutViewModel();

            // If logged in, pre-fill from customer data
            var customerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(customerIdClaim) && int.TryParse(customerIdClaim, out var customerId))
            {
                var customer = await _customerRepository.GetByIdAsync(customerId);
                if (customer != null)
                {
                    model.FirstName = customer.FirstName;
                    model.LastName = customer.LastName;
                    model.Email = customer.Email;
                    model.PhoneNumber = customer.PhoneNumber;
                    model.StreetAddress = customer.StreetAddress;
                    model.City = customer.City;
                    model.State = customer.State;
                    model.ZipCode = customer.ZipCode;
                    model.UseExistingAddress = true;
                }
            }

            return View(model);
        }

        // POST /Checkout/PlaceOrder — save order to database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            if (!ModelState.IsValid)
                return View("Index", model);

            // Get customer ID if logged in
            var customerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? customerId = null;

            if (!string.IsNullOrWhiteSpace(customerIdClaim) && int.TryParse(customerIdClaim, out var id))
            {
                customerId = id;
            }

            // If logged in and using existing address, load it
            if (customerId.HasValue && model.UseExistingAddress)
            {
                var customer = await _customerRepository.GetByIdAsync(customerId.Value);
                if (customer != null)
                {
                    model.StreetAddress = customer.StreetAddress;
                    model.City = customer.City;
                    model.State = customer.State;
                    model.ZipCode = customer.ZipCode;
                }
            }

            // Process payment (mock — in production use Stripe/PayPal)
            bool paymentSuccess = ProcessPayment(model);
            if (!paymentSuccess)
            {
                ModelState.AddModelError("", "Payment failed. Please check your card information.");
                return View("Index", model);
            }

            // Create order if user is logged in
            if (customerId.HasValue)
            {
                var success = await _ordersService.CheckoutAsync(customerId.Value);
                if (!success)
                {
                    ModelState.AddModelError("", "Checkout failed. Some items may be out of stock.");
                    return View("Index", model);
                }
            }
            else
            {
                // For guest users, just clear cart (no order saved without account)
                _cartService.ClearCart();
            }

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("Confirmation");
        }

        // GET /Checkout/Confirmation
        public IActionResult Confirmation()
        {
            return View();
        }

        // Mock payment processing
        private bool ProcessPayment(CheckoutViewModel model)
        {
            var cardNum = model.CardNumber;

            if (string.IsNullOrWhiteSpace(cardNum))
                return false;

            // Remove spaces/dashes
            var cleaned = new string(cardNum.Where(char.IsDigit).ToArray());

            // Basic length check
            return cleaned.Length >= 13 && cleaned.Length <= 19;
        }

        // GET /Checkout/OrderHistory
        public async Task<IActionResult> OrderHistory()
        {
            var customerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(customerIdClaim) ||
                !int.TryParse(customerIdClaim, out var customerId))
            {
                return RedirectToAction("Login", "Account");
            }

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