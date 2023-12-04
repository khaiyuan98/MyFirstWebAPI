using AutoMapper;
using System;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(ILogger<EmployeeService> logger, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int?> RegisterAsync(NewUserDto newUser)
        {
            CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return null;

            User user = _mapper.Map<User>(newUser);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LastUpdatedBy = int.Parse(userIdClaim.Value);

            int res = await _userRepository.AddUser(user);
            return res;
        }

        public async Task<IEnumerable<UserDto>> GetUsers() 
        {
            IEnumerable<User> users = await _userRepository.GetUsers();
            IEnumerable<UserDto> mappedUsers =  _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(users);
            return mappedUsers;
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
