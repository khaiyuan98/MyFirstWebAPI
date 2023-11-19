using Test.DataAccess.Models;

namespace Test.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<int> AddUser(User user);
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByUsername(string username);
    }
}