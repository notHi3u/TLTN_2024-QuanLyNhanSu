using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IHolidayLeavePolicyService
    {
        Task<HolidayLeavePolicyResponseDto> GetHolidayLeavePolicyByIdAsync(string id);
        Task<HolidayLeavePolicyResponseDto> CreateHolidayLeavePolicyAsync(HolidayLeavePolicyRequestDto holidayLeavePolicyRequestDto);
        Task<HolidayLeavePolicyResponseDto> UpdateHolidayLeavePolicyAsync(string id, HolidayLeavePolicyRequestDto holidayLeavePolicyRequestDto);
        Task<bool> DeleteHolidayLeavePolicyAsync(string id);
        Task<PagedDto<HolidayLeavePolicyResponseDto>> GetPagedHolidayLeavePoliciesAsync(HolidayLeavePolicyFilter filter);
    }
}
