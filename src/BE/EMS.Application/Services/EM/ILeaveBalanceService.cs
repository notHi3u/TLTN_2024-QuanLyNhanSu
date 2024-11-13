using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ILeaveBalanceService
    {
        Task<LeaveBalanceResponseDto> GetLeaveBalanceByIdAsync(long id);
        Task<LeaveBalanceResponseDto> CreateLeaveBalanceAsync(LeaveBalanceRequestDto leaveBalanceRequestDto);
        Task<LeaveBalanceResponseDto> UpdateLeaveBalanceAsync(long id, LeaveBalanceRequestDto leaveBalanceRequestDto);
        Task<bool> DeleteLeaveBalanceAsync(long id);
        Task<PagedDto<LeaveBalanceResponseDto>> GetPagedLeaveBalancesAsync(LeaveBalanceFilter filter);
    }
}

