using Test.DataAccess.Models;

namespace Test.DataAccess.Services
{
    public interface IEmployeeRepository
    {
        Task<int> DeleteEmployee(int employeeId);
        Task<IEnumerable<Employee>> GetEmployees();
        Task<int> InsertEmployee(Employee newEmployee);
        Task<int> UpdateEmployee(Employee employee);
    }
}