using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Server.Services
{
    public interface IDocumentsFromApiService
    {
        Task<HttpResponseMessage> FetchDocumentsAsync(DocumentsQueryParameters queryParameters);
        Task<HttpResponseMessage> UpdateDocumentsAsync(DocumentsUpdateStatusParameters updateParameters);
    }
}