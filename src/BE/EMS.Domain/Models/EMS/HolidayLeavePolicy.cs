using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementService.Domain.Models
{
    public class HolidayLeavePolicy
    {
        public int Id { get; set; } // Mã quy định
        public int EffectiveYear { get; set; } // Năm áp dụng
        public List<DateOnly>? Holidays { get; set; } // Những ngày nghỉ
        public int HolidayCount { get; set; } // Số ngày nghỉ
    }
}
