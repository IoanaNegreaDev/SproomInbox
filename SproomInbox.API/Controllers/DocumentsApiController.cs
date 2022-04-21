using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Text.Json;

namespace SproomInbox.API
{
    [ApiController]
    [Route("api/v1.0/documents")]
    public class DocumentsApiController : ControllerBase
    {
        private readonly ILogger<DocumentsApiController> _logger;
        private readonly IDocumentService _documentsService;
        private readonly IPaginationUriBuilder _paginationUriBuilder;
        private readonly IMapper _mapper;

        public DocumentsApiController(IDocumentService documentsService,
                                      IPaginationUriBuilder paginationUriBuilder,
                                      IMapper mapper,
                                      ILogger<DocumentsApiController> logger)
        {
            if (documentsService == null)
                throw new ArgumentNullException(nameof(documentsService));
            if (paginationUriBuilder == null)
                throw new ArgumentNullException(nameof(paginationUriBuilder));
            if (mapper == null) 
                throw new ArgumentNullException(nameof(mapper));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _documentsService = documentsService;
            _paginationUriBuilder = paginationUriBuilder;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(Name = "GetDocuments")]
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 70)]
        [HttpCacheValidation(MustRevalidate = true, NoCache =false, Vary = new[] { "Accept", "Accept-Language", "Accept-Encoding", "UserName", "Type", "State" })]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsAsync(
                                                                    [FromQuery]DocumentsQueryParameters queryParameters)
        {
            // user whould be authenticated
            // var authenticatedUserId = HttpContext.User.Identity.Name;

            var response = await _documentsService.ListDocumentsAsync(queryParameters);
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            var documents = response._entity;
            var documentsDtoPagedList = _mapper.Map<PagedList<Document>, PagedList<DocumentDto>>(documents);

             if (documentsDtoPagedList.Count == 0)
                return NoContent();

            AddPaginationInRequestHeader("GetDocuments",
                                         queryParameters,
                                         documentsDtoPagedList);
            return Ok(documentsDtoPagedList);          
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsById([FromQuery]  DocumentsFindByIdParameters findByIdParameters)
        {
            var response = await _documentsService.FindByIdAsync(findByIdParameters);        
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            var document = response._entity;
            if (document == null)
                return NotFound();

            var documentDto = _mapper.Map<Document, DocumentDto>(document);
            return Ok(documentDto);
        }

        [HttpPut]
        public async Task<ActionResult> Update(DocumentsUpdateStatusParameters updateParameters)
        { 
            var response = await _documentsService.UpdateAsync(updateParameters);
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            return Ok();
         
        }
        private void AddPaginationInRequestHeader(string routeName,
                                               DocumentsQueryParameters queryParameters,
                                               PagedList<DocumentDto> pagedList)
        {
            var paginationMetadata = _paginationUriBuilder.BuildPaginationMetadata(Url,
                                                                                    routeName,
                                                                                    queryParameters,
                                                                                    pagedList);
            if (Response != null && Response.Headers != null)
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        }
    }
}