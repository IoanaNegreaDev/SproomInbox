using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Validation;
using SproomInbox.WebApp.Shared.Pagination;
using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SproomInbox.API
{
    [Consumes("application/json")]
    [Produces("application/json")]
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


        /// <summary>
        /// Gets Sproom documents
        /// </summary>
        /// <param name="UserName">The username of an user. Required (until authentication is implemented)</param>
        /// <param name="Type">The type of the desired documents.Optional filter</param>
        /// <param name="State">The state of the desired documents.Optional filter</param>
        /// <param name="Search">A search string. Optional</param>
        /// <param name="Page.Current">The index of the current page, 1 based. Optional.Default.</param>
        /// <param name="Page.Size">The number of documents/page.Max page size is 50. Default page size is 10. Optional.</param>
        /// <returns>An ActionResult of type IEnumerable of DocumentDto</returns>
        /// <response code="200">Returns the requested list of documents</response>     
        /// 
        [HttpGet(Name = "GetDocuments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<PagedList<DocumentDto>>> GetDocumentsAsync(
                                                                    [FromQuery] DocumentsQueryParameters queryParameters)
        {
            // user whould be authenticated
            // authenticatedUserId = HttpContext.User.Identity.Name;
            var response = await _documentsService.ListDocumentsAsync(queryParameters);
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            var documents = response._entity;
            if (documents == null)
                return NoContent();

            var documentsDtoPagedList = _mapper.Map<PagedList<Document>, PagedList<DocumentDto>>(documents);
            if (documentsDtoPagedList.Count == 0)
                return NoContent();

            AddPaginationInRequestHeader("GetDocuments",
                                         queryParameters,
                                         documentsDtoPagedList);
            return Ok(documentsDtoPagedList);
        }


        /// <summary>
        /// Updates the state of a single document, identified by its id
        /// </summary>
        /// <param name="id">The id of the document to be located</param>
        /// <param name="userName">The username of an user. Required (until authentication is implemented)</param>
        /// <returns>An ActionResult of type IEnumerable of Document</returns>
        /// <response code="200">Returns the requested document</response>     
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsById(Guid id, string userName)
        {
            var findByIdParameters = new DocumentsFindByIdParameters() { Id = id, UserName = userName };
            var response = await _documentsService.FindByIdAsync(findByIdParameters);
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            var document = response._entity;
            if (document == null)
                return NotFound();

            var documentDto = _mapper.Map<Document, DocumentDto>(document);
            return Ok(documentDto);
        }


        /// <summary>
        /// Updates the state of a single document, identified by its id
        /// </summary>
        /// <param name="id">The id of the document to be updated</param>
        /// <param name="userName">The username of an user. Required (until authentication is implemented)</param>
        /// <param name="newState">The only value of a document that can be updated. Allowed values: Approved and Rejected </param>
        /// <returns>An ActionResult</returns>
        /// <remarks>
        ///     If the document is already in Approved or Rejected state, the update will not be performed.
        ///     Allowed values for update: Approved and Rejected.
        /// </remarks>
        /// <response code="200">Signals a successful update</response>     
        /// <response code="422">Signals that the updtate to the new document state is not allowed </response>     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, string userName, string newState)
        {
            if (!StateValidityChecker.IsValid(newState))
                return BadRequest($"Invalid State value. Must be not null or { Enum.GetName<State>(State.Received)}. " +
                                  $"Available states: {String.Join(", ", Enum.GetNames<State>())} .");

            var updateParameters = new DocumentsUpdateStatusParameters()
            {
                DocumentIds = new List<Guid>() { id },
                UserName = userName,
                NewState = newState
            };

            var response = await _documentsService.UpdateAsync(updateParameters);
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            return Ok();
        }

        /// <summary>
        /// Updates the state of a list of documents
        /// </summary>
        /// <param name="DocumentIds">The list of the ids of the documents to be updated</param>
        /// <param name="UserName">The username of an user. Required (until authentication is implemented)</param>
        /// <param name="NewState">The only value of a document that can be updated. Allowed values: Approved and Rejected </param>
        /// <returns>An ActionResult</returns>
        /// <remarks>
        ///     If ONE document is already in Approved or Rejected state, the update will fail.
        ///     If ONE document is not found, the update will fail.
        /// </remarks>
        /// <response code="200">Signals a successful update</response>     
        /// <response code="422">Signals that the updtate to the new document state is not allowed </response>     
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
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
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
        }
    }
}