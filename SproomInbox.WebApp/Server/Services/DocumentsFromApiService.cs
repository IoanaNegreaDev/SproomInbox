using SproomInbox.WebApp.Shared.Resources;
using SproomInbox.WebApp.Shared.Resources.Parametrization;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace SproomInbox.WebApp.Server.Services
{
    public class DocumentsFromApiService : IDocumentsFromApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IEmailService _emailService;
        private readonly string _baseAddress;
        public DocumentsFromApiService(HttpClient httpClient, IEmailService emailService)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            _httpClient = httpClient;
            _emailService = emailService;
            _baseAddress = _httpClient.BaseAddress + "documents";
        }

        public async Task<HttpResponseMessage> FetchDocumentsAsync(DocumentListQueryParameters queryParameters)
        {
            if (queryParameters == null)
                throw new ArgumentNullException(nameof(DocumentListQueryParameters));

            NameValueCollection queryPairs = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryPairs.Add("username", queryParameters.UserName);
            queryPairs.Add("type", queryParameters.Type);
            queryPairs.Add("state", queryParameters.State);

            string query = string.Empty;
            if (queryPairs.Count > 0)
                query = "?" + queryPairs.ToString();

            string uri = _baseAddress + query;

            var httpResponseMessage = await _httpClient.GetAsync(uri);

            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> UpdateDocumentsAsync(DocumentListStatusUpdateParameters updateParameters)
        {
            var enumValueParsingSucceded = Enum.TryParse<StateDto>(updateParameters.NewState, out var stateValue);
            if (!enumValueParsingSucceded ||
                 stateValue == StateDto.Received)
                throw new InvalidOperationException($"Invalid document state {updateParameters.NewState}.");

            var updateParametersJson = new StringContent(
                                            JsonSerializer.Serialize(updateParameters),
                                            Encoding.UTF8,
                                            Application.Json);

            var httpResponseMessage = await _httpClient.PutAsync(_baseAddress, updateParametersJson);

            
            if (httpResponseMessage.IsSuccessStatusCode &&
                stateValue == StateDto.Approved)
                _emailService.SendApprovedDocumentsEmail(updateParameters);

            return httpResponseMessage;
        }
    }
}
