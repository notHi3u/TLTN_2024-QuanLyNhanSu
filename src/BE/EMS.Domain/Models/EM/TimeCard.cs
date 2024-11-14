using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace EMS.Domain.Models.EM
{
    public class TimeCard
    {
        [Key] // Marks Id as the primary key
        public long Id { get; set; } // Mã bản ghi chấm công

        [Required] // Ensures EmployeeId is always provided
        public required string EmployeeId { get; set; } // Mã nhân viên

        [Required] // Ensures that the WeekStartDate is provided
        public DateOnly WeekStartDate { get; set; } // Ngày đầu tuần

        //// AttendanceIds should only be null if explicitly allowed
        //public List<long>? AttendanceIds { get; set; } // Mã các ngày

        [Required] // Ensures SubmittedAt is always provided
        public DateTime SubmittedAt { get; set; } // Thời gian nộp

        public TimeCardStatus? Status { get; set; } // Trạng thái duyệt

        // Navigation properties
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
