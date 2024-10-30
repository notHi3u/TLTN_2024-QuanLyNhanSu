using Common.Enums;

namespace EMS.Application.DTOs.EM
{
    public class TimeCardResponseDto
    {
        public long Id { get; set; } // Mã bản ghi chấm công
        public required string EmployeeId { get; set; } // Mã nhân viên
        public DateOnly WeekStartDate { get; set; } // Ngày đầu tuần
        public List<long>? AttendanceIds { get; set; } // Mã các ngày
        public DateTime SubmittedAt { get; set; } // Thời gian nộp
        public TimeCardStatus? Status { get; set; } // Trạng thái duyệt
        public virtual EmployeeResponseDto Employee { get; set; } // Thông tin nhân viên
        public virtual ICollection<AttendanceResponseDto> Attendances { get; set; } // Thông tin chấm công
    }
}
