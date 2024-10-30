using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.EM
{
    public interface IAttendanceService
    {
        Task<AttendanceRequestDto> GetAttendanceByIdAsync(string id);
        Task<AttendanceRequestDto> CreateAttendanceAsync(AttendanceRequestDto attendanceRequestDto);
        Task<AttendanceRequestDto> UpdateAttendanceAsync(string id, AttendanceRequestDto permissionRequestDto);
        Task<bool> DeleteAttendanceAsync(string id);
        Task<PagedDto<AttendanceRequestDto>> GetPagedAttendancesAsync(PermissionFilter filter);
    }
}
