using CommonGroundCoffee.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICafeSearchService
    {
        Task<List<CafeResult>> SearchCafesAsync(
            string location,
            int radiusMiles,
            List<string> purchasedProducts,
            string? manualPreferences);
    }
}