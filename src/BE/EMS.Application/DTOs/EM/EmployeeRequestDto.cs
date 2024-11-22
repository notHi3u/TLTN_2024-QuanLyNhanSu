using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace EMS.Application.DTOs.EM
{
    public class EmployeeRequestDto
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? Position { get; set; }
        public EmployeeStatus? Status { get; set; }
        public string? MaritalStatus { get; set; }
        public string? EducationLevel { get; set; }
        public string? IdNumber { get; set; }
        public string? DepartmentId { get; set; } // null when create
        public string? TaxId { get; set; }
        public string? Email { get; set; }
        
        public decimal BaseSalary { get; set; } // Base salary must be a positive value

        public decimal Bonuses { get; set; } // Flat bonus must be a positive value

        public decimal Deductions { get; set; } // Deductions must be a positive value
    }
}
