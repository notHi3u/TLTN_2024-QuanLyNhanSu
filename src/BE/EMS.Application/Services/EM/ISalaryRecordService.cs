using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ISalaryRecordService
    {
        Task<SalaryRecordResponseDto> GetSalaryHistoryByIdAsync(long id);
        Task<SalaryRecordResponseDto> CreateSalaryHistoryAsync(SalaryRecordRequestDto salaryHistoryRequestDto);
        Task<SalaryRecordResponseDto> UpdateSalaryHistoryAsync(long id, SalaryRecordRequestDto salaryHistoryRequestDto);
        Task<bool> DeleteSalaryHistoryAsync(long id);
        Task<PagedDto<SalaryRecordResponseDto>> GetPagedSalaryHistoriesAsync(SalaryRecordFilter filter);
        Task<ICollection<SalaryRecordResponseDto>> GetSalaryHistoryByEmployeeIdAsync(string id);
    }
}
