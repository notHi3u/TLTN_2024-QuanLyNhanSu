namespace EMS.Domain.Models.EM
{
    public class WorkHistory
    {
        public long Id { get; set; } // Mã bản lưu
        public required string EmployeeId { get; set; } // Mã nhân viên
        public required string Position { get; set; } // Vị trí công việc
        public required string DepartmentId { get; set; }
        public DateOnly StartDate { get; set; } // Ngày bắt đầu làm việc
        public DateOnly EndDate { get; set; } // Ngày kết thúc hợp đồng

        public virtual Employee Employee { get; set; }
    }
}
