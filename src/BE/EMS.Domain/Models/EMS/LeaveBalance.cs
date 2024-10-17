using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Models
{
    public class LeaveBalance
    {
        public required string EmployeeId { get; set; }
        public int Year {  get; set; }
        public int LeaveDayCount { get; set; }
        public int UsedLeaveDays { get; set; }

    }
}
