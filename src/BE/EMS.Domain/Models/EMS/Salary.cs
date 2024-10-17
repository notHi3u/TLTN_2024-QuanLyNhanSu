using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Models
{
    public class Salary
    {
        public required string EmployeeId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal PercentBonus { get; set; }
        public decimal FlatBonus { get; set; }
    }
}
