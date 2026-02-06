using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
namespace CommonGroundCoffee.Controllers
{
    public class PreferencesController : Controller
    {
        private readonly IPreferencesService preferencesService;

        public PreferencesController(IPreferencesService preferencesService)
        {
            this.preferencesService = preferencesService;
        }

        public async Task<IActionResult> Index()
        {
            var preferences = await preferencesService.GetPreferencesAsync();
            return View(preferences);
        }

    }
}
