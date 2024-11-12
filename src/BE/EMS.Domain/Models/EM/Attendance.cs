using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.EM
{
    public class Attendance
    {
        [Key] // Optionally mark the Id as the primary key
        public long Id { get; set; }

        [Required] // Ensures that the EmployeeId is always provided
        public required string EmployeeId { get; set; }

        [Required] // Ensures the Date is always provided
        public required DateOnly Date { get; set; }

        [Required] // Ensures the WorkStatus is always provided
        public required bool WorkStatus { get; set; }

        [MaxLength(500)] // Limiting the length of AbsentReasons (optional, adjust based on your needs)
        public string? AbsentReasons { get; set; }

        public bool Status { get; set; } = false; // Default value of Status is false

        [Required] // Ensures that TimeCardId is always provided
        public long TimeCardId { get; set; }

        // Navigation properties (no data annotations needed here)
        public virtual Employee Employee { get; set; }
        public virtual TimeCard TimeCard { get; set; }
    }
}
