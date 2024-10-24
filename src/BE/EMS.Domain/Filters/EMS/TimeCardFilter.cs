using Common.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Filters.EMS
{
    public class TimeCardFilter: FilterBase
    {
        public string? EmployeeId { get; set; }
        public DateOnly? WeekStartDate { get; set; }
    }
}
