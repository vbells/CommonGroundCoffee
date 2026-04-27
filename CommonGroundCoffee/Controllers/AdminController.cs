using BusinessLogicLayer;
using BusinessLogicLayer.ViewModels;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CommonGroundCoffee.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // IProductService to get/fetch products from db
        private readonly IProductService _productService;
        private readonly ApplicationDbContext _context;

        public AdminController(IProductService productService, ApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        // home page for admin dashboard
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount),
                RecentOrders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToListAsync(),
                LowStockProducts = await _context.Products
                    .Where(p => p.Quantity_Available <= 10)
                    .OrderBy(p => p.Quantity_Available)
                    .ToListAsync()
            };

            return View(viewModel);
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
