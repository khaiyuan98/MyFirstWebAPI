using Dapper;
using System.Data;
using Test.DataAccess.DataAccess;
using Test.DataAccess.Models;

namespace Test.DataAccess.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ISqlDataAccess _db;

        public DepartmentRepository(ISqlDataAccess myDb)
        {
            _db = myDb;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            string query = @"SELECT DepartmentId, DepartmentName FROM dbo.Department;";

            using IDbConnection connection = _db.OpenConnection();
            return await connection.QueryAsync<Department>(query);
        }
        public async Task<int> InsertDepartment(Department newDepartment) 
        {
            string query = @"INSERT INTO dbo.Department(DepartmentName) VALUES (@DepartmentName);
                            SELECT @department_id = SCOPE_IDENTITY();";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.AddDynamicParams(newDepartment);
            dynamicParameters.Add("@department_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, dynamicParameters);

            int newIdentity = dynamicParameters.Get<int>("@department_id");
            return newIdentity;
        }

        public async Task<int> UpdateDepartment(Department department)
        {
            string query = @"UPDATE dbo.Department SET DepartmentName = @DepartmentName WHERE DepartmentId = @DepartmentId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, department);
            return res;
        }

        public async Task<int> DeleteDepartment(int departmentId)
        {
            string query = @"DELETE FROM dbo.Department WHERE DepartmentId = @DepartmentId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { DepartmentId = departmentId});
            return res;
        }

    }
}
