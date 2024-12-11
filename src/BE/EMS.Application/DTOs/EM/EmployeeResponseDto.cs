using Common.Enums;
using EMS.Application.DTOs.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Models.EM;

namespace EMS.Application.DTOs.EM
{
    public class EmployeeResponseDto
    {
        public string Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? HireDate { get; set; }
        public DateOnly? FiredDate { get; set; }
        public string? Position { get; set; }
        public EmployeeStatus? Status { get; set; }
        public string? MaritalStatus { get; set; }
        public string? EducationLevel { get; set; }
        public string? IdNumber { get; set; }
        public string? DepartmentId { get; set; }
        public string? TaxId { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string? ImageUrl { get; set; }

        // Salary Information
        public decimal BaseSalary { get; set; } // Base salary must be a positive value

        public decimal Bonuses { get; set; } // Flat bonus must be a positive value

        public decimal Deductions { get; set; } // Deductions must be a positive value

        public decimal NetSalary { get; set; }

        // Collections for related data
        public ICollection<TimeCardResponseDto> TimeCards { get; set; }
        public ICollection<LeaveRequestResponseDto> LeaveRequests { get; set; }
        public ICollection<LeaveBalanceResponseDto> LeaveBalances { get; set; }
        public ICollection<AttendanceResponseDto> Attendances { get; set; }
        public ICollection<EmployeeRelativeResponseDto> EmployeeRelatives { get; set; }
        public ICollection<WorkRecordResponseDto> WorkRecords { get; set; }
        public ICollection<SalaryRecordResponseDto> SalaryRecords { get; set; }
        public DepartmentResponseDto Department {  get; set; } 
        public UserResponseDto User { get; set; }
        public DepartmentResponseDto ManagedDepartment { get; set; }
        public WorkRecordResponseDto WorkRecord { get; set; }
    }

}
