using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.API.Controllers
{
    [ApiController]
    [Route("api/v1.0/users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService,
                            IMapper mapper,
                            ILogger<UsersController> logger)
        {
            if (userService == null)
                throw new ArgumentNullException(nameof(userService));

            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetDocuments()
        {
            var users = await _userService.ListUsersAsync();
            var usersDtoList = _mapper.Map <IEnumerable<User>, IEnumerable<UserDto>>(users);
            return Ok(usersDtoList);
        }
    }
}
