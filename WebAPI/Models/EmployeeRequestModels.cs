namespace Test.WebAPI.Models
{
    public class NewEmployeeRequest
    {
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}
