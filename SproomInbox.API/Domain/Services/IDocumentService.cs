using SproomInbox.API.Domain.Models;
using SproomInbox.API.Parametrization;
using SproomInbox.API.Utils.ErrorHandling;
using SproomInbox.API.Utils.Pagination;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.API.Domain.Services
{
    public interface IDocumentService
    {
        Task<Status<PagedList<Document>>> ListDocumentsAsync(DocumentsQueryParameters queryParameters);
        Task<Status<Document>> FindByIdAsync(DocumentsFindByIdParameters findParameters);
        Task<Status<IEnumerable<Document>>> UpdateAsync(DocumentsUpdateStatusParameters updateListParameters);
    }
}