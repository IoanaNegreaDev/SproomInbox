
namespace SproomInbox.WebApp.Server.Services
{
    public interface IUsersFromApiService
    {
        Task<HttpResponseMessage> FetchUsersAsync();
    }
}