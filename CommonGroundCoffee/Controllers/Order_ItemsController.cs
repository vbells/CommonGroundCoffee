using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace CommonGroundCoffee.Controllers
{
    public class Order_ItemsController : Controller
    {
        private readonly IOrder_ItemsService order_ItemsService;

        public Order_ItemsController(IOrder_ItemsService order_ItemsService)
        {
            this.order_ItemsService = order_ItemsService;
        }

        public async Task<IActionResult> Index()
            {
                var order_Items = await order_ItemsService.GetOrder_ItemsAsync();
                return View(order_Items);
        }
    }
}
