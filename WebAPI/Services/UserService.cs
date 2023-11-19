using AutoMapper;
using System.Security.Cryptography;
using Test.DataAccess.Models;
using Test.DataAccess.Repository;
using Test.WebAPI.Models.User;

namespace Test.WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(ILogger<EmployeeService> logger, IUserRepository userRepository, IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> RegisterAsync(NewUserDto newUser)
        {
            CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                Username = newUser.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            int res = await _userRepository.AddUser(user);
            return res;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
