using Test.DataAccess.Models;
using Test.WebAPI.Models.Department;

namespace Test.WebAPI.Services
{
    public interface IDepartmentService
    {
        Task<int> DeleteDepartment(int departmentId);
        Task<IEnumerable<Department>> GetDepartments();
        Task<int> InsertDepartment(NewDepartmentDto newDepartment);
        Task<int> UpdateDepartment(Department department);
    }
}