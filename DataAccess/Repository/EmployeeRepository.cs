using Dapper;
using System.Data;
using Test.DataAccess.DataAccess;
using Test.DataAccess.Models;

namespace Test.DataAccess.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlDataAccess _db;

        public EmployeeRepository(ISqlDataAccess myDb)
        {
            _db = myDb;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            string query = @"SELECT EmployeeId, EmployeeName, Department, DateOfJoining, PhotoFileName FROM dbo.Employee;";

            using IDbConnection connection = _db.OpenConnection();
            return await connection.QueryAsync<Employee>(query);
        }
        public async Task<int> InsertEmployee(Employee newEmployee)
        {
            string query = @"INSERT INTO dbo.Employee(EmployeeName, Department, DateOfJoining, PhotoFileName) 
                            VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName);
                            SELECT @employee_id = SCOPE_IDENTITY();";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.AddDynamicParams(newEmployee);
            dynamicParameters.Add("@employee_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, dynamicParameters);

            int newIdentity = dynamicParameters.Get<int>("@employee_id");
            return newIdentity;
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            string query = @"UPDATE dbo.Employee SET 
                            EmployeeName = @EmployeeName,
                            Department = @Department,
                            DateOfJoining = @DateOfJoining,
                            PhotoFileName = @PhotoFileName
                            WHERE EmployeeId = @EmployeeId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, employee);
            return res;
        }

        public async Task<int> DeleteEmployee(int employeeId)
        {
            string query = @"DELETE FROM dbo.Employee WHERE EmployeeId = @EmployeeId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { EmployeeId = employeeId });
            return res;
        }
    }
}
