using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using DataAccessLayer.Entities;

namespace CommonGroundCoffee.Controllers
{
    public class AdminController : Controller
    {
        // IProductService to get/fetch products from db
        private readonly IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        // home page for admin dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // manage products page
        public async Task<IActionResult> Products()
        {
            var products = await _productService.GetProductsAsync();
            return View(products);
        }

        // form to add new product
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction("Products");
            }
            return View(product);
        }
        // form to edit existing product
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProductAsync(product);
                return RedirectToAction("Products");
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction("Products");
        }
    }
}
