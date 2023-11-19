using AutoMapper;
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

        public async Task<string?> LoginAsync(UserLoginDto userLogin)
        {
            User? user = await _userRepository.GetUserByUsername(userLogin.Username);

            if (user?.Username != userLogin.Username || !VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            string token = CreateToken(user);
            return token;
        }

        public async Task<string?> LogoutAsync(UserLoginDto userLogin)
        {
            User? user = await _userRepository.GetUserByUsername(userLogin.Username);

            if (user?.Username != userLogin.Username || !VerifyPasswordHash(userLogin.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            string token = CreateToken(user);
            return token;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var userIdClaim = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return null;

            User? user = await _userRepository.GetUserById(int.Parse(userIdClaim.Value));
            return user;
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
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:Expires"] ?? "5")),
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
    }
}
