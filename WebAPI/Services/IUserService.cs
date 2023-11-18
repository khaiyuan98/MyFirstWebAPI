using Test.WebAPI.Models.User;

namespace Test.WebAPI.Services
{
    public interface IUserService
    {
        Task<string?> LoginAsync(UserLoginDto userLogin);
        Task<int> RegisterAsync(NewUserDto newUser);
    }
}