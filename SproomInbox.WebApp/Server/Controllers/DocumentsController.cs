using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Server.Services;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private readonly IDocumentsFromApiService _documentService;
        public DocumentsController(IDocumentsFromApiService documentService, ILogger<DocumentsController> logger)
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

            return Ok(await response.Content.ReadFromJsonAsync<IEnumerable<DocumentDto>>() ?? Enumerable.Empty<DocumentDto>());
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