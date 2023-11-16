using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Test.DataAccess.Repository;
using Test.WebAPI.Models;
using Test.WebAPI.Services;

namespace Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, IUserService userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userService = userService;
        }

        [Authorize]
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<int>> Register(NewUserRequest request)
        {
            int res = await _userService.RegisterAsync(request);
            return Ok(res);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<string>> LoginAsync(UserLoginRequest request) 
        {
            string? token = await _userService.LoginAsync(request);

            if (token == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(token);
        }
    }
}
