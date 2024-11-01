using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ISalaryService
    {
        Task<SalaryResponseDto> GetSalaryByIdAsync(string id);
        Task<SalaryResponseDto> CreateSalaryAsync(SalaryRequestDto salaryRequestDto);
        Task<SalaryResponseDto> UpdateSalaryAsync(string id, SalaryRequestDto salaryRequestDto);
        Task<bool> DeleteSalaryAsync(string id);
        Task<PagedDto<SalaryResponseDto>> GetPagedSalariesAsync(SalaryFilter filter);
    }
}
