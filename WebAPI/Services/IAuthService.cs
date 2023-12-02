using Test.DataAccess.Models;
using Test.WebAPI.Models.Auth;

namespace Test.WebAPI.Services
{
    public interface IAuthService
    {
        Task<User?> GetCurrentUserAsync();
        Task<AuthUser?> LoginAsync(UserLoginDto userLogin);
        Task LogoutAsync();
        Task<AuthUser?> RefreshToken();
    }
}