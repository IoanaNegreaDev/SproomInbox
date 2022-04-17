using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {

        private static HttpClient _httpClient = new HttpClient();

        private readonly ILogger<DocumentController> _logger;

        public DocumentController(ILogger<DocumentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<DocumentDto>> GetDocuments()
        {
            var result = await _httpClient.GetAsync("http://localhost:5000/api/v1.0/documents?UserName=TheOne");
            return await result.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>() ?? Enumerable.Empty<DocumentDto>();
        }
    }
}