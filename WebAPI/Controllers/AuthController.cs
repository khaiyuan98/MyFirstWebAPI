using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.DataAccess.Models;
using Test.WebAPI.Models.Auth;
using Test.WebAPI.Models.User;
using Test.WebAPI.Services;

namespace Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration configuration, ILogger<AuthController> logger, IAuthService authService, IMapper mapper)
        {
            _configuration = configuration;
            _logger = logger;
            _authService = authService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<AuthUser>> Login(UserLoginDto request) 
        {
            AuthUser? authUser = await _authService.LoginAsync(request);
            return authUser is not null ? Ok(authUser) : Unauthorized("Invalid username or password.");
        }

        [AllowAnonymous]
        [Route("refresh")]
        [HttpPost]
        public async Task<ActionResult<AuthUser>> RefreshToken()
        {
            AuthUser? authUser = await _authService.RefreshToken();
            if (authUser is null)
            {
                return Forbid("Invalid refresh token");
            }

            return Ok(authUser);
        }

        [AllowAnonymous]
        [Route("logout")]
        [HttpPost]
        public async Task<ActionResult<string>> Logout()
        {
            await _authService.LogoutAsync();
            return Ok();
        }

        [Authorize]
        [Route("user")]
        [HttpGet]
        public async Task<ActionResult<CurrentUserDto>> GetCurrentUser()
        {
            User? user = await _authService.GetCurrentUserAsync();
            CurrentUserDto currentUser = _mapper.Map<CurrentUserDto>(user);
            return user is not null ? Ok(currentUser) : NotFound(currentUser);
        }
    }
}
