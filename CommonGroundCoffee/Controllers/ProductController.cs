using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
namespace CommonGroundCoffee.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await productService.GetProductsAsync();
            return View(products);
        }

        //coffee page 
        public async Task<IActionResult> Coffee(string filter = "All")
        {
            var products = await productService.GetProductsAsync();
            var coffeeTypes = new[] { "Coffee", "Single Origin", "Blends" };
            var filtered = products.Where(p => coffeeTypes.Contains(p.Product_Type));

            if (filter != "All")
                filtered = filtered.Where(p => p.Product_Type == filter);

            ViewBag.Filter = filter;
            return View(filtered);
        }
        // merch page
        public async Task<IActionResult> Merch(string filter = "All")
        {
            var products = await productService.GetProductsAsync();
            var filtered = products.Where(p => p.Product_Type == "Merch" || p.Product_Type == "Other");

            if (filter != "All")
                filtered = filtered.Where(p => p.Product_Type == filter);

            ViewBag.Filter = filter;
            return View(filtered);
        }

        // get product details
        public async Task<IActionResult> Details(int id)
        {
            var products = await productService.GetProductsAsync();
            var product = products.FirstOrDefault(p => p.Product_ID == id);

            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
