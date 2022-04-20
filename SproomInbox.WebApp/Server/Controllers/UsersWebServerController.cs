using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Server.Services;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersWebServerController : ControllerBase
    {
        private readonly ILogger<UsersWebServerController> _logger;
        private readonly IUsersFromApiService _usersService;

        public UsersWebServerController(IUsersFromApiService usersService, ILogger<UsersWebServerController> logger)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var result = await _usersService.FetchUsersAsync();
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            return Ok(await result.Content.ReadFromJsonAsync<IEnumerable<UserDto>>() ?? Enumerable.Empty<UserDto>());
        }
    }
}
