using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;

namespace CommonGroundCoffee.Controllers
{
    public class CustomerController : Controller
    {
        // create relationship between application layer and dal - using bll 
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }
        // for the index view, call the get customers asyn method to populate the customers variate 
        // this getCustomersAsync methos is coming from the IEmployeeService in the bll
        public async Task<IActionResult> Index()
        {
            var customers = await customerService.GetCustomersAsync();
            return View(customers);
        }
    }
}
