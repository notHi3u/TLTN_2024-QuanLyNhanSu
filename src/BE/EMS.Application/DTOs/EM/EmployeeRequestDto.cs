using Common.Enums;

namespace EMS.Application.DTOs.EM
{
    public class EmployeeRequestDto
    {
        public required string LastName { get; set; } // Họ
        public required string FirstName { get; set; } // Tên
        public DateOnly? DateOfBirth { get; set; } // Ngày sinh
        public required string Gender { get; set; } // Giới tính
        public required string Nationality { get; set; } // Quốc tịch
        public string? Address { get; set; } // Địa chỉ
        public string? PhoneNumber { get; set; } // Số điện thoại 
        public DateOnly HireDate { get; set; } // Ngày vào làm
        public DateOnly? FiredDate { get; set; } // Ngày rời cty
        public string? Position { get; set; } // Vị trí công việc
        public EmployeeStatus? Status { get; set; } // Tình trạng hoạt động
        public string? MaritalStatus { get; set; } // Tình trạng hôn nhân
        public string? EducationLevel { get; set; } // Trình độ học vấn
        public string? IdNumber { get; set; } // Mã số CCCD hoặc Passport
        public string? DepartmentId { get; set; } // Mã phòng ban
        public string? TaxId { get; set; } // Mã số thuế
        public required string Email { get; set; } // Email
        public string? UserId { get; set; } // Tài khoản người dùng
    }
}
