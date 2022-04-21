﻿using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Repositories;
using SproomInbox.API.Utils.ErrorHandling;
using SproomInbox.API.Utils.Paging;

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
        public async Task<Status<IEnumerable<User>>> ListUsersAsync()
        {
            var response = await _unitOfWork.UserRepository.ListAsync();

            if (response == null)
                throw new Exception("Failed to get users. Internal error.");

            return new Status<IEnumerable<User>>(response);
        }
    }
}
