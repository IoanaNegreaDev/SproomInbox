using Microsoft.AspNetCore.WebUtilities;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SproomInbox.WebApp.Client.Services
{
    public class DocumentsFromWebServerService: IDocumentsFromWebServerService
    {
        private readonly HttpClient _httpClient;
        public DocumentsFromWebServerService(HttpClient httpClient )
        {
            if ( httpClient == null )
                throw new ArgumentNullException(nameof(httpClient));
            _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> FetchDocumentsAsync(DocumentsQueryParameters queryParameters)
        {
            if (queryParameters == null)
                throw new ArgumentNullException(nameof(DocumentsQueryParameters));

            NameValueCollection queryPairs = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryPairs.Add("username", queryParameters.UserName);
            queryPairs.Add("type", queryParameters.Type);
            queryPairs.Add("state", queryParameters.State);
            queryPairs.Add("currentPage", queryParameters.Page.Current.ToString());

            string query = string.Empty;
            if (queryPairs.Count > 0)
                query = "?" + queryPairs.ToString();

            string uri = _httpClient.BaseAddress + "documents" + query;
            
            var httpResponseMessage = await _httpClient.GetAsync(uri);
    
            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> UpdateDocumentsAsync(DocumentsUpdateStatusParameters updateParameters)
        {
            var updateParametersJson = new StringContent(
                        JsonSerializer.Serialize(updateParameters),
                        Encoding.UTF8,
                        Application.Json);

            string uri = _httpClient.BaseAddress + "documents";
            var httpResponseMessage = await _httpClient.PutAsync(uri, updateParametersJson);

            return httpResponseMessage;
        }
    }
}
