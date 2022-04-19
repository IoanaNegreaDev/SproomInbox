using SproomInbox.API.Utils.Parametrization;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Server.Services
{
    public class DocumentsFromApiService : IDocumentsFromApiService
    {
        private readonly HttpClient _httpClient;
        public DocumentsFromApiService(HttpClient httpClient)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            _httpClient = httpClient;
        }

        public async Task<IEnumerable<DocumentDto>> FetchDocuments(DocumentListQueryParameters queryParameters)
        {
            if (queryParameters == null)
                throw new ArgumentNullException(nameof(DocumentListQueryParameters));

            var filterString = $"?username={queryParameters.UserName}";
            filterString += $"&type={queryParameters.Type}";
            filterString += $"&state={queryParameters.State}";

            return await _httpClient.GetFromJsonAsync<IList<DocumentDto>>($"documents" + filterString);
        }
    }
}
