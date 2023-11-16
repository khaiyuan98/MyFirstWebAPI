using Test.Shared.Models;
using Test.WebAPI.Models;

namespace Test.WebAPI.Services
{
    public interface IDepartmentService
    {
        Task<int> DeleteDepartment(int departmentId);
        Task<IEnumerable<Department>> GetDepartments();
        Task<int> InsertDepartment(NewDepartmentRequest newDepartment);
        Task<int> UpdateDepartment(Department department);
    }
}