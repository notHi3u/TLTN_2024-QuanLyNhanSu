using System.ComponentModel.DataAnnotations;
using Common.Enums;
using EMS.Domain.Models.Account;

namespace EMS.Domain.Models.EM
{
    public class Employee
    {
        [Key] // Marks Id as the primary key
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Mã nhân viên (Primary Key)

        [Required] // Ensures LastName is always provided
        [MaxLength(100)] // Limits the LastName to 100 characters
        public required string LastName { get; set; } // Họ

        [Required] // Ensures FirstName is always provided
        [MaxLength(100)] // Limits the FirstName to 100 characters
        public required string FirstName { get; set; } // Tên

        public DateOnly? DateOfBirth { get; set; } // Ngày sinh

        [Required] // Ensures Gender is always provided
        public required Gender Gender { get; set; } // Giới tính

        [Required] // Ensures Nationality is always provided
        [MaxLength(50)] // Limits Nationality to 50 characters
        public required string Nationality { get; set; } // Quốc tịch

        [MaxLength(250)] // Optional: Limits Address to 250 characters
        public string? Address { get; set; } // Địa chỉ

        [Phone] // Validates that PhoneNumber is in the correct format
        [MaxLength(15)] // Optional: Limits PhoneNumber to 15 characters
        public string? PhoneNumber { get; set; } // Số điện thoại 

        [Required] // Ensures HireDate is always provided
        public DateOnly HireDate { get; set; } // Ngày vào làm

        public DateOnly? FiredDate { get; set; } // Ngày rời cty (nullable)

        [MaxLength(100)] // Limits Position to 100 characters
        public string? Position { get; set; } // Vị trí công việc của nhân viên

        public EmployeeStatus? Status { get; set; } // Tình trạng hoạt động

        [MaxLength(50)] // Optional: Limits MaritalStatus to 50 characters
        public string? MaritalStatus { get; set; } // Tình trạng hôn nhân

        [MaxLength(100)] // Limits EducationLevel to 100 characters
        public string? EducationLevel { get; set; } // Trình độ học vấn cao nhất

        [MaxLength(20)] // Limits IdNumber to 20 characters
        public string? IdNumber { get; set; } // Mã số CCCD hoặc Passport

        public string? DepartmentId { get; set; } // Mã phòng ban (Foreign Key)

        [MaxLength(15)] // Limits TaxId to 15 characters
        public string? TaxId { get; set; } // Mã số thuế thu nhập cá nhân

        [Required] // Ensures Email is always provided
        [EmailAddress] // Ensures Email is in a valid format
        [MaxLength(100)] // Limits Email to 100 characters
        public required string Email { get; set; } // Email

        public string? UserId { get; set; } // Tk đc tạo sau nv, có thể đổi tk cho nv nếu mất tk

        // Navigation properties
        public virtual User User { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<EmployeeRelative> EmployeeRelatives { get; set; }
        public virtual ICollection<WorkRecord> WorkRecord { get; set; }
        public virtual Department Department { get; set; }
        public virtual Department ManagedDepartment { get; set; }

        // Merged Salary Information
        [Range(0, double.MaxValue, ErrorMessage = "Base salary must be a positive value.")]
        public decimal? BaseSalary { get; set; } // Base salary must be a positive value

        [Range(0, 100, ErrorMessage = "Percent bonus must be between 0 and 100.")]
        public decimal? PercentBonus { get; set; } // Percent bonus should be between 0 and 100

        [Range(0, double.MaxValue, ErrorMessage = "Flat bonus must be a positive value.")]
        public decimal? FlatBonus { get; set; } // Flat bonus must be a positive value

        [Range(0, double.MaxValue, ErrorMessage = "Deductions must be a positive value.")]
        public decimal? Deductions { get; set; } // Deductions must be a positive value

        // Navigation property for salary history
        public virtual ICollection<SalaryRecord> SalaryRecords { get; set; } // Historical salary data
    }
}
