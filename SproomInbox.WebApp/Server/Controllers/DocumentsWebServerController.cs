using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Server.Services;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

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
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments(string userName, string? type, string? state)
        {
            var queryParameters = new DocumentListQueryParameters()
            {
                UserName = userName,
                Type = type,
                State = state   
            };

            var response = await _documentService.FetchDocumentsAsync(queryParameters);
            
            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var translatedResponse = await response.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>() ??
                                            Enumerable.Empty<DocumentDto>();
            return Ok(translatedResponse);
       }

        [HttpPut]
        public async Task<ActionResult> UpdateDocuments(DocumentListStatusUpdateParameters updateParameters)
        {
            var response = await _documentService.UpdateDocumentsAsync(updateParameters);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            return Ok(response);
        }
    }
}