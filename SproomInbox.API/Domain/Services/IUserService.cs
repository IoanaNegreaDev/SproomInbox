using SproomInbox.API.Domain.Models;
using SproomInbox.API.Utils.ErrorHandling;

namespace SproomInbox.API.Domain.Services
{
    public interface IUserService
    {
        Task<Status<IEnumerable<User>>> ListUsersAsync();
    }
}