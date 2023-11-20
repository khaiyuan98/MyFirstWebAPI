using Dapper;
using System.Data;
using Test.DataAccess.DataAccess;
using Test.DataAccess.Models;

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

        public async Task<User?> GetUserById(int id)
        {
            string query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, RefreshToken, LastLoginDate FROM dbo.Users WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            return (await connection.QueryAsync<User>(query, new { UserId = id })).FirstOrDefault();
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

        public async Task<int> LoginUser(int UserId, string newRefreshToken, DateTime newLoginDate, DateTime newLoginExpires)
        {
            string query = @"UPDATE dbo.Users SET RefreshToken = @RefreshToken, RefreshTokenExpires = @RefreshTokenExpires, LastLoginDate = @LastLoginDate WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { RefreshToken = newRefreshToken, RefreshTokenExpires = newLoginExpires, LastLoginDate = newLoginDate, UserId = UserId});
            return res;
        }

        public async Task<User?> GetUserByRefreshToken(string refreshToken)
        {
            string query = @"SELECT UserId, Username, PasswordHash, PasswordSalt, RefreshToken, LastLoginDate FROM dbo.Users 
                             WHERE RefreshToken = @RefreshToken AND RefreshTokenExpires > GETDATE();";

            using IDbConnection connection = _db.OpenConnection();
            return (await connection.QueryAsync<User>(query, new { RefreshToken = refreshToken })).FirstOrDefault();
        }

        public async Task<int> LogoutUser(int UserId)
        {
            string query = @"UPDATE dbo.Users SET RefreshToken = NULL, RefreshTokenExpires = GETDATE(), WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { UserId = UserId });
            return res;
        }

    }
}
