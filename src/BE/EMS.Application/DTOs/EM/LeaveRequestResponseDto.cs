namespace EMS.Application.DTOs.EM
{
    public class LeaveRequestResponseDto
    {
        public long Id { get; set; } // ID
        public required string EmployeeId { get; set; } // Mã nhân viên
        public required string LeaveType { get; set; } // Loại nghỉ
        public DateOnly StartDate { get; set; } // Ngày bắt đầu
        public DateOnly EndDate { get; set; } // Ngày kết thúc
        public virtual EmployeeResponseDto Employee { get; set; } // Thông tin nhân viên
    }
}
