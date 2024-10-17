using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Models
{
    public class SalaryStatement
    {
        public long Id { get; set; } 
        public required string EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; } 
        public decimal BaseSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; } 
        public decimal NetSalary { get; set; }
    }
}
