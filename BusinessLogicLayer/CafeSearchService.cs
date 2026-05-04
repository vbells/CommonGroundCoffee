using BusinessLogicLayer.Interfaces;
using CommonGroundCoffee.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class CafeSearchService : ICafeSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CafeSearchService> _logger;

        public CafeSearchService(
            IHttpClientFactory httpClientFactory,
            ILogger<CafeSearchService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // 🌍 STEP 1: Geocode location -> lat/lon
        private async Task<(double lat, double lon)> GeocodeLocationAsync(string location)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "CommonGroundCoffee/1.0 (contact: support@commongroundcoffee.app)");

                var url =
                    $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(location)}&format=json&limit=1";

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Geocoding failed for {Location}. Status: {StatusCode}",
                        location, response.StatusCode);
                    return (0, 0);
                }

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);

                var root = doc.RootElement;

                // ✅ FIX: Proper array validation (fixes your bug)
                if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() == 0)
                {
                    _logger.LogWarning("No geocoding results found for {Location}", location);
                    return (0, 0);
                }

                var result = root[0];

                var lat = double.Parse(
                    result.GetProperty("lat").GetString() ?? "0",
                    CultureInfo.InvariantCulture);

                var lon = double.Parse(
                    result.GetProperty("lon").GetString() ?? "0",
                    CultureInfo.InvariantCulture);

                return (lat, lon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during geocoding for {Location}", location);
                return (0, 0);
            }
        }

        // ☕ STEP 2: Fetch cafes from OpenStreetMap (Overpass API)
        private async Task<List<CafeInfo>> FetchCafesFromOverpassAsync(
            double lat,
            double lon,
            int radiusMeters)
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "CommonGroundCoffee/1.0 (contact: support@commongroundcoffee.app)");

            var query = $@"
[out:json];
(
  node[""amenity""=""cafe""](around:{radiusMeters},{lat},{lon});
  way[""amenity""=""cafe""](around:{radiusMeters},{lat},{lon});
  relation[""amenity""=""cafe""](around:{radiusMeters},{lat},{lon});
);
out center;
";

            var content = new StringContent(query, Encoding.UTF8, "text/plain");

            var response = await client.PostAsync(
                "https://overpass-api.de/api/interpreter",
                content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Overpass API failed. Status: {StatusCode}", response.StatusCode);
                return new List<CafeInfo>();
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var cafes = new List<CafeInfo>();

            foreach (var element in doc.RootElement.GetProperty("elements").EnumerateArray())
            {
                double cafeLat = 0;
                double cafeLon = 0;

                if (element.TryGetProperty("lat", out var latProp))
                {
                    cafeLat = latProp.GetDouble();
                    cafeLon = element.GetProperty("lon").GetDouble();
                }
                else if (element.TryGetProperty("center", out var center))
                {
                    cafeLat = center.GetProperty("lat").GetDouble();
                    cafeLon = center.GetProperty("lon").GetDouble();
                }

                if (!element.TryGetProperty("tags", out var tags))
                    continue;

                var name = tags.TryGetProperty("name", out var nameProp)
                    ? nameProp.GetString()
                    : "Unnamed Cafe";

                cafes.Add(new CafeInfo
                {
                    Name = name ?? "Unnamed Cafe",
                    Address = BuildAddress(tags),
                    Latitude = cafeLat,
                    Longitude = cafeLon
                });
            }

            return cafes;
        }

        // 🏠 STEP 3: Build readable address
        private string BuildAddress(JsonElement tags)
        {
            var parts = new List<string>();

            if (tags.TryGetProperty("addr:housenumber", out var num))
                parts.Add(num.GetString());

            if (tags.TryGetProperty("addr:street", out var street))
                parts.Add(street.GetString());

            if (tags.TryGetProperty("addr:city", out var city))
                parts.Add(city.GetString());

            return parts.Any()
                ? string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)))
                : "Address unavailable";
        }

        // 📏 STEP 4: Distance calculation (Haversine)
        private double CalculateDistanceMiles(
            double lat1,
            double lon1,
            double lat2,
            double lon2)
        {
            const double R = 3958.8;

            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) *
                Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double DegreesToRadians(double deg) =>
            deg * 0.017453292519943295;

        // 🚀 MAIN SEARCH METHOD
        public async Task<List<CafeResult>> SearchCafesAsync(
            string location,
            int radiusMiles,
            List<string> purchasedProducts,
            string? manualPreferences)
        {
            try
            {
                var coffeeProducts = purchasedProducts
                    .Where(p =>
                    {
                        var lower = p.ToLowerInvariant();
                        return !lower.Contains("merch") &&
                               !lower.Contains("mug") &&
                               !lower.Contains("shirt") &&
                               !lower.Contains("hat") &&
                               !lower.Contains("cup");
                    })
                    .ToList();

                var preferences = string.Join(", ", coffeeProducts);

                if (!string.IsNullOrWhiteSpace(manualPreferences))
                    preferences += $", {manualPreferences}";

                var (userLat, userLon) = await GeocodeLocationAsync(location);

                if (userLat == 0 && userLon == 0)
                {
                    _logger.LogWarning("Invalid geocode result for {Location}", location);
                    return new List<CafeResult>();
                }

                var radiusMeters = radiusMiles * 1609;

                var cafes = await FetchCafesFromOverpassAsync(
                    userLat,
                    userLon,
                    radiusMeters);

                var results = cafes
                    .Select(c =>
                    {
                        var distance = CalculateDistanceMiles(
                            userLat,
                            userLon,
                            c.Latitude,
                            c.Longitude);

                        return new { Cafe = c, Distance = distance };
                    })
                    .OrderBy(x => x.Distance)
                    .Take(5)
                    .Select(x => new CafeResult
                    {
                        Name = x.Cafe.Name,
                        Address = x.Cafe.Address,
                        Distance = $"{x.Distance:F1} miles",
                        WhyItMatches = GenerateMatchDescription(preferences, x.Distance)
                    })
                    .ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching cafes for {Location}", location);
                return new List<CafeResult>();
            }
        }

        // 🧠 Smart matching logic
        private string GenerateMatchDescription(string preferences, double distance)
        {
            var reasons = new List<string>();

            // Distance reason
            if (distance < 0.5)
                reasons.Add("closest to you");
            else if (distance < 1.5)
                reasons.Add("nearby");
            else if (distance < 3)
                reasons.Add("within easy reach");
            else
                reasons.Add($"{distance:F1} miles away");

            // Preference matching
            var prefs = preferences.ToLowerInvariant();

            if (prefs.Contains("espresso"))
                reasons.Add("specializes in espresso");
            else if (prefs.Contains("latte"))
                reasons.Add("known for lattes");
            else if (prefs.Contains("cold"))
                reasons.Add("excellent cold brew");
            else if (prefs.Contains("dark"))
                reasons.Add("offers dark roasts");
            else if (!string.IsNullOrWhiteSpace(preferences))
                reasons.Add("matches your preferences");
            else
                reasons.Add("great coffee");

            return string.Join(", ", reasons);
        }
    }

    public class CafeInfo
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}