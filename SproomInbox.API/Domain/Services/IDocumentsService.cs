using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;

namespace SproomInbox.API.Domain.Services
{
    public interface IDocumentsService
    {
        Task<PagedList<Document>> ListDocumentsAsync(QueryParameters queryParameters);
    }
}