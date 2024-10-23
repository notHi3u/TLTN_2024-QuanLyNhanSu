namespace EMS.Domain.Models.EM
{
    public class SalaryHistory
    {
        public long Id { get; set; } 
        public required string EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; } 
        public decimal BaseSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; } 
        public decimal NetSalary { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
