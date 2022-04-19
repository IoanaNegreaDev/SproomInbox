using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Client.Services
{
    public interface IDocumentsFromWebServerService
    {
        Task<HttpResponseMessage> FetchDocumentsAsync(DocumentListQueryParameters queryParameters);
        Task<HttpResponseMessage> UpdateDocumentsAsync(DocumentListStatusUpdateParameters updateParameters);
    }
}