using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Test.DataAccess.Models;
using Test.DataAccess.Services;

namespace Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentRepository _departmentService;

        public DepartmentController(IConfiguration configuration, ILogger<DepartmentController> logger, IDepartmentRepository departmentService)
        {
            _configuration = configuration;
            _departmentService = departmentService;
            _logger = logger;
        }

        // GET: api/department
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> Get()
        {
            try
            {
                IEnumerable<Department> departments = await _departmentService.GetDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        //POST: api/department
        [HttpPost]
        public async Task<ActionResult<int>> Post(Department newDepartment)
        {
            try
            {
                int newId = await _departmentService.InsertDepartment(newDepartment);
                return Ok(newId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Department department)
        {
            try
            {
                int res = await _departmentService.UpdateDepartment(department);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpDelete("{departmentId}")]
        public async Task<ActionResult<int>> Delete(int departmentId)
        {
            try
            {
                int res = await _departmentService.DeleteDepartment(departmentId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

    }
}
