using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Server.Services;
using SproomInbox.WebApp.Shared.Pagination;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Net;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("documents")]
    public class DocumentsWebServerController : ControllerBase
    {
        private readonly ILogger<DocumentsWebServerController> _logger;
        private readonly IDocumentsFromApiService _documentService;
        public DocumentsWebServerController(IDocumentsFromApiService documentService, ILogger<DocumentsWebServerController> logger)
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<DocumentDto>>> GetDocuments(string userName, string? type, string? state, string? currentPage)
        {
            if (!Int32.TryParse(currentPage, out var current))
                current = 1;

            var queryParameters = new DocumentsQueryParameters()
            {
                UserName = userName,
                Type = type,
                State = state,
                Page = new PagedListMetadata() { Current = current }
            };

            var response = await _documentService.FetchDocumentsAsync(queryParameters);
            
            if (response.StatusCode == HttpStatusCode.NoContent)
                return NoContent();
            if (response.StatusCode != HttpStatusCode.OK)
                return BadRequest();

            var translatedResponse = await response.Content.ReadFromJsonAsync<List<DocumentDto>>() ??
                                            Enumerable.Empty<DocumentDto>();
            var pagination = JsonSerializer.Deserialize<PagedListMetadata>(response.Headers.GetValues("X-Pagination").First());
        
            PagedList<DocumentDto> pagedList = new PagedList<DocumentDto>(translatedResponse.ToList(), pagination);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
            return Ok(pagedList);
       }

        [HttpPut]
        public async Task<ActionResult> UpdateDocuments(DocumentsUpdateStatusParameters updateParameters)
        {
            var response = await _documentService.UpdateDocumentsAsync(updateParameters);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            return Ok(response);
        }
    }
}