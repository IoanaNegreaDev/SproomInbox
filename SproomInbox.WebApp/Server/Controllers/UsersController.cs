using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Server.Services;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersFromApiService _usersService;

        public UsersController(IUsersFromApiService usersService, ILogger<UsersController> logger)
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
