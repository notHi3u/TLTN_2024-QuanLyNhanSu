using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IWorkRecordService
    {
        Task<WorkRecordResponseDto> GetWorkRecordByIdAsync(long id); // Changed "History" to "Record"
        Task<WorkRecordResponseDto> CreateWorkRecordAsync(WorkRecordRequestDto workRecordRequestDto); // Changed "History" to "Record"
        Task<WorkRecordResponseDto> UpdateWorkRecordAsync(long id, WorkRecordRequestDto workRecordRequestDto); // Changed "History" to "Record"
        Task<bool> DeleteWorkRecordAsync(long id); // Changed "History" to "Record"
        Task<PagedDto<WorkRecordResponseDto>> GetPagedWorkRecordsAsync(WorkRecordFilter filter); // Changed "Histories" to "Records"
    }
}
