using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommonGroundCoffee.ViewModels
{
    public class CafesNearMeViewModel
    {
        // auto-populated from order history
        public List<string> PurchasedProducts { get; set; } = new();

        // manual preferences
        [Display(Name = "Additional Preferences (e.g. oat milk, quiet atmosphere, outdoor seating)")]
        public string? ManualPreferences { get; set; }

        // location
        [Display(Name = "Your Address")]
        public string? Address { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Required(ErrorMessage = "Please enter a distance.")]
        [Range(1, 100, ErrorMessage = "Distance must be between 1 and 100 miles.")]
        [Display(Name = "Search Radius (miles)")]
        public int RadiusMiles { get; set; } = 10;

        // results
        public List<CafeResult> Results { get; set; } = new();
        public bool SearchPerformed { get; set; } = false;
        public string? ErrorMessage { get; set; }
    }

    public class CafeResult
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Distance { get; set; } = string.Empty;
        public string WhyItMatches { get; set; } = string.Empty;
    }
}