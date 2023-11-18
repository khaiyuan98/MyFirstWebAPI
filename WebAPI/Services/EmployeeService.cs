using AutoMapper;
using Test.DataAccess.Models;
using Test.DataAccess.Repository;
using Test.WebAPI.Models.Employee;

namespace Test.WebAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _employeeRepository.GetEmployees();
        }

        public async Task<int> InsertEmployee(NewEmployeeDto newEmployee)
        {
            Employee employee = _mapper.Map<Employee>(newEmployee);
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
