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

        // GET: api/user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers()
        {
            IEnumerable<GetUserDto> users = await _userService.GetUsers();
            return users is not null ? Ok(users) : NotFound("Users not found");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetUserById(int id)
        {
            UserDetailsDto? user = await _userService.GetUserById(id);
            return user is not null ? Ok(user) : NotFound("User was not found");
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddUser(NewUserDto request)
        {
           int? res = await _userService.AddUser(request);
            return res > 0 ? Ok("Successfully added the user") : StatusCode(500, "Could not add the user");
        }

        [HttpPut]
        public async Task<ActionResult<string>> UpdateUser(UpdateUserDto request)
        {
            int? res = await _userService.UpdateUser(request);
            return res > 0 ? Ok("Successfully updated the user") : StatusCode(500, "Could not update the user");
        }

        [HttpDelete("{UserId}")]
        public async Task<ActionResult<string>> DeleteUser(int UserId)
        {
            int res = await _userService.DeleteUser(UserId);
            return res > 0 ? Ok("Successfully deleted the user") : StatusCode(500, "Could not delete the user");
        }

    }
}
