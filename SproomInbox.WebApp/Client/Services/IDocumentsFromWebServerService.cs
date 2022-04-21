using SproomInbox.WebApp.Shared.Resources.Parametrization;

namespace SproomInbox.WebApp.Client.Services
{
    public interface IDocumentsFromWebServerService
    {
        Task<HttpResponseMessage> FetchDocumentsAsync(DocumentsQueryParameters queryParameters);
        Task<HttpResponseMessage> UpdateDocumentsAsync(DocumentsUpdateStatusParameters updateParameters);
    }
}