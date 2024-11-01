using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequestResponseDto> GetLeaveRequestByIdAsync(string id);
        Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(LeaveRequestRequestDto leaveRequestRequestDto);
        Task<LeaveRequestResponseDto> UpdateLeaveRequestAsync(string id, LeaveRequestRequestDto leaveRequestRequestDto);
        Task<bool> DeleteLeaveRequestAsync(string id);
        Task<PagedDto<LeaveRequestResponseDto>> GetPagedLeaveRequestsAsync(LeaveRequestFilter filter);
    }
}
