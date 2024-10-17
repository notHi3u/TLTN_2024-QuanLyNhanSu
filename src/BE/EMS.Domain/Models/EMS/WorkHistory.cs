using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementService.Domain.Models
{
    public class WorkHistory
    {
        public long Id { get; set; } // Mã bản lưu
        public required string EmployeeId { get; set; } // Mã nhân viên
        public required string Position { get; set; } // Vị trí công việc
        public required string DepartmentId { get; set; }
        public DateOnly StartDate { get; set; } // Ngày bắt đầu làm việc
        public DateOnly EndDate { get; set; } // Ngày kết thúc hợp đồng
    }
}
