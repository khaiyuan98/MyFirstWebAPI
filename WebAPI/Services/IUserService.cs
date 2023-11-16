using Test.WebAPI.Models;

namespace Test.WebAPI.Services
{
    public interface IUserService
    {
        Task<string?> LoginAsync(UserLoginRequest request);
        Task<int> RegisterAsync(NewUserRequest request);
    }
}