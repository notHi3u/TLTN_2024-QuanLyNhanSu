namespace EMS.Application.DTOs.EM
{
    public class LeaveRequestRequestDto
    {
        public required string EmployeeId { get; set; } // Mã nhân viên
        public required string LeaveType { get; set; } // Loại nghỉ
        public DateOnly StartDate { get; set; } // Ngày bắt đầu
        public DateOnly EndDate { get; set; } // Ngày kết thúc
    }
}
