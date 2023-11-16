using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Shared.Models;
using Test.WebAPI.Models;
using Test.WebAPI.Services;

namespace Test.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IConfiguration configuration, ILogger<DepartmentController> logger, IWebHostEnvironment env, IEmployeeService employeeService)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _env = env;
            _logger = logger; 
        }

        // GET: api/employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            IEnumerable<Employee> employees = await _employeeService.GetEmployees();
            return Ok(employees);
        }

        //POST: api/employee
        [HttpPost]
        public async Task<ActionResult<int>> Post(NewEmployeeRequest newEmployee)
        {
            int newId = await _employeeService.InsertEmployee(newEmployee);
            return Ok(newId);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Employee employee)
        {
            int res = await _employeeService.UpdateEmployee(employee);
            return Ok(res);
        }

        [HttpDelete("{employeeId}")]
        public async Task<ActionResult<int>> Delete(int employeeId)
        {
            int res = await _employeeService.DeleteEmployee(employeeId);
            return Ok(res);
        }

        [Route("SaveFile")]
        [HttpPost]
        public ActionResult<string> SaveFile(IFormFile postedFile)
        {
            string filename = _employeeService.SaveFile(postedFile, _env);
            return Ok(filename);
        }
    }
}
