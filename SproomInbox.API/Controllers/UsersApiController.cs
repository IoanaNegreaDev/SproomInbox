using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SproomInbox.API.Domain.Models;
using SproomInbox.API.Domain.Services;
using SproomInbox.WebApp.Shared.Resources;


namespace SproomInbox.API.Controllers
{
  // [Authorize]
    [Produces("application/json")]
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

        /// <summary>
        /// Gets all the users from the database
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable of UserDto</returns>
        /// <response code="200">Returns the list of users</response>     
        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 70)]
        // [HttpCacheValidation(MustRevalidate = true, NoCache = false)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {     
            var response = await _userService.ListUsersAsync();
            if (!response.Success)
                return StatusCode((int)response.StatusCode, response.Message);

            IEnumerable<User>? users = response._entity;
            if (users == null)
                return NoContent();

            var usersDtoList = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return Ok(usersDtoList);
           
        }
    }
}
