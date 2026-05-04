using BusinessLogicLayer;
using BusinessLogicLayer.Interfaces;
using CommonGroundCoffee.ViewModels;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace CommonGroundCoffee.Controllers
{
    [Authorize]
    public class CafeController : Controller
    {
        private readonly ICafeSearchService _cafeSearchService;
        private readonly IOrderRepository _orderRepository;

        public CafeController(ICafeSearchService cafeSearchService,
                               IOrderRepository orderRepository)
        {
            _cafeSearchService = cafeSearchService;
            _orderRepository = orderRepository;
        }

        // GET /Cafe
        public async Task<IActionResult> Index()
        {
            var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var purchasedProducts = await _orderRepository
                .GetPurchasedProductNamesByCustomerIdAsync(customerId);

            var model = new CafesNearMeViewModel
            {
                PurchasedProducts = purchasedProducts.ToList(),
                RadiusMiles = 10,
                UsePurchaseHistory = true
            };

            return View(model);
        }

        // POST /Cafe/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(CafesNearMeViewModel model)
        {
            var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var purchasedProducts = await _orderRepository
                .GetPurchasedProductNamesByCustomerIdAsync(customerId);

            model.PurchasedProducts = purchasedProducts.ToList();
            model.SearchPerformed = true;

            // Determine location
            string location;
            if (!string.IsNullOrWhiteSpace(model.Address))
            {
                location = model.Address;
            }
            else if (model.Latitude.HasValue && model.Longitude.HasValue)
            {
                location = $"{model.Latitude}, {model.Longitude}";
            }
            else
            {
                model.ErrorMessage = "Please enter an address or allow location access.";
                return View("Index", model);
            }

            if (!ModelState.IsValid)
                return View("Index", model);

            // ← KEY: Only pass purchased products if checkbox is checked
            var preferencesToUse = model.UsePurchaseHistory
                ? purchasedProducts.ToList()
                : new List<string>();

            model.Results = await _cafeSearchService.SearchCafesAsync(
                location,
                model.RadiusMiles,
                preferencesToUse,
                model.ManualPreferences);

            if (model.Results.Count == 0)
                model.ErrorMessage = "No cafes found. Try expanding your search radius.";

            return View("Index", model);
        }
    }
}