using SproomInbox.API.Domain.Models;

namespace SproomInbox.API.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> ListAsync();
    }
}