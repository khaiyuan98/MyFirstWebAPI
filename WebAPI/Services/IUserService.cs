using Test.WebAPI.Models.User;

namespace Test.WebAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task<int> RegisterAsync(NewUserDto newUser);
    }
}