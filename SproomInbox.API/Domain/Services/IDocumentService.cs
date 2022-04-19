using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.Paging;
using SproomInbox.API.Utils.Parametrization;

namespace SproomInbox.API.Domain.Services
{
    public interface IDocumentService
    {
        Task<PagedList<Document>> ListDocumentsAsync(DocumentListQueryParameters queryParameters);
        Task<Document> FindByIdAsync(DocumentsFindByIdParameters findParameters);
        Task<Document> UpdateAsync(DocumentsFindByIdParameters findParameters, string newState);
        Task<IEnumerable<Document>> UpdateAsync(List<DocumentsFindByIdParameters> findParametersList, string newState);
    }
}