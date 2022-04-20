
namespace SproomInbox.WebApp.Client.Services
{
    public interface IUsersFromWebServerService
    {
        Task<HttpResponseMessage> FetchUsersAsync();
    }
}