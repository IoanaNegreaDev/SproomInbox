using SproomInbox.API.Domain.Models;
using SproomInbox.API.Parametrization;
using SproomInbox.API.Utils.Pagination;

namespace SproomInbox.API.Domain.Repositories
{
    public interface IDocumentRepository
    {
        Task<PagedList<Document>> ListAsync();
        Task<PagedList<Document>> ListAsync(DocumentsQueryParameters queryParameters);
        Task<Document?> FindByIdAsync(DocumentsFindByIdParameters findParameters);
        void Update(Document document);
    }
}