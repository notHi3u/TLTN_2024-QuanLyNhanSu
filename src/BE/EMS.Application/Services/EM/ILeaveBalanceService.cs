using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ILeaveBalanceService
    {
        Task<LeaveBalanceResponseDto> GetLeaveBalanceByIdAsync(string id);
        Task<LeaveBalanceResponseDto> CreateLeaveBalanceAsync(LeaveBalanceRequestDto leaveBalanceRequestDto);
        Task<LeaveBalanceResponseDto> UpdateLeaveBalanceAsync(string id, LeaveBalanceRequestDto leaveBalanceRequestDto);
        Task<bool> DeleteLeaveBalanceAsync(string id);
        Task<PagedDto<LeaveBalanceResponseDto>> GetPagedLeaveBalancesAsync(LeaveBalanceFilter filter);
    }
}

