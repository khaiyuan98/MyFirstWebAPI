using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.WebAPI.Models.User;
using Test.WebAPI.Services;

namespace Test.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration, ILogger<AuthController> logger, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<int>> Register(NewUserDto request)
        {
            int res = await _userService.RegisterAsync(request);
            return Ok(res);
        }

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            IEnumerable<UserDto> users = await _userService.GetUsers();
            return Ok(users);
        }

    }
}
