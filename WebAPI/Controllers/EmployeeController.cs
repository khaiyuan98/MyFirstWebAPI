using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.DataAccess.Models;
using Test.DataAccess.Services;

namespace Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository _employeeService;

        public EmployeeController(IConfiguration configuration, ILogger<DepartmentController> logger, IWebHostEnvironment env, IEmployeeRepository employeeService)
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
            try
            {
                IEnumerable<Employee> employees = await _employeeService.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        //POST: api/employee
        [HttpPost]
        public async Task<ActionResult<int>> Post(Employee newEmployee)
        {
            try
            {
                int newId = await _employeeService.InsertEmployee(newEmployee);
                return Ok(newId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Employee employee)
        {
            try
            {
                int res = await _employeeService.UpdateEmployee(employee);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpDelete("{employeeId}")]
        public async Task<ActionResult<int>> Delete(int employeeId)
        {
            try
            {
                int res = await _employeeService.DeleteEmployee(employeeId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public ActionResult<string> SaveFile()
        {
            try
            {
                var  httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return Ok(filename);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }
    }
}
