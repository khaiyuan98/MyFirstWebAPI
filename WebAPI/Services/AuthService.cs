using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Test.DataAccess.Models;
using Test.DataAccess.Repository;
using Test.WebAPI.Models.Auth;

namespace Test.WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger, IUserRepository userService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthUser?> LoginAsync(UserLoginDto userLogin)
        {
            User? user = await _userRepository.GetUserByUsername(userLogin.Username);

            if (user is null || user?.Username != userLogin.Username || !VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            string token = CreateToken(user);
            RefreshToken newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            await _userRepository.LoginUser(user.UserId, newRefreshToken.Token, newRefreshToken.CreatedDate, newRefreshToken.Expires);

            AuthUser authUser = _mapper.Map<AuthUser>(user);
            authUser.AccessToken = token;
            return authUser;
        }

        public async Task LogoutAsync()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete("refreshToken");
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is not null)
                await _userRepository.LogoutUser(int.Parse(userIdClaim.Value));

            return;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return null;

            User? user = await _userRepository.GetUserById(int.Parse(userIdClaim.Value));
            return user;
        }

        public async Task<AuthUser?> RefreshToken()
        {
            string? refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

            if (refreshToken is null)
                return null;

            User? user = await _userRepository.GetUserByRefreshToken(refreshToken);

            if (user is null)
                return null;

            AuthUser authUser = _mapper.Map<AuthUser>(user);
            string token = CreateToken(user);
            authUser.AccessToken = token;

            return authUser;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:Expires"] ?? "5")),
                signingCredentials: cred);

            string? jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                byte[]? computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private RefreshToken GenerateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                CreatedDate = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:RefreshExpires"] ?? "720")),
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken refreshToken)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
