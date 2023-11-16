using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Test.DataAccess.Repository;
using Test.Shared.Models;
using Test.WebAPI.Controllers;
using Test.WebAPI.Models;

namespace Test.WebAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(ILogger<DepartmentService> logger, IDepartmentRepository departmentService)
        {
            _departmentRepository = departmentService;
            _logger = logger;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            IEnumerable<Department> departments = await _departmentRepository.GetDepartments();
            return departments;
        }

        public async Task<int> InsertDepartment(NewDepartmentRequest newDepartment)
        {
            Department department = new Department
            {
                DepartmentName = newDepartment.DepartmentName
            };

            int newId = await _departmentRepository.InsertDepartment(department);
            return newId;
        }

        public async Task<int> UpdateDepartment(Department department)
        {
            int res = await _departmentRepository.UpdateDepartment(department);
            return res;
        }

        public async Task<int> DeleteDepartment(int departmentId)
        {
            int res = await _departmentRepository.DeleteDepartment(departmentId);
            return res;
        }
    }
}
