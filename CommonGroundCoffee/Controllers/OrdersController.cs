using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace CommonGroundCoffee.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService ordersService;
        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await ordersService.GetOrdersAsync();
            return View(orders);
        }
    }
}
