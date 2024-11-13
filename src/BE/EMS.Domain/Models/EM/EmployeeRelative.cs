using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.EM
{
    public class EmployeeRelative
    {
        [Key] // Marks Id as the primary key
        public int Id { get; set; }

        [Required] // Ensures EmployeeId is always provided
        public required string EmployeeId { get; set; }

        [MaxLength(100)] // Limits LastName to 100 characters
        public string? LastName { get; set; }

        [MaxLength(100)] // Limits FirstName to 100 characters
        public string? FirstName { get; set; }

        [Required] // Ensures Relationship is always provided
        [MaxLength(50)] // Limits Relationship to 50 characters
        public required string Relationship { get; set; }

        [Required] // Ensures PhoneNumber is always provided
        [Phone] // Validates that PhoneNumber is in the correct format
        [MaxLength(15)] // Limits PhoneNumber to 15 characters
        public required string PhoneNumber { get; set; }

        [MaxLength(250)] // Limits Address to 250 characters
        public string? Address { get; set; }

        public bool EmergencyContact { get; set; }

        // Navigation property
        public virtual Employee Employee { get; set; }
    }
}
