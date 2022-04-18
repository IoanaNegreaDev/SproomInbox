using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Shared.Resources;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private static HttpClient _httpClient = new HttpClient();

        private readonly ILogger<DocumentController> _logger;
        private readonly string _baseApiRoute;

        public DocumentController(IConfiguration appConfig, ILogger<DocumentController> logger)
        {
            _logger = logger;
            _baseApiRoute = appConfig.GetSection("ConnectionStrings:SproomDocumentsApiV1.0").Value;
        }

        [HttpGet]
        public async Task<IEnumerable<DocumentDto>> GetDocuments(string userName, string? type, string? state)
        {
            string filterString = string.Empty;
            if (!string.IsNullOrEmpty(userName))
                filterString = $"?username={userName}";
            else
                throw new Exception("UserName is required.");

            if (!string.IsNullOrEmpty(type))
                if (filterString == string.Empty)
                   filterString = $"?type={type}";
                else
                  filterString += $"&type={type}";

            if (!string.IsNullOrEmpty(state))
                if (filterString == string.Empty)
                    filterString = $"?state={state}";
                else
                    filterString += $"&state={state}";

            string uri = _baseApiRoute + "documents" + filterString;
          
            var result = await _httpClient.GetAsync(uri);
            return await result.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>() ?? Enumerable.Empty<DocumentDto>();
        }


        [HttpPut]
        public async Task<IEnumerable<DocumentDto>> UpdateDocuments(DocumentListUpdateParameters updateParameters)
        {
            var updateParametersJson = new StringContent(
                  JsonSerializer.Serialize(updateParameters),
                  Encoding.UTF8,
                  Application.Json);

            string uri = _baseApiRoute + "documents" ;
            var httpResponseMessage = await _httpClient.PutAsync(uri, updateParametersJson);

            httpResponseMessage.EnsureSuccessStatusCode();

            return await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>() ?? Enumerable.Empty<DocumentDto>();
        }
    }
}