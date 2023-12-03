using Test.DataAccess.Models;

namespace Test.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByRefreshToken(string refreshToken);
        Task<User?> GetUserByUsername(string username);
        Task<IEnumerable<User>> GetUsers();
        Task<int> LoginUser(int UserId, string newRefreshToken, DateTime newLoginDate, DateTime newLoginExpires);
        Task<int> LogoutUser(int UserId);
    }
}