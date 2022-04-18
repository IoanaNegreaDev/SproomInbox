using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Domain.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> ListUsersAsync();
    }
}