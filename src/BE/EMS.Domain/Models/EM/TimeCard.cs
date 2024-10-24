namespace EMS.Domain.Models.EM
{
    public class TimeCard
    {
        public long Id { get; set; } // Mã bản ghi chấm công
        public required string EmployeeId { get; set; } // Mã nhân viên
        public DateOnly WeekStartDate { get; set; } // Ngày đầu tuần
        public List<long>? AttendanceIds { get; set; }// Mã các ngày
        public DateTime SubmittedAt { get; set; } // Thời gian nộp
        public TimeCardStatus? Status { get; set; }// Trạng thái duyệt

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public enum TimeCardStatus
    {
        Pending,   // 0
        Approved,  // 1
        Rejected   // 2
    }

}
