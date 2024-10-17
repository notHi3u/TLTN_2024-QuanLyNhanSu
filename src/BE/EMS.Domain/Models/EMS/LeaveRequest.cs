using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Infrastructure.Repositories
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public required string EmployeeId { get; set; }
        public required string LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
