using Test.DataAccess.Models;
using Test.WebAPI.Models.Auth;

namespace Test.WebAPI.Services
{
    public interface IAuthService
    {
        Task<User?> GetCurrentUserAsync();
        Task<string?> LoginAsync(UserLoginDto userLogin);
    }
}