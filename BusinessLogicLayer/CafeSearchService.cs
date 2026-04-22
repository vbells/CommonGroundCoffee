using BusinessLogicLayer.Interfaces;
using CommonGroundCoffee.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class CafeSearchService : ICafeSearchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        // 🔥 SIMPLE THROTTLE (prevents API spam)
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        // retry settings
        private const int MaxRetries = 3;
        private const int BaseDelayMs = 1000;

        public CafeSearchService(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<List<CafeResult>> SearchCafesAsync(
            string location,
            int radiusMiles,
            List<string> purchasedProducts,
            string? manualPreferences)
        {
            await _semaphore.WaitAsync(); // 🚦 throttle start

            try
            {
                var preferences = string.Join(", ", purchasedProducts ?? new List<string>());

                if (!string.IsNullOrWhiteSpace(manualPreferences))
                    preferences += $", {manualPreferences}";

                var prompt = $@"
You are a coffee shop recommendation assistant.
A customer is located at: {location}
They want cafes within {radiusMiles} miles.
Their preferences: {preferences}

Return ONLY valid JSON array:
[
  {{
    ""name"": ""Cafe Name"",
    ""address"": ""Full Address"",
    ""distance"": ""X.X miles"",
    ""whyItMatches"": ""short reason""
  }}
]";
                Console.WriteLine("🧠 ===== GEMINI PROMPT START =====");
                Console.WriteLine(prompt);
                Console.WriteLine("🧠 ===== GEMINI PROMPT END =====");
                var client = _httpClientFactory.CreateClient();

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url =
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

                HttpResponseMessage response = null;
                string responseBody = "";

                // 🔁 RETRY LOOP (fixes 429 + temporary failures)
                for (int attempt = 1; attempt <= MaxRetries; attempt++)
                {
                    response = await client.PostAsync(url, content);
                    responseBody = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"📊 Attempt {attempt} Status: {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                        break;

                    if ((int)response.StatusCode == 429 || (int)response.StatusCode >= 500)
                    {
                        int delay = BaseDelayMs * attempt; // exponential backoff
                        Console.WriteLine($"⏳ Rate limited or server issue. Retrying in {delay}ms...");
                        await Task.Delay(delay);
                        continue;
                    }

                    // non-retryable error
                    Console.WriteLine($"❌ Fatal API error: {responseBody}");
                    return new List<CafeResult>();
                }

                if (!response.IsSuccessStatusCode)
                    return new List<CafeResult>();

                using var doc = JsonDocument.Parse(responseBody);

                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(text))
                    return new List<CafeResult>();

                var start = text.IndexOf('[');
                var end = text.LastIndexOf(']');

                if (start == -1 || end == -1)
                    return new List<CafeResult>();

                var jsonArray = text.Substring(start, end - start + 1);

                var results = JsonSerializer.Deserialize<List<CafeResult>>(
                    jsonArray,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return results ?? new List<CafeResult>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Exception: {ex.Message}");
                return new List<CafeResult>();
            }
            finally
            {
                _semaphore.Release(); // 🚦 always release throttle
            }
        }
    }
}