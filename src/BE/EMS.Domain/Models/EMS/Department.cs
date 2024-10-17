using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementService.Domain.Models
{
    public class Department
    {
        public required string Id { get; set; } // Mã phòng ban
        public required string DepartmentName { get; set; } // Tên phòng ban
        public string? DepartmentManagerId { get; set; } // Mã quản lý phòng ban
    }
}
