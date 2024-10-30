using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.DTOs.EM
{
    public class LeaveBalanceResponseDto
    {
        public long Id { get; set; } // ID
        public required string EmployeeId { get; set; } // Mã nhân viên
        public int Year { get; set; } // Năm
        public int LeaveDayCount { get; set; } // Tổng số ngày nghỉ
        public int UsedLeaveDays { get; set; } // Số ngày nghỉ đã sử dụng
        public virtual EmployeeResponseDto Employee { get; set; } // Thông tin nhân viên
    }
}
