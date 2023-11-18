using System.ComponentModel.DataAnnotations;

namespace Test.WebAPI.Models.Department
{
    public class NewDepartmentDto
    {
        [Required]
        public string DepartmentName { get; set; }
    }
}
