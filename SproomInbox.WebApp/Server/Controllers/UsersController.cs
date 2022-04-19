using Microsoft.AspNetCore.Mvc;
using SproomInbox.WebApp.Shared.Resources;

namespace SproomInbox.WebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private static HttpClient _httpClient = new HttpClient();

        private readonly ILogger<UsersController> _logger;
        private readonly string _baseApiRoute;

        public UsersController(IConfiguration appConfig, ILogger<UsersController> logger)
        {
            _logger = logger;
            _baseApiRoute = appConfig.GetSection("ConnectionStrings:SproomDocumentsApiV1.0").Value;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            string uri = _baseApiRoute + "users";
            var result = await _httpClient.GetAsync(uri);
            return await result.Content.ReadFromJsonAsync<IEnumerable<UserDto>>() ?? Enumerable.Empty<UserDto>();
        }
    }
}
