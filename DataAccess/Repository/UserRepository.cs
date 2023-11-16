using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DataAccess.Data;
using Test.Shared.Models;

namespace Test.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess _db;

        public UserRepository(ISqlDataAccess myDb)
        {
            _db = myDb;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            string query = @"SELECT UserId, Username, PasswordHash, PasswordSalt FROM dbo.Users WHERE Username = @Username;";

            using IDbConnection connection = _db.OpenConnection();
            return (await connection.QueryAsync<User>(query, new { Username = username })).FirstOrDefault();
        }

        public async Task<int> AddUser(User user)
        {
            string query = @"INSERT INTO dbo.Users(Username, PasswordHash, PasswordSalt) 
                            VALUES(@Username, @PasswordHash, @PasswordSalt);
                            SELECT @user_id = SCOPE_IDENTITY();";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.AddDynamicParams(user);
            dynamicParameters.Add("@user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, dynamicParameters);

            int newIdentity = dynamicParameters.Get<int>("@user_id");
            return newIdentity;
        }
    }
}
