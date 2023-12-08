using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using Test.DataAccess.Models.Users;
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

        public async Task<int?> AddUser(NewUserDto newUser)
        {
            FullUser? existingUser = await _userRepository.GetUserByUsername(newUser.Username);

            if (existingUser is not null)
                return null;

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return null;

            CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);

            AddUser user = _mapper.Map<AddUser>(newUser);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.LastUpdatedById = int.Parse(userIdClaim.Value);

            int res = await _userRepository.InsertUser(user);
            return res;
        }

        public async Task<int?> UpdateUser(UpdateUserDto updatedUser)
        {
            FullUser? existingUser = await _userRepository.GetUserById(updatedUser.UserId);

            if (existingUser is null)
                return null;

            Claim? userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
                return null;

            UpdateUser user = _mapper.Map<UpdateUser>(updatedUser);
            user.LastUpdatedById = int.Parse(userIdClaim.Value);

            if (!String.IsNullOrWhiteSpace(updatedUser.Password))
            {
                CreatePasswordHash(updatedUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            else
            {
                user.PasswordHash = existingUser.PasswordHash;
                user.PasswordSalt = existingUser.PasswordSalt;
            }

            int res = await _userRepository.UpdateUser(user);
            return res;
        }

        public async Task<IEnumerable<GetUserDto>> GetUsers() 
        {
            IEnumerable<User> users = await _userRepository.GetUsers();
            IEnumerable<GetUserDto> mappedUsers =  _mapper.Map<IEnumerable<User>, IEnumerable<GetUserDto>>(users);
            return mappedUsers;
        }

        public async Task<UserDetailsDto?> GetUserById(int id) 
        {
            FullUser? user = await _userRepository.GetUserById(id);

            if (user is null)
                return null;

            UserDetailsDto userDetails = _mapper.Map<UserDetailsDto>(user);
            return userDetails;
        }

        public async Task<int> DeleteUser(int UserId) 
        {
            int res = await _userRepository.DeleteUser(UserId);
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
