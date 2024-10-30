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
    public class AttendanceService : IAttendanceService
    {
        public Task<AttendanceRequestDto> CreateAttendanceAsync(AttendanceRequestDto attendanceRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAttendanceAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceRequestDto> GetAttendanceByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedDto<AttendanceRequestDto>> GetPagedAttendancesAsync(PermissionFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceRequestDto> UpdateAttendanceAsync(string id, AttendanceRequestDto permissionRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
