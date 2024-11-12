using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.Account;
using EMS.Domain.Filters.EMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.EM
{
    public interface IAttendanceService
    {
        Task<AttendanceResponseDto> GetAttendanceByIdAsync(long id);
        Task<AttendanceResponseDto> CreateAttendanceAsync(AttendanceRequestDto attendanceRequestDto);
        Task<AttendanceResponseDto> UpdateAttendanceAsync(long id, AttendanceRequestDto attendanceRequestDto);
        Task<bool> DeleteAttendanceAsync(long id);
        Task<PagedDto<AttendanceResponseDto>> GetPagedAttendancesAsync(AttendanceFilter filter);
        Task<IEnumerable<AttendanceResponseDto>> GetAttendancesByEmployIdAsync(string employeeId);
        Task<IEnumerable<AttendanceResponseDto>> GetAttendancesByTimeCardIdAsync(long timeCardId);
        Task<int> DeleteBulkAsync(IEnumerable<long> ids);
    }
}
