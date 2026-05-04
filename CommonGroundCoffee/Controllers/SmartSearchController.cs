using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Services;

[ApiController]
[Route("api/smart-search")]
public class SmartSearchController : ControllerBase
{
    private readonly GeminiService _gemini;

    public SmartSearchController(GeminiService gemini)
    {
        _gemini = gemini;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required.");

        var prompt =
            "You are an AI assistant for a coffee shop ordering system. " +
            "Answer clearly, help users find drinks, orders, and recommendations.\n\n" +
            $"User query: {query}";

        var result = await _gemini.AskAsync(prompt);

        return Ok(new SmartSearchResponse
        {
            Query = query,
            Result = result
        });
    }

    [HttpPost]
    public async Task<IActionResult> SearchPost([FromBody] SmartSearchRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Query))
            return BadRequest("Query is required.");

        var result = await _gemini.AskAsync(request.Query);

        return Ok(new SmartSearchResponse
        {
            Query = request.Query,
            Result = result
        });
    }
}