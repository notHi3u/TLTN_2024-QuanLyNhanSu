namespace EMS.Domain.Models.EM
{
    public class Salary
    {
        public int Id { get; set; }  // Primary key
        public required string EmployeeId { get; set; }  // Foreign key to Employee
        public decimal BaseSalary { get; set; }
        public decimal PercentBonus { get; set; }
        public decimal FlatBonus { get; set; }

        public virtual Employee Employee { get; set; } // Navigation property
    }
}
