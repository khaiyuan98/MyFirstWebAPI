namespace Test.DataAccess.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public int UserRoleId { get; set; }
        public UserRole? UserRole { get; set; }
        public List<UserGroup>? UserGroups {get; set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int LastUpdatedById { get; set; }
        public User? LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
