using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.EM
{
    public class LeaveBalance
    {
        [Key] // Marks Id as the primary key
        public long Id { get; set; }

        [Required] // Ensures EmployeeId is always provided
        public required string EmployeeId { get; set; }

        [Range(1900, int.MaxValue)] // Ensures the year is a valid number, typically no year should be before 1900
        public int Year { get; set; }

        [Range(0, int.MaxValue)] // Ensures UsedLeaveDays is a non-negative integer
        public int UsedLeaveDays { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
