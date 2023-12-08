using Test.DataAccess.Models.Users;
using Test.WebAPI.Models.Auth;

namespace Test.WebAPI.Services
{
    public interface IAuthService
    {
        Task<FullUser?> GetCurrentUserAsync();
        Task<AuthUser?> LoginAsync(UserLoginDto userLogin);
        Task LogoutAsync();
        Task<AuthUser?> RefreshToken();
    }
}