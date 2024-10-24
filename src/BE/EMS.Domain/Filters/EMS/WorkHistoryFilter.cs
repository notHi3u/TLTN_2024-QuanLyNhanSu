using Common.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Filters.EMS
{
    public class WorkHistoryFilter: FilterBase
    {
        public string? EmployeeId { get; set; }
        public string? Position { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
