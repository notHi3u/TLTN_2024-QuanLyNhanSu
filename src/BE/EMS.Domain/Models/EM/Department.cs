using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EMS.Domain.Models.EM
{
    public class Department
    {
        [Key] // Optionally mark the Id as the primary key
        public required string Id { get; set; } // Mã phòng ban

        [Required] // Ensures the DepartmentName is always provided
        [MaxLength(100)] // Limiting the length of DepartmentName (adjust as necessary)
        public required string DepartmentName { get; set; } // Tên phòng ban

        [StringLength(50)] // Optional: Limit the length of the DepartmentManagerId (adjust based on your needs)
        public string? DepartmentManagerId { get; set; } // Mã quản lý phòng ban

        // Navigation properties (no data annotations needed here)
        [JsonIgnore]
        public virtual ICollection<Employee>? Employees { get; set; } // One Department has many Employees
        [JsonIgnore]
        public virtual Employee? Manager { get; set; } // One Department has one Manager (Employee)

        public decimal TotalSalary => Employees?.Sum(e => e.Salary ?? 0) ?? 0;
    }
}
