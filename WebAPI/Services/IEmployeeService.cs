using Test.Shared.Models;
using Test.WebAPI.Models;

namespace Test.WebAPI.Services
{
    public interface IEmployeeService
    {
        Task<int> DeleteEmployee(int employeeId);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<int> InsertEmployee(NewEmployeeRequest newEmployee);
        string SaveFile(IFormFile postedFile, IWebHostEnvironment env);
        Task<int> UpdateEmployee(Employee employee);
    }
}