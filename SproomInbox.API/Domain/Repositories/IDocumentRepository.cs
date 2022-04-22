using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Paging;
using SproomInbox.WebApp.Shared.Resources.Parametrization;

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