using Test.DataAccess.Models;
using Test.WebAPI.Models.Employee;

namespace Test.WebAPI.Services
{
    public interface IEmployeeService
    {
        Task<int> DeleteEmployee(int employeeId);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<int> InsertEmployee(NewEmployeeDto newEmployee);
        string SaveFile(IFormFile postedFile, IWebHostEnvironment env);
        Task<int> UpdateEmployee(Employee employee);
    }
}