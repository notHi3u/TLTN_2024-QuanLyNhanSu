namespace EMS.Application.DTOs.EM
{
    public class SalaryRecordRequestDto
    {
        public required string EmployeeId { get; set; } // Mã nhân viên
        public int Month { get; set; } // Tháng
        public int Year { get; set; } // Năm
        public decimal BaseSalary { get; set; } // Lương cơ bản
        public decimal Bonus { get; set; } // Thưởng
        public decimal Deductions { get; set; } // Khấu trừ
        public decimal NetSalary { get; set; } // Lương thực nhận
    }
}
