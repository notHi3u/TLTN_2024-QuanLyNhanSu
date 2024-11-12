using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.EM
{
    public class WorkHistory
    {
        [Key] // Marks Id as the primary key
        public long Id { get; set; } // Mã bản lưu

        [Required] // Ensures EmployeeId is always provided
        public required string EmployeeId { get; set; } // Mã nhân viên

        [Required] // Ensures Position is always provided
        public required string Position { get; set; } // Vị trí công việc

        [Required] // Ensures DepartmentId is always provided
        public required string DepartmentId { get; set; }

        [Required] // Ensures StartDate is always provided
        public DateOnly StartDate { get; set; } // Ngày bắt đầu làm việc

        [Required] // Ensures EndDate is always provided
        public DateOnly EndDate { get; set; } // Ngày kết thúc hợp đồng

        // Navigation property
        public virtual Employee Employee { get; set; }
    }
}
