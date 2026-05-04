using BusinessLogicLayer;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommonGroundCoffee.ViewModels
{
    public class CafesNearMeViewModel
    {
        public List<string> PurchasedProducts { get; set; } = new();

        [Display(Name = "Use my past coffee purchases as preferences")]
        public bool UsePurchaseHistory { get; set; } = true;

        [Display(Name = "Additional Preferences")]
        public string? ManualPreferences { get; set; }

        [Display(Name = "Your Address")]
        public string? Address { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Required(ErrorMessage = "Please enter a distance.")]
        [Range(1, 100, ErrorMessage = "Distance must be between 1 and 100 miles.")]
        [Display(Name = "Search Radius (miles)")]
        public int RadiusMiles { get; set; } = 10;

        public List<CafeResult> Results { get; set; } = new();
        public bool SearchPerformed { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }
}