using Test.Shared.Models;

namespace Test.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User?> GetUserByUsername(string username);
    }
}