using Test.WebAPI.Models.User;

namespace Test.WebAPI.Services
{
    public interface IUserService
    {
        Task<int> RegisterAsync(NewUserDto newUser);
    }
}