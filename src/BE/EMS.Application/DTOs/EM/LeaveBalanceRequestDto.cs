namespace EMS.Application.DTOs.EM
{
    public class LeaveBalanceRequestDto
    {
        public required string EmployeeId { get; set; } // Mã nhân viên
        public int Year { get; set; } // Năm
        //public int UsedLeaveDays { get; set; } // Số ngày nghỉ đã sử dụng
    }
}
