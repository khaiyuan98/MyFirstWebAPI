using Test.DataAccess.Models.Users;

namespace Test.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<int> InsertUser(AddUser user);
        Task<int> DeleteUser(int UserId);
        Task<FullUser?> GetUserById(int id);
        Task<FullUser?> GetUserByRefreshToken(string refreshToken);
        Task<FullUser?> GetUserByUsername(string username);
        Task<IEnumerable<User>> GetUsers();
        Task<int> LoginUser(int UserId, string newRefreshToken, DateTime newLoginDate, DateTime newLoginExpires);
        Task<int> LogoutUser(int UserId);
        Task<int> UpdateUser(UpdateUser user);
    }
}