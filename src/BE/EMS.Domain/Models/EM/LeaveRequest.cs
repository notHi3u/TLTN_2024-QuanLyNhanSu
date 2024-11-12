using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.EM
{
    public class LeaveRequest
    {
        [Key] // Marks Id as the primary key
        public long Id { get; set; }

        [Required] // Ensures EmployeeId is always provided
        public required string EmployeeId { get; set; }

        [Required] // Ensures LeaveType is always provided
        public required string LeaveType { get; set; }

        [DataType(DataType.Date)] // Specifies that StartDate should be treated as a date
        [Required] // Ensures StartDate is always provided
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)] // Specifies that EndDate should be treated as a date
        [Required] // Ensures EndDate is always provided
        public DateOnly EndDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
