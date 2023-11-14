﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Test.DataAccess.Models;
using Test.DataAccess.Services;

namespace Test.WebAPI.Controllers
{
    [Authorize]
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
            IEnumerable<Department> departments = await _departmentService.GetDepartments();
            return Ok(departments);
        }

        //POST: api/department
        [HttpPost]
        public async Task<ActionResult<int>> Post(Department newDepartment)
        {
            int newId = await _departmentService.InsertDepartment(newDepartment);
            return Ok(newId);
        }

        [HttpPut]
        public async Task<ActionResult<int>> Put(Department department)
        {
            int res = await _departmentService.UpdateDepartment(department);
            return Ok(res);
        }

        [HttpDelete("{departmentId}")]
        public async Task<ActionResult<int>> Delete(int departmentId)
        {
            int res = await _departmentService.DeleteDepartment(departmentId);
            return Ok(res);
        }

    }
}
