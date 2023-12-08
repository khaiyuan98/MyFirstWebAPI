using Dapper;
using System.Data;
using Test.DataAccess.DataAccess;
using Test.DataAccess.Models.Users;
using static Dapper.SqlMapper;

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
            string query = @"SELECT usr.UserId, usr.Username, usr.FirstName, usr.LastName, usr.LastLoginDate, usr.LastUpdated, usr.IsActive,
                            role.Description AS 'RoleName',
                            updatedby.Username AS 'LastUpdatedBy'
                            FROM dbo.Users usr
                            LEFT JOIN dbo.UserRole role ON usr.UserRoleId = role.UserRoleId
                            LEFT JOIN dbo.Users updatedby ON usr.LastUpdatedById = updatedby.UserId;

                            SELECT * FROM dbo.UserGroup;
                            SELECT * FROM dbo.User_UserGroup_Map;";

            using IDbConnection connection = _db.OpenConnection();
            GridReader? res = await connection.QueryMultipleAsync(query);

            IEnumerable<User> users = res.Read<User>();
            IEnumerable<UserGroup> userGroups = res.Read<UserGroup>();
            IEnumerable<UserGroupMap> userGroupMaps = res.Read<UserGroupMap>();

            foreach (User user in users)
            {
                // Get UserGroups
                IEnumerable<int> userGroupIds = userGroupMaps
                    .Where(map => map.UserId == user.UserId)
                    .Select(map => map.UserGroupId);

                if (userGroupIds is not null)
                    user.UserGroups = userGroups.Where(group => userGroupIds.Contains(group.UserGroupId)).Select(group => group.Description!).ToList();
            }

            return users;
        }

        public async Task<FullUser?> GetUserById(int id)
        {
            string query = @"SELECT usr.UserId, usr.Username, usr.FirstName, usr.LastName, usr.PasswordHash, usr.PasswordSalt, 
                            usr.LastLoginDate, usr.LastUpdated, usr.IsActive,
                            role.UserRoleId, role.Description,
                            updatedby.UserId, updatedby.Username
                            FROM dbo.Users usr
                            LEFT JOIN dbo.UserRole role ON usr.UserRoleId = role.UserRoleId
                            LEFT JOIN dbo.Users updatedby ON usr.LastUpdatedById = updatedby.UserId
                            WHERE usr.UserId = @UserId;

                            SELECT usergroup.* 
                            FROM dbo.UserGroup usergroup
                            JOIN dbo.User_UserGroup_Map group_mapping ON usergroup.UserGroupId = group_mapping.UserGroupId
                            WHERE group_mapping.UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            GridReader? res = await connection.QueryMultipleAsync(query, new { UserId = id });

            FullUser? user = res.Read<FullUser, UserRole, FullUser, FullUser>((user, userRole, updatedBy) =>
            {
                user.UserRole = userRole;
                user.LastUpdatedBy = updatedBy;
                return user;
            }, splitOn: "UserRoleId, UserId").FirstOrDefault();

            if (user is null)
                return null;

            IEnumerable<UserGroup> userGroups = res.Read<UserGroup>();
            user.UserGroups = userGroups.ToList();

            return user;
        }

        public async Task<FullUser?> GetUserByUsername(string username)
        {
            string query = @"SELECT usr.UserId, usr.Username, usr.FirstName, usr.LastName, usr.PasswordHash, usr.PasswordSalt, 
                            usr.LastLoginDate, usr.LastUpdated, usr.IsActive,
                            role.UserRoleId, role.Description,
                            updatedby.UserId, updatedby.Username
                            FROM dbo.Users usr
                            LEFT JOIN dbo.UserRole role ON usr.UserRoleId = role.UserRoleId
                            LEFT JOIN dbo.Users updatedby ON usr.LastUpdatedById = updatedby.UserId
                            WHERE usr.Username = @Username;

                            SELECT usergroup.* 
                            FROM dbo.UserGroup usergroup
                            JOIN dbo.User_UserGroup_Map group_mapping ON usergroup.UserGroupId = group_mapping.UserGroupId
                            JOIN dbo.Users usr ON group_mapping.UserId = usr.UserId
                            WHERE usr.Username = @Username;";

            using IDbConnection connection = _db.OpenConnection();
            GridReader? res = await connection.QueryMultipleAsync(query, new { Username = username });

            FullUser? user = res.Read<FullUser, UserRole, FullUser, FullUser>((user, userRole, updatedBy) =>
            {
                user.UserRole = userRole;
                user.LastUpdatedBy = updatedBy;
                return user;
            }, splitOn: "UserRoleId, UserId").FirstOrDefault();

            if (user is null)
                return null;

            IEnumerable<UserGroup> userGroups = res.Read<UserGroup>();
            user.UserGroups = userGroups.ToList();

            return user;
        }

        public async Task<FullUser?> GetUserByRefreshToken(string refreshToken)
        {
            string query = @"SELECT usr.UserId, usr.Username, usr.FirstName, usr.LastName, usr.PasswordHash, usr.PasswordSalt, 
                            usr.LastLoginDate, usr.LastUpdated, usr.IsActive,
                            role.UserRoleId, role.Description,
                            updatedby.UserId, updatedby.Username
                            FROM dbo.Users usr
                            LEFT JOIN dbo.UserRole role ON usr.UserRoleId = role.UserRoleId
                            LEFT JOIN dbo.Users updatedby ON usr.LastUpdatedById = updatedby.UserId
                            WHERE usr.RefreshToken = @RefreshToken;

                            SELECT usergroup.* 
                            FROM dbo.UserGroup usergroup
                            JOIN dbo.User_UserGroup_Map group_mapping ON usergroup.UserGroupId = group_mapping.UserGroupId
                            JOIN dbo.Users usr ON group_mapping.UserId = usr.UserId
                            WHERE usr.RefreshToken = @RefreshToken;";

            using IDbConnection connection = _db.OpenConnection();
            GridReader? res = await connection.QueryMultipleAsync(query, new { RefreshToken = refreshToken });

            FullUser? user = res.Read<FullUser, UserRole, FullUser, FullUser>((user, userRole, updatedBy) =>
            {
                user.UserRole = userRole;
                user.LastUpdatedBy = updatedBy;
                return user;
            }, splitOn: "UserRoleId, UserId").FirstOrDefault();

            if (user is null)
                return null;

            IEnumerable<UserGroup> userGroups = res.Read<UserGroup>();
            user.UserGroups = userGroups.ToList();

            return user;
        }

        public async Task<int> InsertUser(AddUser user)
        {
            string insertUserQuery = @"INSERT INTO dbo.Users(Username, FirstName, LastName, UserRoleId, PasswordHash, PasswordSalt, LastUpdatedById, IsActive) 
                            VALUES(@Username, @FirstName, @LastName, @UserRoleId, @PasswordHash, @PasswordSalt, @LastUpdatedById, @IsActive);
                            SELECT @user_id = SCOPE_IDENTITY();";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.AddDynamicParams(user);
            dynamicParameters.Add("@user_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (IDbConnection connection = _db.OpenConnection())
            {
                connection.Open();
                using IDbTransaction? transaction = connection.BeginTransaction();

                int res = await connection.ExecuteAsync(insertUserQuery, dynamicParameters, transaction);
                int newIdentity = dynamicParameters.Get<int>("@user_id");

                // Set User Group Mapping
                string groupMappingQuery = "INSERT INTO dbo.User_UserGroup_Map(UserId, UserGroupId) VALUES (@UserId, @UserGroupId)";
                IEnumerable<UserGroupMap>? groupMappings = user.UserGroupIds?.Select(group => new UserGroupMap { UserId = newIdentity, UserGroupId = group });
                await connection.ExecuteAsync(groupMappingQuery, groupMappings, transaction);
                transaction.Commit();

                return newIdentity;
            }
        }
        
        public async Task<int> UpdateUser(UpdateUser user)
        {
            string updateUserQuery = @"UPDATE dbo.Users
                                        SET Username = @Username,
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        UserRoleId = @UserRoleId,
                                        PasswordHash = @PasswordHash,
                                        PasswordSalt = @PasswordSalt,
                                        LastUpdatedById = @LastUpdatedById,
                                        IsActive = @IsActive
                                        WHERE UserId = @UserId;";

            using (IDbConnection connection = _db.OpenConnection())
            {
                connection.Open();
                using IDbTransaction? transaction = connection.BeginTransaction();

                int res = await connection.ExecuteAsync(updateUserQuery, user, transaction);

                // Set User Group Mapping
                string deleteGroupMappingQuery = "DELETE FROM dbo.User_UserGroup_Map WHERE UserId = @UserId";
                await connection.ExecuteAsync(deleteGroupMappingQuery, new { user.UserId }, transaction);

                string groupMappingQuery = "INSERT INTO dbo.User_UserGroup_Map(UserId, UserGroupId) VALUES (@UserId, @UserGroupId)";
                IEnumerable<UserGroupMap>? groupMappings = user.UserGroupIds?.Select(group => new UserGroupMap { UserId = user.UserId, UserGroupId = group });
                await connection.ExecuteAsync(groupMappingQuery, groupMappings, transaction);
                transaction.Commit();

                return res;
            }
        }

        public async Task<int> DeleteUser(int UserId)
        {
            string query = $"DELETE FROM dbo.Users WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { UserId });
            return res;
        }

        public async Task<int> LoginUser(int UserId, string newRefreshToken, DateTime newLoginDate, DateTime newLoginExpires)
        {
            string query = @"UPDATE dbo.Users SET RefreshToken = @RefreshToken, RefreshTokenExpires = @RefreshTokenExpires, LastLoginDate = @LastLoginDate WHERE UserId = @UserId;";

            using IDbConnection connection = _db.OpenConnection();
            int res = await connection.ExecuteAsync(query, new { RefreshToken = newRefreshToken, RefreshTokenExpires = newLoginExpires, LastLoginDate = newLoginDate, UserId = UserId });
            return res;
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
