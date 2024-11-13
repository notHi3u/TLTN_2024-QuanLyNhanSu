using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IHolidayLeavePolicyService
    {
        Task<HolidayLeavePolicyResponseDto> GetHolidayLeavePolicyByIdAsync(int id);
        Task<HolidayLeavePolicyResponseDto> CreateHolidayLeavePolicyAsync(HolidayLeavePolicyRequestDto holidayLeavePolicyRequestDto);
        Task<HolidayLeavePolicyResponseDto> UpdateHolidayLeavePolicyAsync(int id, HolidayLeavePolicyRequestDto holidayLeavePolicyRequestDto);
        Task<bool> DeleteHolidayLeavePolicyAsync(int id);
        Task<PagedDto<HolidayLeavePolicyResponseDto>> GetPagedHolidayLeavePoliciesAsync(HolidayLeavePolicyFilter filter);
    }
}
