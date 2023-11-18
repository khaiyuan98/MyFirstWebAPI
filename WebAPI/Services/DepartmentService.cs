using AutoMapper;
using Test.DataAccess.Models;
using Test.DataAccess.Repository;
using Test.WebAPI.Models.Department;

namespace Test.WebAPI.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(ILogger<DepartmentService> logger, IDepartmentRepository departmentService, IMapper mapper)
        {
            _departmentRepository = departmentService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            IEnumerable<Department> departments = await _departmentRepository.GetDepartments();
            return departments;
        }

        public async Task<int> InsertDepartment(NewDepartmentDto newDepartment)
        {
            Department department = _mapper.Map<Department>(newDepartment);
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
