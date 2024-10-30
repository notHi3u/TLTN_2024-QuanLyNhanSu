namespace EMS.Application.DTOs.EM
{
    public class SalaryRequestDto
    {
        public required string EmployeeId { get; set; } // Mã nhân viên
        public decimal BaseSalary { get; set; } // Lương cơ bản
        public decimal PercentBonus { get; set; } // Phần trăm thưởng
        public decimal FlatBonus { get; set; } // Thưởng cố định
    }
}
