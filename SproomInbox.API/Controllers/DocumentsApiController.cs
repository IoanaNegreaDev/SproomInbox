using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.API
{
    [ApiController]
    [Route("api/v1.0/documents")]
    public class DocumentsApiController : ControllerBase
    {
        private readonly ILogger<DocumentsApiController> _logger;
        private readonly IDocumentService _documentsService;
        private readonly IMapper _mapper;

        public DocumentsApiController(IDocumentService documentsService,
                                  IMapper mapper,
                                  ILogger<DocumentsApiController> logger)
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
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 70)]
        [HttpCacheValidation(MustRevalidate = true, NoCache =false, Vary = new[] { "Accept", "Accept-Language", "Accept-Encoding", "UserName", "Type", "State" })]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments(
                                                                    [FromQuery] DocumentListQueryParameters queryParameters)
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

        [HttpPut("{id}")]
        public async Task<ActionResult<DocumentDto>> Update(string id, string userName,
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

        [HttpPut]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> Update(DocumentListStatusUpdateParameters updateParameters)
        { 
            List<DocumentsFindByIdParameters > findByIdParametersList = new List< DocumentsFindByIdParameters >();
            foreach (var id in updateParameters.DocumentIds)
            {
                if (!Guid.TryParse(id, out var documentId))
                    return BadRequest("Invalid Id value.");

                DocumentsFindByIdParameters findByIdParameters = new DocumentsFindByIdParameters()
                {
                    Id = documentId,
                    UserName = updateParameters.UserName
                };
                findByIdParametersList.Add(findByIdParameters);
            }

            var documents = await _documentsService.UpdateAsync(findByIdParametersList, updateParameters.NewState);
            if (documents == null)
                return BadRequest();

            var documentsDto = _mapper.Map<IEnumerable<Document>, IEnumerable<DocumentDto>>(documents);
            return Ok(documentsDto);
        }
    }
}