using SproomInbox.API.Utils.Parametrization;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Server.Services
{
    public interface IDocumentsFromApiService
    {
        Task<IEnumerable<DocumentDto>> FetchDocuments(DocumentListQueryParameters queryParameters);
    }
}