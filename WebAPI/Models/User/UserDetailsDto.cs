namespace Test.WebAPI.Models.User
{
    public class UserDetailsDto
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
        public UserRoleDto? UserRole { get; set; }
        public List<UserGroupDto>? UserGroups { get; set; }
        public string? LastUpdatedBy { get; set; }
    }
}
