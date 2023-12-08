namespace Test.WebAPI.Models.User
{
    public class GetUserDto
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsActive { get; set; }
        public string? RoleName { get; set; }
        public string? LastUpdatedBy { get; set; }
        public List<string>? UserGroups { get; set; }
    }
}
