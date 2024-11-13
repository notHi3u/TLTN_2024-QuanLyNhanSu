namespace EMS.Application.DTOs.EM
{
    public class WorkRecordRequestDto
    {
        public required string EmployeeId { get; set; } // Mã nhân viên
        public required string Position { get; set; } // Vị trí công việc
        public required string DepartmentId { get; set; } // Mã phòng ban
        public DateOnly StartDate { get; set; } // Ngày bắt đầu làm việc
        public DateOnly EndDate { get; set; } // Ngày kết thúc hợp đồng
    }
}
