using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.DTOs;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;

namespace SproomInbox.API
{
    [ApiController]
    [Route("api/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IDocumentsService _documentsService;
        private readonly IMapper _mapper;

        public DocumentController(IDocumentsService documentsService,
                                  IMapper mapper,
                                  ILogger<DocumentController> logger)
        {
            if (documentsService == null)
                throw new ArgumentNullException(nameof(documentsService));
            
            if (mapper == null) 
                throw new ArgumentNullException(nameof(mapper));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _documentsService = documentsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments(
                                                                    [FromQuery] DocumentsQueryParameters queryParameters)
        {
            var documentsPagedList = await _documentsService.ListDocumentsAsync(queryParameters);
            var documentsDtoPagedList = _mapper.Map<PagedList<Document>, PagedList<DocumentDto>>(documentsPagedList);
            return Ok(documentsDtoPagedList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsById(string id, string userName)
        {
            if (!Guid.TryParse(id, out var documentId))
                return BadRequest("Invalid Id value.");

            DocumentsFindByIdParameters findByIdParameters = new DocumentsFindByIdParameters()
            {
                Id = documentId,
                UserName = userName
            };

            var document = await _documentsService.FindByIdAsync(findByIdParameters);
            if (document == null)
                return NotFound();

            var documentDto = _mapper.Map<Document, DocumentDto>(document);

            return Ok(documentDto);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsById(
                                                              string id, string userName,
                                                              string newState)
        {
            if (!Guid.TryParse(id, out var documentId))
                return BadRequest("Invalid Id value.");

            DocumentsFindByIdParameters findByIdParameters = new DocumentsFindByIdParameters()
            {
                Id = documentId,
                UserName = userName
            };

            var document = await _documentsService.UpdateAsync(findByIdParameters, newState);
            
            if (document == null)
                return NotFound();

            var documentDto = _mapper.Map<Document, DocumentDto>(document);
            return Ok(documentDto);
        }
    }
}