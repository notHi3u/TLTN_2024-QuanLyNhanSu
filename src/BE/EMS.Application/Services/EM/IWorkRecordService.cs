using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IWorkRecordService
    {
        Task<WorkRecordResponseDto> GetWorkHistoryByIdAsync(long id);
        Task<WorkRecordResponseDto> CreateWorkHistoryAsync(WorkRecordRequestDto workHistoryRequestDto);
        Task<WorkRecordResponseDto> UpdateWorkHistoryAsync(long id, WorkRecordRequestDto workHistoryRequestDto);
        Task<bool> DeleteWorkHistoryAsync(long id);
        Task<PagedDto<WorkRecordResponseDto>> GetPagedWorkHistoriesAsync(WorkRecordFilter filter);
    }
}
