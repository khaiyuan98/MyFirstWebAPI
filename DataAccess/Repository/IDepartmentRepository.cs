using Test.DataAccess.Models;

namespace Test.DataAccess.Services
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetDepartments();
        Task<int> InsertDepartment(Department newDepartment);
        Task<int> UpdateDepartment(Department department);
        Task<int> DeleteDepartment(int id);
    }
}