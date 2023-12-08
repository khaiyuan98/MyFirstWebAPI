using Test.WebAPI.Models.User;

namespace Test.WebAPI.Services
{
    public interface IUserService
    {
        Task<int> DeleteUser(int UserId);
        Task<IEnumerable<GetUserDto>> GetUsers();
        Task<int?> AddUser(NewUserDto newUser);
        Task<UserDetailsDto?> GetUserById(int id);
        Task<int?> UpdateUser(UpdateUserDto updatedUser);
    }
}