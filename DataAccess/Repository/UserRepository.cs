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

        public async Task<IEnumerable<User>> GetUsers()
        {
            string userQuery = @"SELECT * FROM dbo.Users;";
            string roleQuery = @"SELECT * FROM dbo.UserRole;";
            string groupQuery = @"SELECT * FROM dbo.UserGroup;";
            string userGroupMapQuery = @"SELECT * FROM dbo.User_UserGroup_Map;";

            using IDbConnection connection = _db.OpenConnection();
            IEnumerable<User> users = await connection.QueryAsync<User>(userQuery);
            IEnumerable<UserRole> userRoles = await connection.QueryAsync<UserRole>(roleQuery);
            IEnumerable<UserGroup> userGroups = await connection.QueryAsync<UserGroup>(groupQuery);
            IEnumerable<UserGroupMap> userGroupMaps = await connection.QueryAsync<UserGroupMap>(userGroupMapQuery);

            foreach (User user in users)
            {
                // Get LastUpdatedBy
                user.LastUpdatedBy = users.FirstOrDefault(person => person.UserId == user.LastUpdatedById);

                // Get UserRole
                user.UserRole = userRoles.FirstOrDefault(role => role.RoleId == user.UserRoleId);

                // Get UserGroups
                IEnumerable<int> userGroupIds = userGroupMaps
                    .Where(map => map.UserId == user.UserId)
                    .Select(map => map.UserGroupId);

                if (userGroupIds is not null)
                    user.UserGroups = userGroups.Where(group => userGroupIds.Contains(group.UserGroupId)).ToList();
            }

            return users;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            string query = @"SELECT * FROM dbo.Users WHERE Username = @Username;";

            using IDbConnection connection = _db.OpenConnection();
            return (await connection.QueryAsync<User>(query, new { Username = username })).FirstOrDefault();
        }

        public async Task<User?> GetUserById(int id)
        {
            string query = @"SELECT * FROM dbo.Users WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            return (await connection.QueryAsync<User>(query, new { UserId = id })).FirstOrDefault();
        }

        public async Task<int> AddUser(User user)
        {
            string query = @"INSERT INTO dbo.Users(Username, FirstName, LastName, UserRoleId, PasswordHash, PasswordSalt, LastUpdatedById) 
                            VALUES(@Username, @FirstName, @LastName, @UserRoleId, @PasswordHash, @PasswordSalt, @LastUpdatedById);
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
            string query = @"SELECT * FROM dbo.Users 
                             WHERE RefreshToken = @RefreshToken AND RefreshTokenExpires > GETDATE();";

            using IDbConnection connection = _db.OpenConnection();
            return (await connection.QueryAsync<User>(query, new { RefreshToken = refreshToken })).FirstOrDefault();
        }

        public async Task<int> LogoutUser(int UserId)
        {
            string query = @"UPDATE dbo.Users SET RefreshToken = NULL, RefreshTokenExpires = GETDATE() WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { UserId = UserId });
            return res;
        }

    }
}
