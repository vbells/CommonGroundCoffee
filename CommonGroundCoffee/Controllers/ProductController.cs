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
