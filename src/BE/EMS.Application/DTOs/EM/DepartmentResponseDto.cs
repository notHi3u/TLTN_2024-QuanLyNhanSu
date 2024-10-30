using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.DTOs.EM
{
    public class DepartmentResponseDto
    {
        public required string Id { get; set; } // Mã phòng ban
        public required string DepartmentName { get; set; } // Tên phòng ban
        public string? DepartmentManagerId { get; set; } // Mã quản lý phòng ban
        public virtual ICollection<EmployeeResponseDto> Employees { get; set; } // List of employees in the department
        public virtual EmployeeResponseDto Manager { get; set; } // Department manager details
    }
}
