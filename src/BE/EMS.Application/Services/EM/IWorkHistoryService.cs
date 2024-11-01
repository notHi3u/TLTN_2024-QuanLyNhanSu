using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IWorkHistoryService
    {
        Task<WorkHistoryResponseDto> GetWorkHistoryByIdAsync(string id);
        Task<WorkHistoryResponseDto> CreateWorkHistoryAsync(WorkHistoryRequestDto workHistoryRequestDto);
        Task<WorkHistoryResponseDto> UpdateWorkHistoryAsync(string id, WorkHistoryRequestDto workHistoryRequestDto);
        Task<bool> DeleteWorkHistoryAsync(string id);
        Task<PagedDto<WorkHistoryResponseDto>> GetPagedWorkHistoriesAsync(WorkHistoryFilter filter);
    }
}
