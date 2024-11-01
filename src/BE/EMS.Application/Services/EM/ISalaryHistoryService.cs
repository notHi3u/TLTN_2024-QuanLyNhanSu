using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ISalaryHistoryService
    {
        Task<SalaryHistoryResponseDto> GetSalaryHistoryByIdAsync(string id);
        Task<SalaryHistoryResponseDto> CreateSalaryHistoryAsync(SalaryHistoryRequestDto salaryHistoryRequestDto);
        Task<SalaryHistoryResponseDto> UpdateSalaryHistoryAsync(string id, SalaryHistoryRequestDto salaryHistoryRequestDto);
        Task<bool> DeleteSalaryHistoryAsync(string id);
        Task<PagedDto<SalaryHistoryResponseDto>> GetPagedSalaryHistoriesAsync(SalaryHistoryFilter filter);
    }
}
