namespace Test.DataAccess.Models.Users
{
    public class FullUser
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
        public UserRole? UserRole { get; set; }
        public List<UserGroup>? UserGroups { get; set; }
        public FullUser? LastUpdatedBy { get; set; }
    }
}
