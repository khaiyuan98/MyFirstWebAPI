using System.ComponentModel.DataAnnotations;

namespace Test.WebAPI.Models.Employee
{
    public class NewEmployeeDto
    {
        [Required]
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }
    }
}
