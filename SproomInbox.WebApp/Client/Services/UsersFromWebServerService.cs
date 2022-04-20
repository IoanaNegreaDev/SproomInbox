namespace SproomInbox.WebApp.Client.Services
{
    public class UsersFromWebServerService : IUsersFromWebServerService
    {
        private readonly HttpClient _httpClient;
        public UsersFromWebServerService(HttpClient httpClient)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> FetchUsersAsync()
        {
            string uri = _httpClient.BaseAddress + "users";
            var httpResponseMessage = await _httpClient.GetAsync(uri);
            return httpResponseMessage;
        }
    }
}
