using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Utils.Parametrization;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private static HttpClient _httpClient = new HttpClient();

        private readonly ILogger<DocumentsController> _logger;
        private readonly string _baseApiRoute;

        public DocumentsController(IConfiguration appConfig, ILogger<DocumentsController> logger)
        {
            _logger = logger;
            _baseApiRoute = appConfig.GetSection("ConnectionStrings:SproomDocumentsApiV1.0").Value;
        }

   

        [HttpGet]
        public async Task<IEnumerable<DocumentDto>> GetDocuments(string userName, string? type, string? state)
        {
            NameValueCollection queryPairs = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryPairs.Add("username", userName);
            queryPairs.Add("type", type);
            queryPairs.Add("state", state);

            string query = string.Empty;
            if (queryPairs.Count > 0)
                query = "?" + queryPairs.ToString();   

            string uri = _baseApiRoute + "documents" + query;
          
            var result = await _httpClient.GetAsync(uri);
            return await result.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>() ?? Enumerable.Empty<DocumentDto>();
        }


        [HttpPut]
        public async Task<IEnumerable<DocumentDto>> UpdateDocuments(DocumentListStatusUpdateParameters updateParameters)
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