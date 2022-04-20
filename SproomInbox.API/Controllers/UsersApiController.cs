using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.API.Utils.Caching;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.API.Controllers
{
    [ApiController]
    [Route("api/v1.0/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly ILogger<UsersApiController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersApiController(IUserService userService,
                            IMapper mapper,
                            ILogger<UsersApiController> logger)
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
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 70)]
        [HttpCacheValidation(MustRevalidate = true, NoCache = false)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.ListUsersAsync();
            var usersDtoList = _mapper.Map <IEnumerable<User>, IEnumerable<UserDto>>(users);
            return Ok(usersDtoList);
        }
    }
}
