using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Test.DataAccess.Repository;
using Test.Shared.Models;
using Test.WebAPI.Models;

namespace Test.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;

        public UserService(IConfiguration configuration, ILogger<UserService> logger, IUserRepository userService)
        {
            _configuration = configuration;
            _logger = logger;
            _userRepository = userService;
        }

        public async Task<int> RegisterAsync(NewUserRequest request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            int res = await _userRepository.AddUser(user);
            return res;
        }

        public async Task<string?> LoginAsync(UserLoginRequest request)
        {
            User? user = await _userRepository.GetUserByUsername(request.Username);

            if (user?.Username != request.Username || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            string token = CreateToken(user);
            return token;
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
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            string? jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
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
