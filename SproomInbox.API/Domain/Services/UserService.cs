using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;

namespace SproomInbox.API.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<User>> ListUsersAsync()
        {
            return await _unitOfWork.UserRepository.ListAsync();
        }
    }
}
