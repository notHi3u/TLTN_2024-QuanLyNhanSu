using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementService.Domain.Models
{
    public class TimeCard
    {
        public long Id { get; set; } // Mã bản ghi chấm công
        public int EmployeeId { get; set; } // Mã nhân viên
        public DateOnly WeekStartDate { get; set; } // Ngày đầu tuần
        public List<long>? AttendanceIds { get; set; }// Mã các ngày
        public DateTime SubmittedAt { get; set; } // Thời gian nộp
        public bool Status { get; set; } = false;// Trạng thái duyệt
    }
}
