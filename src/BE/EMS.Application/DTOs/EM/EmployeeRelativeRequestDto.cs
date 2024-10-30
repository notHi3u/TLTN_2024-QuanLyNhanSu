namespace EMS.Application.DTOs.EM
{
    public class EmployeeRelativeRequestDto
    {
        public required string EmployeeId { get; set; } // Mã nhân viên
        public string? LastName { get; set; } // Họ
        public string? FirstName { get; set; } // Tên
        public required string Relationship { get; set; } // Mối quan hệ
        public required string PhoneNumber { get; set; } // Số điện thoại
        public string? Address { get; set; } // Địa chỉ
        public bool EmergencyContact { get; set; } // Liên hệ khẩn cấp
    }
}
