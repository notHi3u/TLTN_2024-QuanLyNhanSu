using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequestResponseDto> GetLeaveRequestByIdAsync(long id);
        Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(LeaveRequestRequestDto leaveRequestRequestDto);
        Task<LeaveRequestResponseDto> UpdateLeaveRequestAsync(long id, LeaveRequestRequestDto leaveRequestRequestDto);
        Task<bool> DeleteLeaveRequestAsync(long id);
        Task<PagedDto<LeaveRequestResponseDto>> GetPagedLeaveRequestsAsync(LeaveRequestFilter filter);
    }
}
