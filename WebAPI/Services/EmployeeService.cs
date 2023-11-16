using Test.DataAccess.Repository;
using Test.Shared.Models;
using Test.WebAPI.Controllers;
using Test.WebAPI.Models;

namespace Test.WebAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _employeeRepository.GetEmployees();
        }

        public async Task<int> InsertEmployee(NewEmployeeRequest newEmployee)
        {
            Employee employee = new Employee
            {
                EmployeeName = newEmployee.EmployeeName,
                Department = newEmployee.Department,
                DateOfJoining = newEmployee.DateOfJoining,
                PhotoFileName = newEmployee.PhotoFileName
            };

            return await _employeeRepository.InsertEmployee(employee);
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            return await _employeeRepository.UpdateEmployee(employee);
        }

        public async Task<int> DeleteEmployee(int employeeId)
        {
            return await _employeeRepository.DeleteEmployee(employeeId);
        }

        public string SaveFile(IFormFile postedFile, IWebHostEnvironment env)
        {
            string filename = postedFile.FileName;
            var physicalPath = Path.Combine(env.ContentRootPath, "Photos", filename);

            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }

            return filename;
        }
    }
}
