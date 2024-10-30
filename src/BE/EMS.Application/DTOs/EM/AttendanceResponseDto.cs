using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.DTOs.EM
{
    public class AttendanceResponseDto
    {
        public long Id { get; set; }
        public required string EmployeeId { get; set; }
        public required DateOnly Date { get; set; }
        public required bool WorkStatus { get; set; }
        public string? AbsentReasons { get; set; }
        public bool Status { get; set; }
        public long TimeCardId { get; set; }
    }
}
